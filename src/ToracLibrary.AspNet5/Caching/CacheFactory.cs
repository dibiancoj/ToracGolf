using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracLibrary.AspNet5.Caching
{
    public class CacheFactory : ICacheFactory
    {

        public CacheFactory()
        {
            CacheItems = new List<Func<InMemoryCache<object>>>();
        }

        private IList<Func<InMemoryCache<object>>> CacheItems { get; set; }

        public void AddCacheFactory<T>(Func<InMemoryCache<T>> CacheToAdd)
        {
            var z = CacheToAdd;

            CacheItems.Add(z);
        }

        public InMemoryCache<T> ResolveCacheItem<T>(string CacheKeyName)
        {
            return CacheItems.First(x => x.CacheKey == CacheKeyName) as InMemoryCache<T>;
        }

    }
}
