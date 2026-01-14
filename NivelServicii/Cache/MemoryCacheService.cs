using NivelServicii.Cache;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NivelServicii.Cache
{
    public class MemoryCacheService : ICache
    {
        private readonly int _cacheTime;

        public MemoryCacheService(int cacheTime = 60)
        {
            _cacheTime = cacheTime;
        }

        protected ObjectCache Cache => MemoryCache.Default;

        public T Get<T>(string key)
        {
            var jsonString = Cache[key] as string;
            if (string.IsNullOrEmpty(jsonString))
                return default(T);

            return JsonConvert.DeserializeObject<T>(jsonString, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None
            });
        }

        public void Set(string key, object data, int? cacheTime = null)
        {
            if (data == null) return;

            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(cacheTime ?? _cacheTime)
            };

            // Folosim JSON serialization în loc de BinaryFormatter
            // Aceasta este mai sigură pentru obiectele Entity Framework
            var jsonString = JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                NullValueHandling = NullValueHandling.Ignore
            });

            Cache.Set(key, jsonString, policy);
        }

        public bool IsSet(string key) => Cache.Contains(key);

        public void Remove(string key) => Cache.Remove(key);

        public void RemoveByPattern(string pattern)
        {
            foreach (var item in Cache)
                if (item.Key.StartsWith(pattern))
                    Remove(item.Key);
        }

        public void Clear()
        {
            foreach (var item in Cache)
                Remove(item.Key);
        }
    }
}
