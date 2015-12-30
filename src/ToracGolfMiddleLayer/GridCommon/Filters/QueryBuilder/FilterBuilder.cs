using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.ListingFactories;

namespace ToracGolf.MiddleLayer.GridCommon.Filters.QueryBuilder
{
    public static class FilterBuilder
    {

        public static IQueryable<TFrom> BuildQueryFilter<TFrom, TTo>(ToracGolfContext dbContext, IQueryable<TFrom> query, IListingFactory<TFrom, TTo> factory, params KeyValuePair<string, object>[] filters)
            where TFrom : class
            where TTo : class
        {
            foreach (var filter in filters.Where(x => x.Value != null))
            {
                //grab the filter config
                var filterQueryBuilder = factory.FilterConfiguration[filter.Key];

                //build the expression and tack it on to the query
                query = filterQueryBuilder.BuildAFilterQuery<TFrom, TTo>(dbContext, query, filter);
            }

            //return the query
            return query;
        }

    }
}
