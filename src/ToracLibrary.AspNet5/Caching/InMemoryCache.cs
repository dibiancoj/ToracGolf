using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.Caching.Memory;

namespace ToracLibrary.AspNet5.Caching
{

    //add the following to startup (ConfigureServices)
    //services.AddSingleton<IMemoryCache, MemoryCache>();

    /// <summary>
    /// Abstracts the memory mechanism for asp.net 5.0
    /// </summary>
    public class InMemoryCache<T>
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="CacheKeyToSet">Cache key to use</param>
        /// <param name="GetFromDataSourceToSet">Function to build the data set</param>
        public InMemoryCache(string CacheKeyToSet, Func<T> GetFromDataSourceToSet)
        {
            CacheKey = CacheKeyToSet;
            DataSourceBuilder = GetFromDataSourceToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Cache key to use
        /// </summary>
        public string CacheKey { get; }

        /// <summary>
        /// Function to build the data set
        /// </summary>
        private Func<T> DataSourceBuilder { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Get a cache item. If it can't be found, we go build it and return it
        /// </summary>
        /// <param name="CacheImplementation">Asp.net 5 cache implementation</param>
        /// <returns>Cached item</returns>
        public T GetCacheItem(IMemoryCache CacheImplementation)
        {
            //object we will try to get
            object CacheTryToGet;

            //try to get the cache item
            if (CacheImplementation.TryGetValue(CacheKey, out CacheTryToGet))
            {
                return (T)CacheTryToGet;
            }
            else
            {
                //we need to go build the data set, add it to the cache and return it
                var BuiltItemToCache = DataSourceBuilder.Invoke();

                //set the cache 
                CacheImplementation.Set(CacheKey, BuiltItemToCache);

                //now return the cached item
                return BuiltItemToCache;
            }
        }

        #endregion

    }

}
