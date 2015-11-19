using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracLibrary.AspNet.Caching.Configuration;

namespace ToracLibrary.AspNet.Caching.FactoryStore
{

    /// <summary>
    /// common implementation which holds the cache configurations for all the registered caches
    /// </summary>
    public class CacheFactoryStore : ICacheFactoryStore
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public CacheFactoryStore()
        {
            //create a new dictionary
            RegisteredCaches = new Dictionary<string, CacheConfigurationUntyped>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Holds all the cache configurations
        /// </summary>
        private IDictionary<string, CacheConfigurationUntyped> RegisteredCaches { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a configuration to the cache repository
        /// </summary>
        /// <typeparam name="T">Type of the cache</typeparam>
        /// <param name="CacheKey">Key for the cache</param>
        /// <param name="DataSourceBuilder">The method to build the data set when we can't find it in the cache</param>
        public void AddConfiguration<T>(string CacheKey, Func<T> DataSourceBuilder)
        {
            //go add the untyped configuration to the dictionary
            RegisteredCaches.Add(CacheKey, new CacheConfiguration<T>(DataSourceBuilder));
        }

        /// <summary>
        /// Gets an item from the cache or the data source.
        /// </summary>
        /// <typeparam name="T">Type of the record to get</typeparam>
        /// <param name="CacheKey">Cache key</param>
        /// <param name="MemoryCacheToUse">Cache to use</param>
        /// <returns>The cached item. Or the item from the data source.</returns>
        public T GetCacheItem<T>(string CacheKey, IMemoryCache MemoryCacheToUse)
        {
            //item to get
            T TryToGetItem;

            //go try to find it in the cache
            if (MemoryCacheToUse.TryGetValue<T>(CacheKey, out TryToGetItem))
            {
                //we found the item in the cache, go return it
                return TryToGetItem;
            }
            else
            {
                //we need to grab it from the data source, put it in the cache, then return it
                //go grab the configurationk
                var Config = RegisteredCaches[CacheKey];

                //go get the data
                var DataToReturn = Config.CacheDataSource.Invoke();

                //now put it in the cache
                MemoryCacheToUse.Set(CacheKey, DataToReturn);

                //now return the data
                return (T)DataToReturn;
            }
        }

        #endregion

    }

}