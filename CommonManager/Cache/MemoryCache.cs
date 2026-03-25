using Microsoft.Extensions.Caching.Memory;
using System;

namespace CommonManager.Cache
{
    /// <summary>
    /// 内存缓存帮助类
    /// </summary>
    public static class MemoryCache
    {
        private static IMemoryCache _cache;

        /// <summary>
        /// 初始化缓存
        /// </summary>
        public static void Init(IMemoryCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        public static void Set<T>(string key, T value, TimeSpan? expiration = null)
        {
            if (_cache == null) return;

            var options = new MemoryCacheEntryOptions();
            if (expiration.HasValue)
            {
                options.AbsoluteExpirationRelativeToNow = expiration.Value;
            }

            _cache.Set(key, value, options);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        public static T Get<T>(string key)
        {
            if (_cache == null) return default;
            
            _cache.TryGetValue(key, out T value);
            return value;
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        public static void Remove(string key)
        {
            if (_cache == null) return;
            _cache.Remove(key);
        }

        /// <summary>
        /// 检查缓存是否存在
        /// </summary>
        public static bool Exists(string key)
        {
            if (_cache == null) return false;
            return _cache.TryGetValue(key, out _);
        }

        /// <summary>
        /// 获取或创建缓存
        /// </summary>
        public static T GetOrCreate<T>(string key, Func<T> factory, TimeSpan? expiration = null)
        {
            if (_cache == null) return factory();

            if (!_cache.TryGetValue(key, out T value))
            {
                value = factory();
                Set(key, value, expiration);
            }

            return value;
        }
    }
}
