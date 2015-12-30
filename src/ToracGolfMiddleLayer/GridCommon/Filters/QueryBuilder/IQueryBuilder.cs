using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;

namespace ToracGolf.MiddleLayer.GridCommon.Filters.QueryBuilder
{
    public interface IQueryBuilder<TQueryType>
         where TQueryType : class
    {

        IQueryable<TQueryType> BuildAFilterQuery(ToracGolfContext dbContext, IQueryable<TQueryType> query, KeyValuePair<string, object> filter);

    }
}

