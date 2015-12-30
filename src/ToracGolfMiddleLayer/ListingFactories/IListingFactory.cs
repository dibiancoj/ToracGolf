using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.GridCommon.Filters;
using ToracGolf.MiddleLayer.GridCommon.Filters.QueryBuilder;

namespace ToracGolf.MiddleLayer.ListingFactories
{

    public interface IListingFactory<TFromTable, TToTable>
        where TFromTable : class
        where TToTable : class
    {

        #region Properties

        /// <summary>
        /// int is the user id
        /// </summary>
        IDictionary<string, Func<IQueryable<TFromTable>, ListingFactoryParameters, IOrderedQueryable<TFromTable>>> SortByConfiguration { get; }

        /// <summary>
        /// Key is the filter name. Value is the field to query
        /// </summary>
        IDictionary<string, IQueryBuilder> FilterConfiguration { get; }

        #endregion

    }

}
