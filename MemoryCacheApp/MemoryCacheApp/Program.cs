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
        static void Main(string[] args)
        {
            var memoryCache = new DefaultMemoryCache();
            var person = new Person()
            {
                FirstName = "Test",
                LastName = "Testy"
            };

            memoryCache.Set("person1", person);

            var personFromCache = memoryCache.Get<Person>("person1");

            Console.WriteLine($"From DefaultCache: {personFromCache}");
            RedisImplementation();
            Console.ReadLine();
        }


        static void RedisImplementation()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:5002");


            IDatabase db = redis.GetDatabase();
            string value = "abcdefg";
            db.StringSet("mykey", value);

            string cachedValue = db.StringGet("mykey");
            Console.WriteLine(cachedValue); 
        }
    }
}
