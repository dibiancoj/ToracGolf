using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.EFModel.Tables;
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

        public static async Task<IEnumerable<CourseTeeLocations>> TeeBoxSelectForCourse(ToracGolfContext dbContext, int courseId)
        {
            return await dbContext.CourseTeeLocations.AsNoTracking()
                         .Where(x => x.CourseId == courseId).ToArrayAsync();

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

        public static IQueryable<RoundListingData> RoundSelectQueryBuilder(ToracGolfContext dbContext, int userId, string roundNameFilter, int? StateFilter)
        {
            //build the queryable
            var queryable = dbContext.Rounds.AsNoTracking().Where(x => x.UserId == userId).Select(x => new RoundListingData
            {
                RoundId = x.RoundId,
                CourseId = x.CourseId,
                CourseName = dbContext.Course.FirstOrDefault(y => y.CourseId == x.CourseId).Name,
                RoundDate = x.RoundDate,
                Score = x.Score,
                TeeBoxLocation = dbContext.CourseTeeLocations.FirstOrDefault(y => y.CourseId == x.CourseId && y.CourseTeeLocationId == x.CourseTeeLocationId)
            });

            //if we have a course name, add it as a filter
            if (!string.IsNullOrEmpty(roundNameFilter))
            {
                // queryable = queryable.Where(x => x.Name.Contains(roundNameFilter));
            }

            //do we have a state filter?
            if (StateFilter.HasValue)
            {
                // queryable = queryable.Where(x => x.StateId == StateFilter.Value);
            }

            //return the queryable
            return queryable;
        }

        /// <param name="pageId">0 base index that holds what page we are on</param>
        public static async Task<RoundSelectModel> RoundSelect(ToracGolfContext dbContext, int userId, int pageId, RoundListingSortOrder.RoundListingSortEnum SortBy, string roundNameFilter, int? StateFilter, int RecordsPerPage)
        {
            //how many items to skip
            int skipAmount = pageId * RecordsPerPage;

            //go grab the query
            var queryable = RoundSelectQueryBuilder(dbContext, userId, roundNameFilter, StateFilter);

            //figure out what you want to order by
            if (SortBy == RoundListingSortOrder.RoundListingSortEnum.CourseNameAscending)
            {
                queryable = queryable.OrderBy(x => x.CourseName);
            }
            else if (SortBy == RoundListingSortOrder.RoundListingSortEnum.CourseNameDescending)
            {
                queryable = queryable.OrderByDescending(x => x.CourseName);
            }
            else if (SortBy == RoundListingSortOrder.RoundListingSortEnum.RoundDateAscending)
            {
                queryable = queryable.OrderBy(x => x.RoundDate).ThenBy(x => x.RoundId);
            }
            else if (SortBy == RoundListingSortOrder.RoundListingSortEnum.RoundDateDescending)
            {
                queryable = queryable.OrderByDescending(x => x.RoundDate).ThenByDescending(x => x.RoundId);
            }
            else if (SortBy == RoundListingSortOrder.RoundListingSortEnum.BestScores)
            {
                queryable = queryable.OrderBy(x => x.Score).ThenBy(x => x.RoundId);
            }
            else if (SortBy == RoundListingSortOrder.RoundListingSortEnum.WorseScores)
            {
                queryable = queryable.OrderByDescending(x => x.Score).ThenByDescending(x => x.RoundId);
            }

            //go run the query now
            var dataSet = await queryable.Skip(skipAmount).Take(RecordsPerPage).ToArrayAsync();

            //now grab the distinct course id's so we can get the images
            var distinctCourseIds = dataSet.Select(x => x.CourseId).Distinct();

            //now grab all the course images
            var courseImages = await dbContext.CourseImages.Where(x => distinctCourseIds.Contains(x.CourseId)).ToDictionaryAsync(x => x.CourseId, y => y.CourseImage);

            //let's loop through the rounds and display the stsarts
            foreach (var round in dataSet)
            {
                //performance to set
                RoundPerformance roundPerformance;

                if (round.Score >= 110)
                {
                    roundPerformance = RoundPerformance.Awful;
                }
                else if (round.Score >= 100)
                {
                    roundPerformance = RoundPerformance.Bad;
                }
                else if (round.Score >= 97)
                {
                    roundPerformance = RoundPerformance.BadAverage;
                }
                else if (round.Score >= 93 && round.Score <= 97)
                {
                    roundPerformance = RoundPerformance.Average;
                }
                else if (round.Score >= 87 && round.Score <= 93)
                {
                    roundPerformance = RoundPerformance.AboveAverage;
                }
                else
                {
                    roundPerformance = RoundPerformance.AboveAverage;
                }

                round.RoundPerformance = (int)roundPerformance;
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
