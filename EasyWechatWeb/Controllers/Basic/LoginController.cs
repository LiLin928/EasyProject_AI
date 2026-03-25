using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EasyWechatWeb.Controllers.Basic
{
    /// <summary>
    /// 登录认证控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IConfiguration configuration, ILogger<LoginController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("用户登录请求：{Username}", request.Username);

            // TODO: 验证用户凭据（从数据库查询）
            // 这里是示例代码，实际应该查询数据库验证
            if (request.Username == "admin" && request.Password == "123456")
            {
                var token = GenerateJwtToken(request.Username);
                
                _logger.LogInformation("用户登录成功：{Username}", request.Username);
                
                return Ok(new
                {
                    code = 200,
                    message = "登录成功",
                    data = new
                    {
                        token = token,
                        tokenType = "Bearer",
                        expiresIn = 3600,
                        userInfo = new
                        {
                            username = request.Username,
                            nickname = "管理员",
                            roles = new[] { "admin" }
                        }
                    }
                });
            }

            _logger.LogWarning("用户登录失败：{Username}", request.Username);
            
            return Unauthorized(new
            {
                code = 401,
                message = "用户名或密码错误"
            });
        }

        /// <summary>
        /// 刷新 Token
        /// </summary>
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            // TODO: 验证并刷新 Token
            return Ok(new
            {
                code = 200,
                message = "Token 刷新成功",
                data = new
                {
                    token = GenerateJwtToken("admin"),
                    tokenType = "Bearer",
                    expiresIn = 3600
                }
            });
        }

        /// <summary>
        /// 生成 JWT Token
        /// </summary>
        private string GenerateJwtToken(string username)
        {
            var jwtOptions = _configuration.GetSection("JWTTokenOptions");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions["SecurityKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim("username", username)
            };

            var token = new JwtSecurityToken(
                issuer: jwtOptions["Issuer"],
                audience: jwtOptions["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    /// <summary>
    /// 登录请求
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }

    /// <summary>
    /// 刷新 Token 请求
    /// </summary>
    public class RefreshTokenRequest
    {
        /// <summary>
        /// 刷新令牌
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
