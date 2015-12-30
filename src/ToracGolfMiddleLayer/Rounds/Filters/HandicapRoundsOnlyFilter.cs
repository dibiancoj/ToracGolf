using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracGolf.MiddleLayer.GridCommon.Filters.QueryBuilder;
using ToracGolf.MiddleLayer.HandicapCalculator;

namespace ToracGolf.MiddleLayer.Rounds.Filters
{

    public class HandicapRoundsOnlyFilter : IQueryBuilder
    {
        public IQueryable<TFrom> BuildAFilterQuery<TFrom, TTo>(ToracGolfContext dbContext, IQueryable<TFrom> query, KeyValuePair<string, object> filter)
            where TFrom : class
            where TTo : class
        {
            //if they pass in false, just leave the query alone
            if (!(bool)filter.Value)
            {
                //it's false, just return whatever query we have
                return query;
            }

            //case the query
            var castedQuery = (IQueryable<Round>)query;

            //how many rounds do we have?
            int roundsWeHaveInQuery = castedQuery.Count();

            //now check how many are in the calculation
            var howManyToCalculateWith = Handicapper.HowManyRoundsToUseInFormula(roundsWeHaveInQuery > 20 ? 20 : roundsWeHaveInQuery);

            //now just grab the last 20
            var last20RoundIds = castedQuery.OrderByDescending(x => x.RoundDate)
                                          .ThenByDescending(x => x.RoundId)
                                          .Take(20) //we only ever get the last 20...
                                          .OrderBy(x => x.RoundHandicap) //now grab the lowest rated rounds of how many we are going to calculate with
                                          .Take(howManyToCalculateWith) //then grab just those
                                          .Select(x => x.RoundId).ToArray();

            //add the round id's to the query
            return (IQueryable<TFrom>)castedQuery.Where(x => last20RoundIds.Contains(x.RoundId));
        }
    }

}
