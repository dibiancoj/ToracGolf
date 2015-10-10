using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.EFModel.Tables;

namespace ToracGolf.MiddleLayer.Season
{

    public static class SeasonDataProvider
    {

        public static async Task<int> CurrentSeasonForUser(ToracGolfContext dbContext, int userId)
        {
            return (await dbContext.Users.AsNoTracking().FirstAsync(x => x.UserId == userId)).CurrentSeasonId;
        }

        public static async Task<IDictionary<int, string>> SeasonSelectForUser(ToracGolfContext dbContext, int userId)
        {
            return await (from mySeasons in dbContext.UserSeason.AsNoTracking()
                          join refSeasons in dbContext.Ref_Season.AsNoTracking()
                          on mySeasons.SeasonId equals refSeasons.SeasonId
                          where mySeasons.UserId == userId
                          orderby mySeasons.CreatedDate
                          select refSeasons).ToDictionaryAsync(x => x.SeasonId, x => x.SeasonText);

        }

    }

}
