using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            var query = dbContext.Rounds.AsNoTracking().Where(x => x.UserId == userId);

            //do we need to add the season?
            if (seasonId.HasValue)
            {
                //tack on the season
                query = query.Where(x => x.SeasonId == seasonId.Value);
            }

            //now let's grab the last 20 rounds
            query = query.OrderByDescending(x => x.RoundDate);

            //let's build a temp query so we can grab the scope and rating in 1 query
            return query.Select(x => new
            {
                x.RoundId,
                x.Score,
                TeeLocation = dbContext.CourseTeeLocations.FirstOrDefault(y => y.CourseId == x.CourseId && y.CourseTeeLocationId == x.CourseTeeLocationId)
            }).Select(x => new Last20Rounds
            {
                RoundId = x.RoundId,
                RoundScore = x.Score,
                Rating = x.TeeLocation.Rating,
                Slope = x.TeeLocation.Slope
            }).Take(20).ToArrayAsync();
        }

        #endregion

    }

}
