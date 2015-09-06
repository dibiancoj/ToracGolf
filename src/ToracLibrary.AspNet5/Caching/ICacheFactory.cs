using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracLibrary.AspNet5.Caching;

namespace ToracLibrary.AspNet5.Caching
{

    public interface ICacheFactory
    {
        //IList<InMemoryCache<object>> CacheItems { get; set; }
        InMemoryCache<T> ResolveCacheItem<T>(string CacheKeyName);
    }

}
