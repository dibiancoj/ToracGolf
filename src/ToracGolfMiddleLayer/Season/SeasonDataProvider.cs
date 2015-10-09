using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;

namespace ToracGolf.MiddleLayer.Season
{

    public static class SeasonDataProvider
    {

        public static async Task<int> CurrentSeasonForUser(ToracGolfContext dbContext, int userId)
        {
            return (await dbContext.Users.AsNoTracking().FirstAsync(x => x.UserId == userId)).CurrentSeasonId;
        }

    }

}
