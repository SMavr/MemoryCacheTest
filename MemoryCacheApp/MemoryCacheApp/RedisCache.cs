using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryCacheApp
{
    public class RedisCache
    {

        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:5002");
        IDatabase db;

        public RedisCache()
        {
            db = redis.GetDatabase();
        }

        public void Set(string key, string value)
        {
            db.StringSet(key, value.ToString());
        }

        public string Get(string key)
        {
            return db.StringGet(key);
        }
    }
}
