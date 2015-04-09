using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace AntiTheftClub.Helphers
{
    public class MemoryCacher
    {
        DateTimeOffset absExpiration = DateTimeOffset.UtcNow.AddHours(1);

        public object GetValue(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Get(key);
        }

        public bool Add(string key, object value)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Add(key, value, absExpiration);
        }

        public void Delete(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            if (memoryCache.Contains(key))
            {
                memoryCache.Remove(key);
            }
        }
    }
}