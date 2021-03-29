using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryCacheApp
{
    partial class Program
    {
        static async Task Main(string[] args)
        {
            Person person = CreateTestPerson();

            // Microsoft implementation
            UsedDefaultCache(person);

            // Redis implementation
            //UserRedisCache();
            await UserRedisCache(person);

            Console.ReadLine();
        }

        private static async Task UserRedisCache(Person person)
        {
            RedisCache redisCache = new RedisCache();
            await redisCache.SetRecordAsync("test", person);

            Person cachedValue = await redisCache.GetRecordAsync<Person>("test");

            Console.WriteLine($"From Redis: {cachedValue}");
        }

        private static void UserRedisCache()
        {
            var redisCache = new RedisCache();
            string value = "hello!";
            redisCache.Set("test", value);

            var cachedValue = redisCache.Get("test");

            Console.WriteLine($"From Redis: {cachedValue}");
        }

        private static void UsedDefaultCache(Person person)
        {
            var memoryCache = new DefaultMemoryCache();

            memoryCache.Set("person1", person);

            var personFromCache = memoryCache.Get<Person>("person1");

            Console.WriteLine($"From DefaultCache: {personFromCache}");
        }

        private static Person CreateTestPerson()
        {
            return new Person()
            {
                FirstName = "Test",
                LastName = "Testy"
            };
        }
    }
}
