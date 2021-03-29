﻿using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            //UseRedisCache();
            await UseRedisCache(person);
            await UseHashRedisCache(person);

            await BenchMarkCache();
           

            Console.ReadLine();
        }


        private static async Task UseHashRedisCache(Person person)
        {
            RedisCache redisCache = new RedisCache();
            await redisCache.SetHashRecordAsync("12", "34", person);

            Person cachedValue = await redisCache.GetHashRecordAsync<Person>("12", "34");

            Console.WriteLine($"From Redis: {cachedValue}");
        }

        private static async Task UseRedisCache(Person person)
        {
            RedisCache redisCache = new RedisCache();
            await redisCache.SetRecordAsync("test", person);

            Person cachedValue = await redisCache.GetRecordAsync<Person>("test");

            Console.WriteLine($"From Redis: {cachedValue}");
        }

        private static void UseRedisCache()
        {
            var redisCache = new RedisCache();
            string value = "hello!";
            redisCache.Set("test2", value);

            var cachedValue = redisCache.Get("test2");

            Console.WriteLine($"From Redis: {cachedValue}");
        }

        private static void UsedDefaultCache(Person person)
        {
            var memoryCache = new DefaultMemoryCache();

            memoryCache.Set("person1", person);

            var personFromCache = memoryCache.Get<Person>("person1");

            Console.WriteLine($"From DefaultCache: {personFromCache}");
        }

        private static async Task BenchMarkCache()
        {
            RedisCache redisCache = new RedisCache();
            DefaultMemoryCache memoryCache = new DefaultMemoryCache();

            // DEFAULT MEMORY
            Stopwatch stopwatch = Stopwatch.StartNew();
           
            Person personFromCache = memoryCache.Get<Person>("person1");

            stopwatch.Stop();

            Console.WriteLine($"Default Memory Cache {stopwatch.ElapsedMilliseconds}");

            // DEFAULT REDIS
            stopwatch.Restart();

            Person redisPerson = await redisCache.GetRecordAsync<Person>("test");

            stopwatch.Stop();

            Console.WriteLine($"Redis Memory Cache {stopwatch.ElapsedMilliseconds}");

            // REDIS HASH
            stopwatch.Restart();

            Person redisHasPerson = await redisCache.GetHashRecordAsync<Person>("12", "34");

            stopwatch.Stop();

            Console.WriteLine($"Redis Hash Memory Cache {stopwatch.ElapsedMilliseconds}");

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
