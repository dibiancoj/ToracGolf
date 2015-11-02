using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.Dashboard.Models;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracLibrary.AspNet.Paging;

namespace ToracGolf.MiddleLayer.Dashboard
{
    public static class DashboardDataProvider
    {

        #region Dashboard

        public static async Task<IEnumerable<DashboardRoundDisplay>> Last5RoundsSelect(ToracGolfContext dbContext, int userId, int? seasonId)
        {
            var baseQuery = dbContext.Rounds.AsNoTracking().Where(x => x.UserId == userId);

            if (seasonId.HasValue)
            {
                baseQuery = baseQuery.Where(x => x.SeasonId == seasonId);
            }

            var tempData = await baseQuery.OrderByDescending(x => x.RoundDate)
                            .Take(5)
                            .Select(x => new
                            {
                                RoundDate = x.RoundDate,
                                Score = x.Score,
                                CourseName = x.Course.Name,
                                TeeLocation = dbContext.CourseTeeLocations.FirstOrDefault(y => y.CourseTeeLocationId == x.CourseTeeLocationId).Description
                                }).ToArrayAsync();

            return tempData.Select(x => new DashboardRoundDisplay(x.CourseName, x.RoundDate, x.TeeLocation, x.Score)).ToArray();
        }

        #endregion

    }
}
