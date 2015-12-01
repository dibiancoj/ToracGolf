﻿using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracLibrary.AspNet.Paging;
using ToracGolf.MiddleLayer.Courses.Models;
using ToracGolf.MiddleLayer.Common;
using ToracGolf.MiddleLayer.Courses.Models.CourseStats;

namespace ToracGolf.MiddleLayer.Courses
{
    public static class CourseDataProvider
    {

        #region Course Add

        public static async Task<int> CourseAdd(ToracGolfContext dbContext, int userId, CourseAddEnteredData CourseData)
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
                OnlyAllow18Holes = CourseData.OnlyAllow18Holes
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
                byte[] fileToSave = ToracLibrary.AspNet.Graphics.GraphicsUtilities.ImageFromJsonBase64String(CourseData.CourseImage).FileBytes;

                //let's go save the image
                dbContext.CourseImages.Add(new CourseImages { CourseId = courseToAdd.CourseId, CourseImage = fileToSave });

                //save it now
                await dbContext.SaveChangesAsync();
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
        public static async Task<IEnumerable<CourseListingData>> CourseSelect(ToracGolfContext dbContext,
                                                                              int pageId,
                                                                              CourseListingSortOrder.CourseListingSortEnum sortBy,
                                                                              string courseNameFilter,
                                                                              int? stateFilter,
                                                                              int recordsPerPage,
                                                                              int userId)
        {
            //how many items to skip
            int skipAmount = pageId * recordsPerPage;

            //go grab the query
            var queryable = CourseSelectQueryBuilder(dbContext, courseNameFilter, stateFilter);

            //figure out what you want to order by
            if (sortBy == CourseListingSortOrder.CourseListingSortEnum.CourseNameAscending)
            {
                queryable = queryable.OrderBy(x => x.Name);
            }
            else if (sortBy == CourseListingSortOrder.CourseListingSortEnum.CourseNameDescending)
            {
                queryable = queryable.OrderByDescending(x => x.Name);
            }
            else if (sortBy == CourseListingSortOrder.CourseListingSortEnum.EasiestCourses)
            {
                queryable = queryable.OrderBy(x => x.CourseTeeLocations.Min(y => y.Slope));
            }
            else if (sortBy == CourseListingSortOrder.CourseListingSortEnum.HardestCourses)
            {
                queryable = queryable.OrderByDescending(x => x.CourseTeeLocations.Max(y => y.Slope));
            }

            //go run the query now
            var query = queryable.Select(x => new CourseListingData
            {
                CourseData = x,
                StateDescription = dbContext.Ref_State.FirstOrDefault(y => y.StateId == x.StateId).Description,
                TeeLocationCount = x.CourseTeeLocations.Count,

                CourseImage = x.CourseImage.CourseImage,

                NumberOfRounds = dbContext.Rounds.Count(y => y.CourseId == x.CourseId && y.UserId == userId),
                TopScore = dbContext.Rounds.Where(y => y.CourseId == x.CourseId && y.UserId == userId).Select(y => y.Score).Min(),
                WorseScore = dbContext.Rounds.Where(y => y.CourseId == x.CourseId && y.UserId == userId).Select(y => y.Score).Max(),
                AverageScore = dbContext.Rounds.Where(y => y.CourseId == x.CourseId && y.UserId == userId).Select(y => y.Score).Average(),

                FairwaysHit = dbContext.Rounds.Where(y => y.CourseId == x.CourseId && y.UserId == userId).Select(y => y.FairwaysHit).Sum(),
                FairwaysHitAttempted = dbContext.Rounds.Where(y => y.CourseId == x.CourseId && y.UserId == userId).Select(y => y.CourseTeeLocation.FairwaysOnCourse).Sum(),
                GreensInRegulation = dbContext.Rounds.Where(y => y.CourseId == x.CourseId && y.UserId == userId).Select(y => y.GreensInRegulation).Average(),
                NumberOfPutts = dbContext.Rounds.Where(y => y.CourseId == x.CourseId && y.UserId == userId).Select(y => y.Putts).Average()
            });

            //if we need to sort by most rounds played, then do it now
            if (sortBy == CourseListingSortOrder.CourseListingSortEnum.MostTimesPlayed)
            {
                //todo: need to fix when we get the rounds table going
                query = query.OrderByDescending(x => x.NumberOfRounds).ThenByDescending(x => x.CourseData.CourseId);
            }

            //go execute it and return it
            return await query.Skip(skipAmount).Take(recordsPerPage).ToArrayAsync();
        }

        public static async Task<int> TotalNumberOfCourses(ToracGolfContext dbContext, string courseNameFilter, int? StateFilter, int RecordsPerPage)
        {
            return DataSetPaging.CalculateTotalPages(await CourseSelectQueryBuilder(dbContext, courseNameFilter, StateFilter).CountAsync(), RecordsPerPage);
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
            return (await dbContext.CourseTeeLocations.FirstAsync(x => x.CourseTeeLocationId == courseTeeLocationId)).CourseTeeLocationId;
        }

        #endregion

        #region Course Select

        public static async Task<CourseStatsModel> CourseStatsSelect(ToracGolfContext dbContext, int courseId, int userId)
        {
            return await dbContext.Course.AsNoTracking().Select(x => new CourseStatsModel
            {
                CourseId = x.CourseId,
                CourseCity = x.State.Description,
                CourseDescription = x.Description,
                CourseName = x.Name,
                CourseState = x.State.Description,
                CourseImage = x.CourseImage.CourseImage,
                TeeBoxLocations = x.CourseTeeLocations.Select(y => new TeeBoxData
                {
                    TeeLocationId = y.CourseTeeLocationId,
                    Name = y.Description,
                    Yardage = y.Yardage,
                    Par = y.Front9Par + y.Back9Par,
                    Rating = y.Rating,
                    Slope = y.Slope
                })
            }).FirstAsync(x => x.CourseId == courseId);
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
                return new CourseStatsQueryResponse { QuickStats = new CondensedStats() };
            }

            //group by should chunk it up to 1 record
            return await query.GroupBy(x => x.UserId).Select(x => new CourseStatsQueryResponse
            {
                QuickStats = new CondensedStats
                {
                    RoundCount = x.Count(),
                    AverageScore = x.Average(y => y.Score),
                    BestScore = x.Min(y => y.Score),
                    TeeBoxCount = dbContext.CourseTeeLocations.Count(y => y.CourseId == queryModel.CourseId)
                }
            }).FirstAsync();
        }

        #endregion

    }
}
