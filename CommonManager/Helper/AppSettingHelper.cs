using Microsoft.Extensions.Configuration;
using System;

namespace CommonManager.Helper
{
    /// <summary>
    /// 读取配置文件帮助类
    /// </summary>
    public static class AppSettingHelper
    {
        private static IConfiguration _config;

        /// <summary>
        /// 初始化配置
        /// </summary>
        public static void Init(IConfiguration configuration)
        {
            _config = configuration;
        }

        /// <summary>
        /// 读取字符串值
        /// </summary>
        public static string ReadAppSettings(string key)
        {
            try
            {
                return _config?[key] ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 读取嵌套配置
        /// </summary>
        public static string ReadAppSettings(params string[] keys)
        {
            try
            {
                if (keys == null || keys.Length == 0) return string.Empty;
                return _config?[string.Join(":", keys)] ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 读取实体对象
        /// </summary>
        public static T ReadAppSettings<T>(string key) where T : new()
        {
            try
            {
                var re = new T();
                _config?.GetSection(key).Bind(re);
                return re;
            }
            catch
            {
                return new T();
            }
        }
    }
}
