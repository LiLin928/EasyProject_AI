using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CommonManager.Helper
{
    /// <summary>
    /// JWT Token 帮助类
    /// </summary>
    public static class CustomerJWTHelper
    {
        private static string _securityKey;
        private static string _issuer;
        private static string _audience;
        private static int _accessTokenExpirationMinutes = 60;
        private static int _refreshTokenExpirationDays = 7;

        /// <summary>
        /// 初始化 JWT 配置
        /// </summary>
        public static void Init(string securityKey, string issuer, string audience, int accessTokenExpirationMinutes = 60)
        {
            _securityKey = securityKey;
            _issuer = issuer;
            _audience = audience;
            _accessTokenExpirationMinutes = accessTokenExpirationMinutes;
        }

        /// <summary>
        /// 生成 Token
        /// </summary>
        public static string GenerateToken(string userId, string username, Dictionary<string, string> additionalClaims = null)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, username),
                new Claim("userId", userId),
                new Claim("username", username)
            };

            if (additionalClaims != null)
            {
                foreach (var claim in additionalClaims)
                {
                    claims.Add(new Claim(claim.Key, claim.Value));
                }
            }

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_accessTokenExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 验证 Token
        /// </summary>
        public static ClaimsPrincipal ValidateToken(string token)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey));
                
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                var jwtToken = tokenHandler.ReadJwtToken(token);
                var claimsIdentity = new ClaimsIdentity(jwtToken.Claims);
                return new ClaimsPrincipal(claimsIdentity);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 刷新 Token
        /// </summary>
        public static string RefreshToken(string oldToken)
        {
            var principal = ValidateToken(oldToken);
            if (principal == null)
                return null;

            var userId = principal.FindFirst("userId")?.Value;
            var username = principal.FindFirst("username")?.Value;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(username))
                return null;

            return GenerateToken(userId, username);
        }

        /// <summary>
        /// 生成 Refresh Token
        /// </summary>
        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// 从 Token 中获取用户 ID
        /// </summary>
        public static string GetUserIdFromToken(string token)
        {
            var principal = ValidateToken(token);
            return principal?.FindFirst("userId")?.Value;
        }

        /// <summary>
        /// 从 Token 中获取用户名
        /// </summary>
        public static string GetUserNameFromToken(string token)
        {
            var principal = ValidateToken(token);
            return principal?.FindFirst("username")?.Value;
        }
    }
}
