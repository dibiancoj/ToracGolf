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

        public static async Task<Ref_Season> RefSeasonAddOrGet(ToracGolfContext dbContext, string seasonText)
        {
            //let's first try to find the season with the same name
            var seasonToAdd = await dbContext.Ref_Season.AsNoTracking().FirstOrDefaultAsync(x => x.SeasonText == seasonText);

            //didn't find a season with this name
            if (seasonToAdd == null)
            {
                //create the new season
                seasonToAdd = new Ref_Season { SeasonText = seasonText, CreatedDate = DateTime.Now };

                //add the record
                dbContext.Ref_Season.Add(seasonToAdd);

                //we need to save it for foreign key 
                await dbContext.SaveChangesAsync();
            }

            //return the record
            return seasonToAdd;
        }

        public static async Task<UserSeason> UserSeasonAdd(ToracGolfContext dbContext, int userId, Ref_Season refSeasonRecord)
        {
            //record to add
            var recordToAdd = new UserSeason
            {
                UserId = userId,
                SeasonId = refSeasonRecord.SeasonId,
                CreatedDate = DateTime.Now
            };

            //add the record
            dbContext.UserSeason.Add(recordToAdd);

            //go save the changes
            await dbContext.SaveChangesAsync();

            //return the record now
            return recordToAdd;
        }

        public static async Task<bool> MakeSeasonAsCurrent(ToracGolfContext dbContext, int userId, int currentSeasonId)
        {
            //grab the user record
            var user = await dbContext.Users.FirstAsync(x => x.UserId == userId);

            //set the current season now
            user.CurrentSeasonId = currentSeasonId;

            //save the changes
            await dbContext.SaveChangesAsync();

            //return a postive result
            return true;
        }

    }

}
