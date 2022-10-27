using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using NLog;

namespace IoT.SDK.Bridge.Request
{
    public class RequestIdCache
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private MemoryCache futureCache;

        public RequestIdCache()
        {
            futureCache = new MemoryCache(new MemoryCacheOptions()
            {
                SizeLimit = 2000
            });
        }

        public void SetRequestId2Cache(string requestId, TaskCompletionSource<int> future)
        {
            futureCache.Set(requestId, future, new MemoryCacheEntryOptions()
            {
                SlidingExpiration = TimeSpan.FromMinutes(3)
            });
        }

        public void InvalidateCache(string key)
        {
            futureCache.Remove(key);
        }

        public TaskCompletionSource<int> GetFuture(string requestId)
        {
            try
            {
                TaskCompletionSource<int> value = (TaskCompletionSource<int>)futureCache.Get(requestId);
                InvalidateCache(requestId);
                return value;
            }
            catch (Exception e)
            {
                Log.Warn("getRequestId error : {0} for key: {1}", e, requestId);
                return null;
            }
        }
    }
}
