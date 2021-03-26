using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MemoryCacheApp
{
    public class DefaultMemoryCache
    {
        private readonly MemoryCache memoryCache;
        private readonly CacheItemPolicy cacheItemPolicy;

        public DefaultMemoryCache()
        {
            this.memoryCache = new MemoryCache("Test");
            this.cacheItemPolicy = new CacheItemPolicy()
            {
                SlidingExpiration = TimeSpan.FromSeconds(60)
            };
        }

        public T Get<T>(string key)
        {
            return (T)memoryCache.Get(key);
        }

        public void Set<T>(string key, T value)
        {
            memoryCache.Set(key, value, cacheItemPolicy);
        }
    }
}
