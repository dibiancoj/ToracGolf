﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracGolf.MiddleLayer.Season.Models;

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
                          orderby mySeasons.Rounds.Select(x => x.RoundDate).DefaultIfEmpty(DateTime.MaxValue).Min() descending
                          select refSeasons).ToDictionaryAsync(x => x.SeasonId, x => x.SeasonText).ConfigureAwait(false);
        }

        public static async Task<IEnumerable<SeasonListingData>> SeasonListingForGrid(ToracGolfContext dbContext, int userId)
        {
            return await (from mySeasons in dbContext.UserSeason.AsNoTracking()
                          join refSeasons in dbContext.Ref_Season.AsNoTracking()
                          on mySeasons.SeasonId equals refSeasons.SeasonId
                          where mySeasons.UserId == userId
                          orderby mySeasons.Rounds.Select(x => x.RoundDate).DefaultIfEmpty(DateTime.MaxValue).Min() descending
                          select new SeasonListingData
                          {
                              Description = refSeasons.SeasonText,
                              SeasonId = refSeasons.SeasonId,
                              NumberOfRounds = mySeasons.Rounds.Count(),
                              TopScore = mySeasons.Rounds.Min(x => x.Score),
                              WorseScore = mySeasons.Rounds.Max(x => x.Score),
                              AverageScore = mySeasons.Rounds.Average(x => x.Score),
                          }).ToArrayAsync().ConfigureAwait(false);
        }

        public static async Task<Ref_Season> RefSeasonAddOrGet(ToracGolfContext dbContext, string seasonText)
        {
            //let's first try to find the season with the same name
            var seasonToAdd = await dbContext.Ref_Season.AsNoTracking().FirstOrDefaultAsync(x => x.SeasonText == seasonText).ConfigureAwait(false);

            //didn't find a season with this name
            if (seasonToAdd == null)
            {
                //create the new season
                seasonToAdd = new Ref_Season { SeasonText = seasonText, CreatedDate = DateTime.Now };

                //add the record
                dbContext.Ref_Season.Add(seasonToAdd);

                //we need to save it for foreign key 
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
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
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            //return the record now
            return recordToAdd;
        }

        public static async Task<bool> MakeSeasonAsCurrent(ToracGolfContext dbContext, int userId, int currentSeasonId)
        {
            //grab the user record
            var user = await dbContext.Users.FirstAsync(x => x.UserId == userId).ConfigureAwait(false);

            //set the current season now
            user.CurrentSeasonId = currentSeasonId;

            //save the changes
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            //return a postive result
            return true;
        }

        public static async Task<bool> DeleteSeason(ToracGolfContext dbContext, int userId, int seasonIdToDelete)
        {
            //go grab the season to delete
            var seasonToDelete = await dbContext.UserSeason.FirstAsync(x => x.UserId == userId && x.SeasonId == seasonIdToDelete).ConfigureAwait(false);

            //remove it from the context
            dbContext.UserSeason.Remove(seasonToDelete);

            //save it now
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            //return a postive result
            return true;
        }

    }

}
