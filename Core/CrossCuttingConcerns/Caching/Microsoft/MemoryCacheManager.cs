
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Core.Utilities.IoC;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Core.CrossCuttingConcerns.Caching.Microsoft
{
    //Adapter Pattern : adaptasyon deseni -> sistemimize bir şeyi entegre etmek. plug in gibi kullanmak. gerektiğinde değişebilir.
    public class MemoryCacheManager : ICacheManager
    {
        //işte burada microsoft tan using yapıyoruz IMemorycache kısmını. O bir interface bu yüzden onu çözmemiz lazım
        //ayrıca çalışma zincirimiz webapi business dataaccess şeklinde ilerliyor. aspect bağımlılık zincrinin içinde değil.
        //bu yüzden aspect ile çalışmamız için bunu da orada bağlamamız laızm.

        private IMemoryCache _memoryCache;

        public MemoryCacheManager()
        {
            _memoryCache = ServiceTool.ServiceProvider.GetService<IMemoryCache>();
            //Extension edilmiş bir paketi çağırdık. DependencyInjectionu.
        }

        public void Add(string key, object value, int duration)
        {
            _memoryCache.Set(key, value, TimeSpan.FromMinutes(duration));
            //cache mizi set et yani kur. key value ve zamana göre.
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
            //bir parametre verirsen Get (getir, al) için bununla çağır
        }

        public object Get(string key)
        {
            return _memoryCache.Get(key);
            //bu da yukarıdakinin aynısı ama hergangi bir isteğe dahil değil.
        }

        public bool IsAdd(string key)
        {
            return _memoryCache.TryGetValue(key, out _);
            //var mı yok mu true/false döndürecek ama 2.parametrede out _ dememizin sebebi dene ve değeri getirME demek için.
            //yani datayı istemiyorum sadece t f döndür.
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            var cacheEntriesCollectionDefinition = typeof(MemoryCache).GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var cacheEntriesCollection = cacheEntriesCollectionDefinition.GetValue(_memoryCache) as dynamic;
            List<ICacheEntry> cacheCollectionValues = new List<ICacheEntry>();

            foreach (var cacheItem in cacheEntriesCollection)
            {
                ICacheEntry cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);
                cacheCollectionValues.Add(cacheItemValue);
            }

            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = cacheCollectionValues.Where(d => regex.IsMatch(d.Key.ToString())).Select(d => d.Key).ToList();

            foreach (var key in keysToRemove)
            {
                _memoryCache.Remove(key);
            }

        }
    }
}
