using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracGolf.MiddleLayer.HandicapCalculator;
using ToracGolf.MiddleLayer.Rounds.Models;
using ToracLibrary.AspNet.Paging;

namespace ToracGolf.MiddleLayer.Rounds
{
    public static class RoundDataProvider
    {

        #region General Lookups

        public static async Task<IEnumerable<CourseForRoundAddScreen>> CoursesSelectForState(ToracGolfContext dbContext, int stateId)
        {
            return await dbContext.Course.AsNoTracking()
                         .Where(x => x.StateId == stateId)
                       .Select(x => new CourseForRoundAddScreen
                       {
                           CourseId = x.CourseId,
                           Name = x.Name,
                       }).ToArrayAsync();
        }

        public static async Task<IEnumerable<TeeBoxSelectForCourseSelect>> TeeBoxSelectForCourse(ToracGolfContext dbContext, int courseId, double? currentHandicap)
        {
            var dbResults = await dbContext.CourseTeeLocations.AsNoTracking()
                                   .Where(x => x.CourseId == courseId).ToArrayAsync();


            var lst = new List<TeeBoxSelectForCourseSelect>();

            foreach (var teeBox in dbResults)
            {
                var teeBoxModel = new TeeBoxSelectForCourseSelect
                {
                    Back9Par = teeBox.Back9Par,
                    Front9Par = teeBox.Front9Par,
                    CourseId = teeBox.CourseId,
                    CourseTeeLocationId = teeBox.CourseTeeLocationId,
                    Description = teeBox.Description,
                    Rating = teeBox.Rating,
                    Slope = teeBox.Slope,
                    Yardage = teeBox.Yardage,
                    TeeLocationSortOrderId = teeBox.TeeLocationSortOrderId,
                };

                //now add the course handicap and the max score
                teeBoxModel.CourseTeeBoxHandicap = HandicapCalculator.Handicapper.CalculateCourseHandicap(currentHandicap, teeBoxModel.Slope);

                //now calculate the score
                teeBoxModel.MaxScorePerHole = HandicapCalculator.Handicapper.MaxScorePerHole(teeBoxModel.CourseTeeBoxHandicap);

                //add the tee box score
                lst.Add(teeBoxModel);
            }

            //return the list
            return lst;
        }

        #endregion

        #region Round Add

        public static async Task<int> SaveRound(ToracGolfContext dbContext, int userId, int seasonId, RoundAddEnteredData roundData)
        {
            //build the round record
            var round = new Round
            {
                CourseId = roundData.CourseId,
                CourseTeeLocationId = roundData.TeeLocationId,
                RoundDate = roundData.RoundDate,
                Is9HoleScore = roundData.NineHoleScore,
                Score = roundData.Score.Value,
                UserId = userId,
                SeasonId = seasonId
            };

            //add the round
            dbContext.Rounds.Add(round);

            //save the round
            await dbContext.SaveChangesAsync();

            //return the round id
            return round.RoundId;
        }

        #endregion

        #region Round Listing

        public static IQueryable<RoundListingData> RoundSelectQueryBuilder(ToracGolfContext dbContext, int userId, string courseNameFilter, int? seasonFilter)
        {
            //build the queryable
            var queryable = dbContext.Rounds.AsNoTracking().Where(x => x.UserId == userId).AsQueryable();

            //if we have a course name, add it as a filter
            if (!string.IsNullOrEmpty(courseNameFilter))
            {
                queryable = queryable.Where(x => x.Course.Name.Contains(courseNameFilter));
            }

            //do we have a state filter?
            if (seasonFilter.HasValue)
            {
                queryable = queryable.Where(x => x.SeasonId == seasonFilter.Value);
            }

            //go return the queryable
            return queryable.Select(x => new RoundListingData
            {
                RoundId = x.RoundId,
                CourseId = x.CourseId,
                CourseName = x.Course.Name,
                RoundDate = x.RoundDate,
                Score = x.Score,
                SeasonId = x.SeasonId,
                TeeBoxLocation = dbContext.CourseTeeLocations.FirstOrDefault(y => y.CourseId == x.CourseId && y.CourseTeeLocationId == x.CourseTeeLocationId)
            });
        }

        /// <param name="pageId">0 base index that holds what page we are on</param>
        public static async Task<RoundSelectModel> RoundSelect(ToracGolfContext dbContext, int userId, int pageId, RoundListingSortOrder.RoundListingSortEnum sortBy, string courseNameFilter, int? seasonFilter, int recordsPerPage, double? currentHandicap)
        {
            //how many items to skip
            int skipAmount = pageId * recordsPerPage;

            //go grab the query
            var queryable = RoundSelectQueryBuilder(dbContext, userId, courseNameFilter, seasonFilter);

            //figure out what you want to order by
            if (sortBy == RoundListingSortOrder.RoundListingSortEnum.CourseNameAscending)
            {
                queryable = queryable.OrderBy(x => x.CourseName);
            }
            else if (sortBy == RoundListingSortOrder.RoundListingSortEnum.CourseNameDescending)
            {
                queryable = queryable.OrderByDescending(x => x.CourseName);
            }
            else if (sortBy == RoundListingSortOrder.RoundListingSortEnum.RoundDateAscending)
            {
                queryable = queryable.OrderBy(x => x.RoundDate).ThenBy(x => x.RoundId);
            }
            else if (sortBy == RoundListingSortOrder.RoundListingSortEnum.RoundDateDescending)
            {
                queryable = queryable.OrderByDescending(x => x.RoundDate).ThenByDescending(x => x.RoundId);
            }
            else if (sortBy == RoundListingSortOrder.RoundListingSortEnum.BestScores)
            {
                queryable = queryable.OrderBy(x => x.Score).ThenBy(x => x.RoundId);
            }
            else if (sortBy == RoundListingSortOrder.RoundListingSortEnum.WorseScores)
            {
                queryable = queryable.OrderByDescending(x => x.Score).ThenByDescending(x => x.RoundId);
            }

            //go run the query now
            var dataSet = await queryable.Skip(skipAmount).Take(recordsPerPage).ToArrayAsync();

            //now grab the distinct course id's so we can get the images
            var distinctCourseIds = dataSet.Select(x => x.CourseId).Distinct();

            //now grab all the course images
            var courseImages = await dbContext.CourseImages.Where(x => distinctCourseIds.Contains(x.CourseId)).ToDictionaryAsync(x => x.CourseId, y => y.CourseImage);

            //make sure we have a handicap first
            if (currentHandicap.HasValue)
            {
                //let's loop through the rounds and display the stsarts
                foreach (var round in dataSet)
                {
                    //calculate the round handicap
                    round.RoundHandicap = Handicapper.RoundHandicap(round.Score, round.TeeBoxLocation.Rating, round.TeeBoxLocation.Slope);

                    //go calculate the round performance
                    current handicap needs to be a static handicap that is the handicap at the time of the round
                    round.RoundPerformance = (int)RoundPerformance.CalculateRoundPerformance(currentHandicap, round.RoundHandicap);

                    //calculate the adjusted score
                    current handicap needs to be a static handicap that is the handicap at the time of the round
                    round.AdjustedScore = Convert.ToInt32(Math.Round(round.Score - currentHandicap.Value, 0));
                }
            }

            //go return the lookup now
            return new RoundSelectModel(courseImages, dataSet);
        }

        public static async Task<int> TotalNumberOfRounds(ToracGolfContext dbContext, int userId, string roundNameFilter, int? StateFilter, int RecordsPerPage)
        {
            return DataSetPaging.CalculateTotalPages(await RoundSelectQueryBuilder(dbContext, userId, roundNameFilter, StateFilter).CountAsync(), RecordsPerPage);
        }

        #endregion

    }
}
