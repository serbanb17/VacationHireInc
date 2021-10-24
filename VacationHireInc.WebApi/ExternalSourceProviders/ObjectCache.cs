// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;
using System.Collections.Concurrent;
using VacationHireInc.WebApi.Interfaces;

namespace VacationHireInc.WebApi.ExternalSourceProviders
{
    public class ObjectCache<T> : IObjectCache<T>
    {
        private ConcurrentDictionary<string, CacheValue<T>> _cache;

        public ObjectCache()
        {
            _cache = new ConcurrentDictionary<string, CacheValue<T>>();
        }

        public bool TryGet(string key, out T obj)
        {
            obj = default(T);
            if (_cache.TryGetValue(key, out var cacheValue) && cacheValue.Expiration > DateTime.Now)
            {
                obj = cacheValue.Object;
                return true;
            }
            return false;
        }

        public bool Add(string key, DateTime expiration, T obj)
        {
            if (expiration > DateTime.Now)
            {
                _cache[key] = new CacheValue<T>
                {
                    Expiration = expiration,
                    Object = obj
                };
                return true;
            }
            return false;
        }
    }

    internal class CacheValue<T>
    {
        public DateTime Expiration { get; set; }
        public T Object { get; set; }
    }
}
