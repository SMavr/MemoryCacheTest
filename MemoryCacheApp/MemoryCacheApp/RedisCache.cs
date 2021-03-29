using Newtonsoft.Json;
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

        public async Task SetRecordAsync<T>(string key, T data)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            await db.StringSetAsync(key, jsonData);
        }

        public async Task<T> GetRecordAsync<T>(string key)
        {
            RedisValue jsonData = await db.StringGetAsync(key);
            T data = JsonConvert.DeserializeObject<T>(jsonData);
            return data;
        }


        public void TestExpiration()
        {
            db.StringSet("expire", "TestExpireData", TimeSpan.FromSeconds(60));
        }
    }
}
