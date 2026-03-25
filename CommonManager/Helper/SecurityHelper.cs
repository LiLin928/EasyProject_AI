using System;
using System.Security.Cryptography;
using System.Text;

namespace CommonManager.Helper
{
    /// <summary>
    /// 安全帮助类
    /// </summary>
    public static class SecurityHelper
    {
        /// <summary>
        /// MD5 加密
        /// </summary>
        public static string HashMD5(string input)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        /// <summary>
        /// SHA256 加密
        /// </summary>
        public static string HashSHA256(string input)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        /// <summary>
        /// Base64 编码
        /// </summary>
        public static string Base64Encode(string input)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
        }

        /// <summary>
        /// Base64 解码
        /// </summary>
        public static string Base64Decode(string input)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(input));
        }

        /// <summary>
        /// AES 加密
        /// </summary>
        public static string AESEncrypt(string plainText, string key, string iv)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
            aes.IV = Encoding.UTF8.GetBytes(iv.PadRight(16).Substring(0, 16));

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// AES 解密
        /// </summary>
        public static string AESDecrypt(string cipherText, string key, string iv)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
            aes.IV = Encoding.UTF8.GetBytes(iv.PadRight(16).Substring(0, 16));

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }

        /// <summary>
        /// 生成 HMACSHA256 签名
        /// </summary>
        public static string GenerateHMACSHA256(string message, string secret)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        /// <summary>
        /// 验证 HMACSHA256 签名
        /// </summary>
        public static bool VerifyHMACSHA256(string message, string signature, string secret)
        {
            var expectedSignature = GenerateHMACSHA256(message, secret);
            return string.Equals(expectedSignature, signature, StringComparison.OrdinalIgnoreCase);
        }
    }
}
