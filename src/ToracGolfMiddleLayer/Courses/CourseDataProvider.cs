using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.Courses.Models;
using ToracGolf.MiddleLayer.Courses.Models.CourseStats;
using ToracGolf.MiddleLayer.Dashboard.Models;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracGolf.MiddleLayer.GridCommon;
using ToracGolf.MiddleLayer.ListingFactories;

namespace ToracGolf.MiddleLayer.Courses
{
    public static class CourseDataProvider
    {

        #region Course Add

        public static async Task<int> CourseAdd(ToracGolfContext dbContext, int userId, CourseAddEnteredData CourseData, string courseSavePath)
        {
            //course record to add
            var courseToAdd = new Course
            {
                Name = CourseData.CourseName,
                Description = CourseData.Description,
                IsActive = true,
                Pending = true,
                City = CourseData.City,
                StateId = Convert.ToInt32(CourseData.StateListing),
                UserIdThatCreatedCourse = userId,
                OnlyAllow18Holes = CourseData.OnlyAllow18Holes,
                CreatedDate = DateTime.Now
            };

            courseToAdd.CourseTeeLocations = CourseData.TeeLocations.Select((x, i) => new CourseTeeLocations
            {
                CourseId = courseToAdd.CourseId,
                Description = x.Description,
                TeeLocationSortOrderId = i,
                Front9Par = x.Front9Par.Value,
                Back9Par = x.Back9Par.Value,
                Rating = x.Rating.Value,
                Slope = x.Slope.Value,
                Yardage = x.Yardage.Value,
                FairwaysOnCourse = 18 - x.NumberOfPar3s.Value
            }).ToArray();

            //add the course to the context
            dbContext.Course.Add(courseToAdd);

            //go save it
            await dbContext.SaveChangesAsync();

            //do we have a course image?
            if (CourseData.CourseImage != null)
            {
                //grab the byte array for the file
                var fileToSave = ToracLibrary.AspNet.Graphics.GraphicsUtilities.ImageFromJsonBase64String(CourseData.CourseImage);

                //grab where we have the actual .jpg vs png
                var indexOfSlash = fileToSave.MimeType.IndexOf(@"/");

                //grab the file name to save
                var fileNameToSave = System.IO.Path.Combine(courseSavePath, string.Format("{0}.{1}", courseToAdd.CourseId, fileToSave.MimeType.Substring(indexOfSlash + 1, fileToSave.MimeType.Length - indexOfSlash - 1)));

                //go save the file
                System.IO.File.WriteAllBytes(fileNameToSave, fileToSave.FileBytes);
            }

            //return the course id
            return courseToAdd.CourseId;
        }

        #endregion

        #region Course Listing

        public static IQueryable<Course> CourseSelectQueryBuilder(ToracGolfContext dbContext, string courseNameFilter, int? StateFilter)
        {
            //build the queryable
            var queryable = dbContext.Course.AsNoTracking().Where(x => x.IsActive).AsQueryable();

            //if we have a course name, add it as a filter
            if (!string.IsNullOrEmpty(courseNameFilter))
            {
                queryable = queryable.Where(x => x.Name.Contains(courseNameFilter));
            }

            //do we have a state filter?
            if (StateFilter.HasValue)
            {
                queryable = queryable.Where(x => x.StateId == StateFilter.Value);
            }

            //return the queryable
            return queryable;
        }

        /// <param name="pageId">0 base index that holds what page we are on</param>
        public static async Task<IEnumerable<CourseListingData>> CourseSelect(IListingFactory<Course> courseListingFactory,
                                                                              ToracGolfContext dbContext,
                                                                              int pageId,
                                                                              CourseListingSortOrder.CourseListingSortEnum sortBy,
                                                                              string courseNameFilter,
                                                                              int? stateFilter,
                                                                              int recordsPerPage,
                                                                              int userId,
                                                                              CourseImageFinder courseImageFinder)
        {
            //go grab the query
            var queryable = CourseSelectQueryBuilder(dbContext, courseNameFilter, stateFilter);

            //go sort the data
            var sortedQueryable = courseListingFactory.SortByConfiguration[sortBy.ToString()](queryable, new ListingFactoryParameters(dbContext, userId)).ThenBy(x => x.CourseId);

            //add the secondary sort on now
            sortedQueryable = sortedQueryable.ThenBy(x => x.CourseId);

            //go run the query now
            var query = sortedQueryable.Select(x => new CourseListingData
            {
                CourseData = x,
                StateDescription = dbContext.Ref_State.FirstOrDefault(y => y.StateId == x.StateId).Description,
                TeeLocationCount = x.CourseTeeLocations.Count,

                NumberOfRounds = dbContext.Rounds.Count(y => y.CourseId == x.CourseId && y.UserId == userId),
                TopScore = dbContext.Rounds.Where(y => y.CourseId == x.CourseId && y.UserId == userId).Select(y => y.Score).Min(),
                WorseScore = dbContext.Rounds.Where(y => y.CourseId == x.CourseId && y.UserId == userId).Select(y => y.Score).Max(),
                AverageScore = dbContext.Rounds.Where(y => y.CourseId == x.CourseId && y.UserId == userId).Select(y => y.Score).Average(),

                FairwaysHit = dbContext.Rounds.Where(y => y.CourseId == x.CourseId && y.UserId == userId).Select(y => y.FairwaysHit).Sum(),
                FairwaysHitAttempted = dbContext.Rounds.Where(y => y.CourseId == x.CourseId && y.UserId == userId).Select(y => y.CourseTeeLocation.FairwaysOnCourse).Sum(),
                GreensInRegulation = dbContext.Rounds.Where(y => y.CourseId == x.CourseId && y.UserId == userId).Select(y => y.GreensInRegulation).Average(),
                NumberOfPutts = dbContext.Rounds.Where(y => y.CourseId == x.CourseId && y.UserId == userId).Select(y => y.Putts).Average()
            });

            //go execute it and return it
            var data = await EFPaging.PageEfQuery(query, pageId, recordsPerPage).ToListAsync();

            //go find the course paths
            data.ForEach(x => x.CourseImageLocation = courseImageFinder.FindCourseImage(x.CourseData.CourseId));

            //return the data set
            return data;
        }

        public static async Task<int> TotalNumberOfCourses(ToracGolfContext dbContext, string courseNameFilter, int? StateFilter)
        {
            return await CourseSelectQueryBuilder(dbContext, courseNameFilter, StateFilter).CountAsync();
        }

        public static async Task<Tuple<string, string>> CourseNameAndState(ToracGolfContext dbContext, int courseId)
        {
            //grab the course
            var course = await dbContext.Course.AsNoTracking().Where(x => x.CourseId == courseId).Select(x => new { x.Name, x.StateId }).FirstAsync();

            //return the tuple
            return new Tuple<string, string>(course.Name, course.StateId.ToString());
        }

        public static async Task<bool> DeleteACourse(ToracGolfContext dbContext, int courseId)
        {
            //grab the course
            var courseToDelete = await dbContext.Course.FirstAsync(x => x.CourseId == courseId);

            //now flip the flag on the course
            courseToDelete.IsActive = false;

            //save the changes
            await dbContext.SaveChangesAsync();

            //return a positive result
            return true;
        }

        #endregion

        #region Tee Box Selection

        public static async Task<int> TeeboxNumberOfFairways(ToracGolfContext dbContext, int courseTeeLocationId)
        {
            return (await dbContext.CourseTeeLocations.FirstAsync(x => x.CourseTeeLocationId == courseTeeLocationId)).FairwaysOnCourse;
        }

        #endregion

        #region Course Select

        public static async Task<CourseStatsModel> CourseStatsSelect(ToracGolfContext dbContext, int courseId, int userId, CourseImageFinder courseImageFinder)
        {
            var courseRecord = await dbContext.Course.AsNoTracking().Select(x => new CourseStatsModel
            {
                CourseId = x.CourseId,
                CourseCity = x.City,
                CourseDescription = x.Description,
                CourseName = x.Name,
                CourseState = x.State.Description,
                //CourseImage = x.CourseImage.CourseImage,
                TeeBoxLocations = x.CourseTeeLocations.Select(y => new TeeBoxData
                {
                    TeeLocationId = y.CourseTeeLocationId,
                    Name = y.Description,
                    Yardage = y.Yardage,
                    Par = y.Front9Par + y.Back9Par,
                    Rating = y.Rating,
                    Slope = y.Slope
                }).OrderBy(y => y.Yardage)
            }).FirstAsync(x => x.CourseId == courseId);

            //set the url path
            courseRecord.CourseImageUrl = courseImageFinder.FindCourseImage(courseRecord.CourseId);

            return courseRecord;
        }

        public static async Task<CourseStatsQueryResponse> CourseStatsQuery(ToracGolfContext dbContext, CourseStatsQueryRequest queryModel, int userId)
        {
            var query = dbContext.Rounds.AsNoTracking().Where(x => x.UserId == userId && x.CourseId == queryModel.CourseId);

            if (queryModel.SeasonId.HasValue)
            {
                query = query.Where(x => x.SeasonId == queryModel.SeasonId.Value);
            }

            if (queryModel.TeeBoxLocationId.HasValue)
            {
                query = query.Where(x => x.CourseTeeLocationId == queryModel.TeeBoxLocationId.Value);
            }

            if (query.Count() == 0)
            {
                return new CourseStatsQueryResponse { QuickStats = new CondensedStats { TeeBoxCount = dbContext.CourseTeeLocations.Count(y => y.CourseId == queryModel.CourseId) } };
            }

            //group by should chunk it up to 1 record
            var model = await query.GroupBy(x => x.UserId).Select(x => new CourseStatsQueryResponse
            {
                QuickStats = new CondensedStats
                {
                    RoundCount = x.Count(),
                    AverageScore = x.Average(y => y.Score),
                    BestScore = x.Min(y => y.Score),
                    TeeBoxCount = dbContext.CourseTeeLocations.Count(y => y.CourseId == queryModel.CourseId)
                }
            }).FirstAsync();

            model.RecentRounds = await RecentCourseStatRounds(dbContext, userId, queryModel.CourseId, queryModel.SeasonId, queryModel.TeeBoxLocationId);
            model.ScoreGraphData = await ScoreGraph(dbContext, userId, queryModel.CourseId, queryModel.SeasonId, queryModel.TeeBoxLocationId);
            model.PuttsGraphData = await PuttGraph(dbContext, userId, queryModel.CourseId, queryModel.SeasonId, queryModel.TeeBoxLocationId);
            model.FairwaysGraphData = await FairwaysHitGraph(dbContext, userId, queryModel.CourseId, queryModel.SeasonId, queryModel.TeeBoxLocationId);
            model.GIRGraphData = await GIRGraph(dbContext, userId, queryModel.CourseId, queryModel.SeasonId, queryModel.TeeBoxLocationId);

            return model;
        }

        private static async Task<IEnumerable<RecentCourseStatsRounds>> RecentCourseStatRounds(ToracGolfContext dbContext, int userId, int courseId, int? seasonId, int? teeBoxLocationId)
        {
            var baseQuery = BuildBaseCourseStatsQuery(dbContext, userId, courseId, seasonId, teeBoxLocationId).Take(10);

            return await baseQuery.Select(x => new RecentCourseStatsRounds
            {
                RoundId = x.RoundId,
                Score = x.Score,
                RoundDate = x.RoundDate,
                TeeBoxLocation = x.CourseTeeLocation.Description
            }).OrderByDescending(x => x.RoundDate).ToArrayAsync();
        }

        #region Graphs

        private static IQueryable<Round> BuildBaseCourseStatsQuery(ToracGolfContext dbContext, int userId, int courseId, int? seasonId, int? teeBoxLocationId)
        {
            var baseQuery = dbContext.Rounds.AsNoTracking().Where(x => x.UserId == userId && x.CourseId == courseId);

            if (seasonId.HasValue)
            {
                baseQuery = baseQuery.Where(x => x.SeasonId == seasonId);
            }

            if (teeBoxLocationId.HasValue)
            {
                baseQuery = baseQuery.Where(x => x.CourseTeeLocationId == teeBoxLocationId);
            }

            return baseQuery.OrderBy(x => x.RoundDate).Take(20);
        }

        private static async Task<IEnumerable<DashboardHandicapScoreSplitDisplay>> ScoreGraph(ToracGolfContext dbContext, int userId, int courseId, int? seasonId, int? teeBoxLocationId)
        {
            //just grab the last year so we don't grab globs of data
            return await BuildBaseCourseStatsQuery(dbContext, userId, courseId, seasonId, teeBoxLocationId)
                .Select(x => new DashboardHandicapScoreSplitDisplay
                {
                    Month = x.RoundDate.Month,
                    Day = x.RoundDate.Day,
                    Year = x.RoundDate.Year,
                    Score = x.Score,
                    Handicap = dbContext.Handicap.FirstOrDefault(y => y.RoundId == x.RoundId).HandicapAfterRound
                }).ToArrayAsync();
        }

        private static async Task<IEnumerable<PuttsCourseStatsGraph>> PuttGraph(ToracGolfContext dbContext, int userId, int courseId, int? seasonId, int? teeBoxLocationId)
        {
            //just grab the last year so we don't grab globs of data
            return await BuildBaseCourseStatsQuery(dbContext, userId, courseId, seasonId, teeBoxLocationId).Where(x => x.Putts.HasValue)
                .Select(x => new PuttsCourseStatsGraph
                {
                    Month = x.RoundDate.Month,
                    Day = x.RoundDate.Day,
                    Year = x.RoundDate.Year,
                    Putts = x.Putts.Value
                }).ToArrayAsync();
        }

        private static async Task<IEnumerable<GreensInRegulationCourseStatsGraph>> GIRGraph(ToracGolfContext dbContext, int userId, int courseId, int? seasonId, int? teeBoxLocationId)
        {
            //just grab the last year so we don't grab globs of data
            return await BuildBaseCourseStatsQuery(dbContext, userId, courseId, seasonId, teeBoxLocationId).Where(x => x.GreensInRegulation.HasValue)
                .Select(x => new GreensInRegulationCourseStatsGraph
                {
                    Month = x.RoundDate.Month,
                    Day = x.RoundDate.Day,
                    Year = x.RoundDate.Year,
                    GreensHit = x.GreensInRegulation.Value
                }).ToArrayAsync();
        }

        private static async Task<IEnumerable<FairwaysInRegulationCourseStatsGraph>> FairwaysHitGraph(ToracGolfContext dbContext, int userId, int courseId, int? seasonId, int? teeBoxLocationId)
        {
            //just grab the last year so we don't grab globs of data
            return await BuildBaseCourseStatsQuery(dbContext, userId, courseId, seasonId, teeBoxLocationId).Where(x => x.FairwaysHit.HasValue)
                .Select(x => new FairwaysInRegulationCourseStatsGraph
                {
                    Month = x.RoundDate.Month,
                    Day = x.RoundDate.Day,
                    Year = x.RoundDate.Year,
                    FairwaysHit = x.FairwaysHit.Value
                }).ToArrayAsync();
        }

        #endregion

        #endregion

    }
}
