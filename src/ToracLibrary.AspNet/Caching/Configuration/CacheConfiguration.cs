using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracLibrary.AspNet.Caching.Configuration
{

    /// <summary>
    /// Holds the configuration for a registered cache item
    /// </summary>
    /// <typeparam name="T">Type of the cache item</typeparam>
    internal class CacheConfiguration<T> : CacheConfigurationUntyped

    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// <param name="CacheDataSourceToSet">Data source for populating the cache</param>
        public CacheConfiguration(Func<T> CacheDataSourceToSet) : base(() => CacheDataSourceToSet)
        {
        }

        #endregion    

    }

}