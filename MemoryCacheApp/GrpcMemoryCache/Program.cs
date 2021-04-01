using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcMemoryCache
{
    public class Program
    {
        public static void Main(string[] args)
        {
            UseRedisCache();
            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        private static void UseRedisCache()
        {
            var redisCache = new RedisCache();
            string value = "I come from grpc!";
            redisCache.Set("grpctest", value);

            var cachedValue = redisCache.Get("grpctest");

            Console.WriteLine($"From Redis: {cachedValue}");
        }

    }
}
