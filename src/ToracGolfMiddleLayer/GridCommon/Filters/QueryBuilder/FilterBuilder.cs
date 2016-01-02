using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.ListingFactories;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;

namespace ToracGolf.MiddleLayer.GridCommon.Filters.QueryBuilder
{
    public static class FilterBuilder
    {

        public static IQueryable<TFrom> BuildQueryFilter<TSortByEnum, TFrom, TTo>(ToracGolfContext dbContext, IQueryable<TFrom> query, IListingFactory<TSortByEnum, TFrom, TTo> factory, params KeyValuePair<string, object>[] filters)
            where TSortByEnum : struct, IConvertible
            where TFrom : class
            where TTo : class
        {
            foreach (var filter in filters)
            {
                //grab the filter config
                var filterQueryBuilder = factory.FilterConfiguration[filter.Key];

                //process this filter?
                //so either we have no rules, or 1 rule evalulated to true
                if (!filterQueryBuilder.ProcessFilterRules.AnyWithNullCheck() || filterQueryBuilder.ProcessFilterRules.AnyWithNullCheck(x => x.ProcessFilter(filter)))
                {
                    //build the expression and tack it on to the query
                    query = filterQueryBuilder.BuildAFilterQuery(dbContext, query, filter);
                }
            }

            //return the query
            return query;
        }

    }
}
