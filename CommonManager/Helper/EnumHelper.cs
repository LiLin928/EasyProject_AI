using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CommonManager.Helper
{
    /// <summary>
    /// 枚举帮助类
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        public static string GetDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            if (field == null) return value.ToString();
            
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }

        /// <summary>
        /// 获取枚举列表（带描述）
        /// </summary>
        public static List<EnumItem> GetList<T>() where T : Enum
        {
            var type = typeof(T);
            var result = new List<EnumItem>();

            foreach (var value in Enum.GetValues(type))
            {
                var enumValue = (Enum)value;
                result.Add(new EnumItem
                {
                    Value = Convert.ToInt32(enumValue),
                    Name = enumValue.ToString(),
                    Description = GetDescription(enumValue)
                });
            }

            return result;
        }

        /// <summary>
        /// 根据值获取枚举描述
        /// </summary>
        public static string GetDescriptionByValue<T>(int value) where T : Enum
        {
            if (!Enum.IsDefined(typeof(T), value))
                return string.Empty;

            var enumValue = (Enum)Enum.ToObject(typeof(T), value);
            return GetDescription(enumValue);
        }

        /// <summary>
        /// 根据名称获取枚举值
        /// </summary>
        public static int GetValueByName<T>(string name) where T : struct, Enum
        {
            if (Enum.TryParse<T>(name, true, out var result))
            {
                return Convert.ToInt32(result);
            }
            return -1;
        }

        /// <summary>
        /// 枚举项
        /// </summary>
        public class EnumItem
        {
            /// <summary>
            /// 值
            /// </summary>
            public int Value { get; set; }

            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 描述
            /// </summary>
            public string Description { get; set; }
        }
    }
}
