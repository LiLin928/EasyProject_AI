using System;
using System.Security.Cryptography;

namespace CommonManager.Helper
{
    /// <summary>
    /// 随机数帮助类
    /// </summary>
    public static class RandomHelper
    {
        private static readonly Random Random = new Random();

        /// <summary>
        /// 生成随机整数
        /// </summary>
        public static int Generate(int minValue, int maxValue)
        {
            return Random.Next(minValue, maxValue);
        }

        /// <summary>
        /// 生成随机数
        /// </summary>
        public static double GenerateDouble()
        {
            return Random.NextDouble();
        }

        /// <summary>
        /// 生成随机字符串
        /// </summary>
        public static string GenerateString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[Random.Next(chars.Length)];
            }
            return new string(result);
        }

        /// <summary>
        /// 生成随机验证码
        /// </summary>
        public static string GenerateCaptcha(int length = 6)
        {
            const string chars = "23456789ABCDEFGHJKLMNPQRSTUVWXYZ";
            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[Random.Next(chars.Length)];
            }
            return new string(result);
        }

        /// <summary>
        /// 生成加密随机数
        /// </summary>
        public static int GenerateSecureInt(int minValue, int maxValue)
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            var num = BitConverter.ToInt32(bytes, 0);
            return minValue + (Math.Abs(num) % (maxValue - minValue));
        }

        /// <summary>
        /// 生成 GUID
        /// </summary>
        public static string GenerateGuid()
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 生成短 GUID（16 位）
        /// </summary>
        public static string GenerateShortGuid()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 16);
        }
    }
}
