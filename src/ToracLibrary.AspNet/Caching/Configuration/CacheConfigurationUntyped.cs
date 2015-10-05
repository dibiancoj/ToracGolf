using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracLibrary.AspNet.Caching.Configuration
{

    /// <summary>
    /// Holds the configuration for a registered cache item. Holds the untyped generic version
    /// </summary>
    internal class CacheConfigurationUntyped
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// <param name="CacheDataSourceToSet">Data source for populating the cache</param>
        public CacheConfigurationUntyped(Func<object> CacheDataSourceToSet)
        {
            //we pass in () => to convert it to a func of object. So we invoke that action to just get the func. This way when we resolve, we just need to invoke once.
            CacheDataSource = (Func<object>)CacheDataSourceToSet.Invoke();
        }

        #endregion  

        #region Properties

        /// <summary>
        /// Data source for populating the cache
        /// </summary>
        public Func<object> CacheDataSource { get; }

        #endregion

    }

}