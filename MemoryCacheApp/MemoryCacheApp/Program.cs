using Grpc.Core;
using StackExchange.Redis;
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
            //UsedDefaultCache(person);

            // Redis implementation
            //UseRedisCache();
            //await UseRedisCache(person);
            //await UseHashRedisCache(person);

            ////await BenchMarkCache();
            //await TestGrpc();

            await TestGrpcMemoryCache();

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

        private static async Task TestGrpc()
        {
            Channel channel = new Channel("localhost:5000", ChannelCredentials.Insecure);

            var client = new Greeter.GreeterClient(channel);
            String user = "you";

            var reply = client.SayHello(new HelloRequest { Name = user });
            Console.WriteLine("Greeting: " + reply.Message);

            await channel.ShutdownAsync();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static async Task TestGrpcMemoryCache()
        {
            Channel channel = new Channel("localhost:5000", ChannelCredentials.Insecure);


            var client = new MemoryCache.MemoryCacheClient(channel);

            var reply = await client.SaveAsync(new CacheKeyValue { Key = "grpc_memcache", Value = "hello from grpc Redis!" });

            Console.WriteLine($"Reply from Grpc: {reply}");

            var cachedValue = await client.GetAsync(new CacheKey = "grpc_memcache");

            Console.WriteLine($"CachedValue: {cachedValue}");
        }
    }
}
