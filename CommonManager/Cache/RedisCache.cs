using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace CommonManager.Cache
{
    /// <summary>
    /// Redis 缓存帮助类
    /// </summary>
    public static class RedisCache
    {
        private static IDistributedCache _cache;

        /// <summary>
        /// 初始化缓存
        /// </summary>
        public static void Init(IDistributedCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        public static async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            if (_cache == null) return;
            
            var options = new DistributedCacheEntryOptions();
            if (expiration.HasValue)
            {
                options.AbsoluteExpirationRelativeToNow = expiration.Value;
            }

            var json = JsonConvert.SerializeObject(value);
            await _cache.SetStringAsync(key, json, options);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        public static async Task<T> GetAsync<T>(string key)
        {
            if (_cache == null) return default;
            
            var json = await _cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(json))
                return default;

            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        public static async Task RemoveAsync(string key)
        {
            if (_cache == null) return;
            await _cache.RemoveAsync(key);
        }

        /// <summary>
        /// 检查缓存是否存在
        /// </summary>
        public static async Task<bool> ExistsAsync(string key)
        {
            if (_cache == null) return false;
            var value = await _cache.GetStringAsync(key);
            return !string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// 设置字符串缓存
        /// </summary>
        public static async Task SetStringAsync(string key, string value, TimeSpan? expiration = null)
        {
            if (_cache == null) return;
            
            var options = new DistributedCacheEntryOptions();
            if (expiration.HasValue)
            {
                options.AbsoluteExpirationRelativeToNow = expiration.Value;
            }

            await _cache.SetStringAsync(key, value, options);
        }

        /// <summary>
        /// 获取字符串缓存
        /// </summary>
        public static async Task<string> GetStringAsync(string key)
        {
            if (_cache == null) return null;
            return await _cache.GetStringAsync(key);
        }

        /// <summary>
        /// 自增
        /// </summary>
        public static async Task<long> IncrementAsync(string key)
        {
            if (_cache == null) return 0;
            // Redis 自增需要特殊处理，这里简化实现
            var value = await GetStringAsync(key);
            if (long.TryParse(value, out var num))
            {
                num++;
                await SetStringAsync(key, num.ToString());
                return num;
            }
            return 0;
        }
    }
}
