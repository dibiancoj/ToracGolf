using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.HandicapCalculator.Models;

namespace ToracGolf.MiddleLayer.HandicapCalculator
{

    public static class HandicapDataProvider
    {

        #region Public Methods

        public static Task<Last20Rounds[]> HandicapCalculatorRoundSelectorSeason(ToracGolfContext dbContext, int userId, int seasonId)
        {
            return HandicapCalculatorRoundSelectorHelper(dbContext, userId, seasonId);
        }

        public static Task<Last20Rounds[]> HandicapCalculatorRoundSelectorCareer(ToracGolfContext dbContext, int userId)
        {
            return HandicapCalculatorRoundSelectorHelper(dbContext, userId, null);
        }

        #endregion

        #region Helper Methods

        private static Task<Last20Rounds[]> HandicapCalculatorRoundSelectorHelper(ToracGolfContext dbContext, int userId, int? seasonId)
        {
            //go build the base query
            var query = (from myRounds in dbContext.Rounds.AsNoTracking()
                         join myTeeLocations in dbContext.CourseTeeLocations.AsNoTracking()
                         on new { myRounds.CourseId, myRounds.CourseTeeLocationId } equals new { myTeeLocations.CourseId, myTeeLocations.CourseTeeLocationId }
                         where myRounds.UserId == userId
                         orderby myRounds.RoundDate descending, myRounds.RoundId descending
                         select new
                         {
                             myRounds.RoundId,
                             myRounds.Score,
                             myRounds.SeasonId,
                             myTeeLocations.Rating,
                             myTeeLocations.Slope
                         }).Take(20).AsQueryable();

            //do we need to add the season?
            if (seasonId.HasValue)
            {
                //tack on the season
                query = query.Where(x => x.SeasonId == seasonId.Value);
            }

            //go grab the last 20 rounds class and return the task
            return query.Select(x => new Last20Rounds
            {
                RoundId = x.RoundId,
                RoundScore = x.Score,
                Rating = x.Rating,
                Slope = x.Slope
            }).ToArrayAsync();
        }

        #endregion

    }

}
