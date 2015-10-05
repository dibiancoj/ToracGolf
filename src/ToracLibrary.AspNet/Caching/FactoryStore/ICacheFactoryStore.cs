using Microsoft.Framework.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracLibrary.AspNet.Caching.Configuration;

namespace ToracLibrary.AspNet.Caching.FactoryStore
{

    /// <summary>
    /// common interface which holds the cache configurations for all the registered caches. Gives the ability to implement into the default di container. di default container doesn't have factory names (which would be the cache key)
    /// </summary>
    public interface ICacheFactoryStore
    {

        /// <summary>
        /// Adds a configuration to the cache repository
        /// </summary>
        /// <typeparam name="T">Type of the cache</typeparam>
        /// <param name="ConfigurationToAdd">Configuration to add</param>
        void AddConfiguration<T>(string CacheKey, Func<T> DataSourceBuilder);

        /// <summary>
        /// Gets an item from the cache or the data source.
        /// </summary>
        /// <typeparam name="T">Type of the record to get</typeparam>
        /// <param name="CacheKey">Cache key</param>
        /// <param name="MemoryCacheToUse">Cache to use</param>
        /// <returns>The cached item. Or the item from the data source.</returns>
        T GetCacheItem<T>(string CacheKey, IMemoryCache MemoryCacheToUse);

    }

}