using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.ListingFactories;

namespace ToracGolf.MiddleLayer.GridCommon.Filters.QueryBuilder
{
    public interface IQueryBuilder
    {

        IQueryable<TFrom> BuildAFilterQuery<TFrom, TTo>(ToracGolfContext dbContext, IQueryable<TFrom> query, KeyValuePair<string, object> filter)
             where TFrom : class
             where TTo : class;

    }
}

