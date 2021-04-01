using Grpc.Core;
using GrpcMemoryCache.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcMemoryCache.Services
{
    public class MemoryCacheService : MemoryCache.MemoryCacheBase
    {

        RedisCache redisCache = new RedisCache();

        public override async Task<CacheSavedReply> Save(CacheKeyValue request, ServerCallContext context)
        {
            try 
            {
                await Task.Run(() => redisCache.Set(request.Key, request.Value));
                return new CacheSavedReply
                {
                    Message = $"{request.Value} saved in memory cache!"
                };
            }
            catch (Exception ex)
            {
                return new CacheSavedReply
                {
                    Message = ex.Message
                };
            }
        }

        public override async Task<CacheValue> Get(CacheKey request, ServerCallContext context)
        {
            var cacheValue = await Task.Run(() => redisCache.Get(request.Key));
            return new CacheValue
            {
                Value = cacheValue
            };
        }
    }
}
