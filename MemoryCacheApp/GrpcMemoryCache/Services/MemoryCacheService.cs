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
        public override Task<CacheSavedReply> Save(CacheKeyValue request, ServerCallContext context)
        {
            return base.Save(request, context);
        }

        public override Task<CacheValue> Get(CacheKey request, ServerCallContext context)
        {
            return base.Get(request, context);
        }
    }
}
