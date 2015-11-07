using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.Dashboard.Models;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.EFModel.Tables;

namespace ToracGolf.MiddleLayer.Dashboard
{
    public static class DashboardDataProvider
    {

        #region Dashboard

        #region Top 5 Queries

        public static async Task<IEnumerable<DashboardRoundDisplay>> Last5RoundsSelect(ToracGolfContext dbContext, int userId, int? seasonId)
        {
            return await BuildSelectStatement(dbContext, BuildBaseQuery(dbContext, userId, seasonId).OrderByDescending(x => x.RoundDate)).ToArrayAsync();
        }

        public static async Task<IEnumerable<DashboardRoundDisplay>> Top5RoundsSelect(ToracGolfContext dbContext, int userId, int? seasonId)
        {
            return await BuildSelectStatement(dbContext, BuildBaseQuery(dbContext, userId, seasonId).OrderBy(x => x.Score)).ToArrayAsync();
        }

        private static IQueryable<Round> BuildBaseQuery(ToracGolfContext dbContext, int userId, int? seasonId)
        {
            var baseQuery = dbContext.Rounds.AsNoTracking().Where(x => x.UserId == userId);

            if (seasonId.HasValue)
            {
                baseQuery = baseQuery.Where(x => x.SeasonId == seasonId);
            }

            return baseQuery;
        }

        private static IQueryable<DashboardRoundDisplay> BuildSelectStatement(ToracGolfContext dbContext, IOrderedQueryable<Round> sortedQuery)
        {
            return sortedQuery.Take(5).Select(x => new DashboardRoundDisplay
            {
                RoundDate = x.RoundDate,
                Score = x.Score,
                CourseName = x.Course.Name,
                TeeBoxLocation = dbContext.CourseTeeLocations.FirstOrDefault(y => y.CourseTeeLocationId == x.CourseTeeLocationId).Description
            });
        }

        #endregion

        #region Top Main Grid

        public static async Task<IEnumerable<DashboardHandicapScoreSplitDisplay>> ScoreHandicapGraph(ToracGolfContext dbContext, int userId, int? seasonId)
        {
            //let's build our months that we care about. we will go 1 year back
            var yearBackData = new List<DashboardHandicapScoreSplitDisplay>();

            //just grab the last 50 to limit the amount of data
            return await BuildBaseQuery(dbContext, userId, seasonId).OrderBy(x => x.RoundDate)
                .Select(x => new DashboardHandicapScoreSplitDisplay
                {
                    Month = x.RoundDate.Month,
                    Day = x.RoundDate.Day,
                    Year = x.RoundDate.Year,
                    Score = x.Score,
                    Handicap = dbContext.RoundHandicap.FirstOrDefault(y => y.RoundId == x.RoundId).HandicapBeforeRound
                }).ToArrayAsync();
        }

        #endregion

        #endregion

    }
}
