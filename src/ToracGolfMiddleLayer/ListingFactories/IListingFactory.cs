using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;

namespace ToracGolf.MiddleLayer.ListingFactories
{

    public interface IListingFactory<T>
        where T : class
    {

        #region Properties

        /// <summary>
        /// int is the user id
        /// </summary>
        IDictionary<string, Func<IQueryable<T>, ListingFactoryParameters, IOrderedQueryable<T>>> SortByConfiguration { get; }

        #endregion

    }

}
