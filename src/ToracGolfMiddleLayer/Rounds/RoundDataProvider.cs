using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracGolf.MiddleLayer.Rounds.Models;

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


        public static IQueryable<Course> RoundSelectQueryBuilder(ToracGolfContext dbContext, string courseNameFilter, int? StateFilter)
        {
            throw new NotImplementedException();
            ////build the queryable
            //var queryable = dbContext.Course.AsNoTracking().AsQueryable();

            ////if we have a course name, add it as a filter
            //if (!string.IsNullOrEmpty(courseNameFilter))
            //{
            //    queryable = queryable.Where(x => x.Name.Contains(courseNameFilter));
            //}

            ////do we have a state filter?
            //if (StateFilter.HasValue)
            //{
            //    queryable = queryable.Where(x => x.StateId == StateFilter.Value);
            //}

            ////return the queryable
            //return queryable;
        }

        /// <param name="pageId">0 base index that holds what page we are on</param>
        public static async Task<IEnumerable<RoundListingData>> RoundSelect(ToracGolfContext dbContext, int pageId, RoundListingSortOrder SortBy, string roundNameFilter, int? StateFilter, int RecordsPerPage)
        {
            throw new NotImplementedException();
            ////how many items to skip
            //int skipAmount = pageId * RecordsPerPage;

            ////go grab the query
            //var queryable = CourseSelectQueryBuilder(dbContext, courseNameFilter, StateFilter);

            ////figure out what you want to order by
            //if (SortBy == CourseListingSortOrder.CourseListingSortEnum.CourseNameAscending)
            //{
            //    queryable = queryable.OrderBy(x => x.Name);
            //}
            //else if (SortBy == CourseListingSortOrder.CourseListingSortEnum.CourseNameDescending)
            //{
            //    queryable = queryable.OrderByDescending(x => x.Name);
            //}
            //else if (SortBy == CourseListingSortOrder.CourseListingSortEnum.EasiestCourses)
            //{
            //    queryable = queryable.OrderBy(x => x.CourseTeeLocations.Min(y => y.Slope));
            //}
            //else if (SortBy == CourseListingSortOrder.CourseListingSortEnum.HardestCourses)
            //{
            //    queryable = queryable.OrderByDescending(x => x.CourseTeeLocations.Max(y => y.Slope));
            //}
            //else if (SortBy == CourseListingSortOrder.CourseListingSortEnum.MostTimesPlayed)
            //{
            //    //todo: need to fix when we get the rounds table going
            //    queryable = queryable.OrderBy(x => x.Name);
            //}

            ////go run the query now
            //return await queryable.Select(x => new CourseListingData
            //{
            //    CourseData = x,
            //    StateDescription = dbContext.Ref_State.FirstOrDefault(y => y.StateId == x.StateId).Description,
            //    TeeLocationCount = dbContext.CourseTeeLocations.Count(y => y.CourseId == x.CourseId),
            //    CourseImage = dbContext.CourseImages.FirstOrDefault(y => y.CourseId == x.CourseId)
            //}).Skip(skipAmount).Take(RecordsPerPage).ToArrayAsync();
        }

        public static async Task<int> TotalNumberOfRounds(ToracGolfContext dbContext, string courseNameFilter, int? StateFilter, int RecordsPerPage)
        {
            throw new NotImplementedException();
            //return DataSetPaging.CalculateTotalPages(await CourseSelectQueryBuilder(dbContext, courseNameFilter, StateFilter).CountAsync(), RecordsPerPage);
        }

        #endregion

    }
}
