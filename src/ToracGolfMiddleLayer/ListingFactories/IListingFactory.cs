using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.GridCommon.Filters.QueryBuilder;

namespace ToracGolf.MiddleLayer.ListingFactories
{

    public interface IListingFactory<TSortByEnum, TFromTable, TToTable>
        where TSortByEnum : struct, IConvertible
        where TFromTable : class
        where TToTable : class
    {

        #region Properties

        /// <summary>
        /// int is the user id
        /// </summary>
        IDictionary<TSortByEnum, Func<IQueryable<TFromTable>, ListingFactoryParameters, IOrderedQueryable<TFromTable>>> SortByConfiguration { get; }

        /// <summary>
        /// Key is the filter name. Value is class
        /// </summary>
        IDictionary<string, IQueryBuilder<TFromTable>> FilterConfiguration { get; }

        #endregion

    }

}
