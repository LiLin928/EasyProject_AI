using Newtonsoft.Json;
using System;

namespace CommonManager.Helper
{
    /// <summary>
    /// JSON 序列化帮助类
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// 序列化为 JSON 字符串
        /// </summary>
        public static string ToJson(this object obj)
        {
            if (obj == null) return string.Empty;
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 序列化为 JSON 字符串（格式化）
        /// </summary>
        public static string ToJsonFormat(this object obj)
        {
            if (obj == null) return string.Empty;
            try
            {
                return JsonConvert.SerializeObject(obj, Formatting.Indented);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 反序列化为对象
        /// </summary>
        public static T ToObject<T>(this string json)
        {
            if (string.IsNullOrEmpty(json)) return default(T);
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 反序列化为对象
        /// </summary>
        public static object ToObject(this string json, Type type)
        {
            if (string.IsNullOrEmpty(json)) return null;
            try
            {
                return JsonConvert.DeserializeObject(json, type);
            }
            catch
            {
                return null;
            }
        }
    }
}
