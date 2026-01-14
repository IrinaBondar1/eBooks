using NivelServicii.Cache;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

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
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream((byte[])Cache[key]))
            {
                return (T)bf.Deserialize(ms);
            }
        }

        public void Set(string key, object data, int? cacheTime = null)
        {
            if (data == null) return;

            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(cacheTime ?? _cacheTime)
            };

            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, data);
                Cache.Set(key, ms.ToArray(), policy);
            }
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
