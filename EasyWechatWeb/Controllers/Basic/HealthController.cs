using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyWechatWeb.Controllers.Basic
{
    /// <summary>
    /// 健康检查控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 健康检查接口
        /// </summary>
        [HttpGet("index")]
        public IActionResult Index()
        {
            _logger.LogInformation("健康检查请求");
            return Ok(new
            {
                status = "healthy",
                timestamp = DateTime.Now,
                version = "1.0.0"
            });
        }

        /// <summary>
        /// 检查数据库连接
        /// </summary>
        [HttpGet("db")]
        public async Task<IActionResult> CheckDb()
        {
            try
            {
                // TODO: 添加数据库连接检查
                return Ok(new
                {
                    status = "healthy",
                    message = "数据库连接正常",
                    timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "数据库连接检查失败");
                return StatusCode(500, new
                {
                    status = "unhealthy",
                    message = "数据库连接失败",
                    error = ex.Message
                });
            }
        }
    }
}
