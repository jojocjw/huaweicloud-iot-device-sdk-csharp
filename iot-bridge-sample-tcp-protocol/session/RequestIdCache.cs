using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using NLog;

namespace IoT.Bridge.Sample.Tcp.Session
{
    class RequestIdCache
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static readonly string SEPARATOR = ":";

        private static readonly RequestIdCache INSTANCE = new RequestIdCache();

        private readonly MemoryCache cache = new MemoryCache(new MemoryCacheOptions()
        {
            SizeLimit = 20000
        });

        public static RequestIdCache GetInstance()
        {
            return INSTANCE;
        }

        private string GetKey(string deviceId, string flowNo)
        {
            return deviceId + SEPARATOR + flowNo;
        }

        public void SetRequestId(string deviceId, string flowNo, string requestId)
        {
            cache.Set(GetKey(deviceId, flowNo), requestId);
        }

        public string RemoveRequestId(string deviceId, string flowNo)
        {
            string key = GetKey(deviceId, flowNo);
            try
            {
                string value = cache.Get(key).ToString();
                cache.Remove(key);
                return value;
            }
            catch (Exception e)
            {
                Log.Warn("getRequestId error : {0} for key: {1}", e.StackTrace, key);
                return null;
            }
        }

    }
}
