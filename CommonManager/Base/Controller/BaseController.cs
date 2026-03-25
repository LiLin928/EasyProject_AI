using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CommonManager.Base
{
    /// <summary>
    /// 基础控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// 获取当前用户 ID
        /// </summary>
        protected string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
                   User.FindFirst("userId")?.Value ?? 
                   User.FindFirst("id")?.Value;
        }

        /// <summary>
        /// 获取当前用户名
        /// </summary>
        protected string GetCurrentUserName()
        {
            return User.FindFirst(ClaimTypes.Name)?.Value ??
                   User.FindFirst("username")?.Value ??
                   User.FindFirst("name")?.Value;
        }

        /// <summary>
        /// 获取当前用户昵称
        /// </summary>
        protected string GetCurrentNickName()
        {
            return User.FindFirst("nickName")?.Value ??
                   User.FindFirst("nickname")?.Value;
        }

        /// <summary>
        /// 检查是否登录
        /// </summary>
        protected bool IsLoggedIn()
        {
            return User.Identity?.IsAuthenticated ?? false;
        }

        /// <summary>
        /// 成功响应
        /// </summary>
        protected ApiResponse<T> Success<T>(T data, string message = "success")
        {
            return ApiResponse<T>.Success(data, message);
        }

        /// <summary>
        /// 成功响应（无数据）
        /// </summary>
        protected ApiResponse Success(string message = "success")
        {
            return ApiResponse.Success(message);
        }

        /// <summary>
        /// 错误响应
        /// </summary>
        protected ApiResponse<T> Error<T>(string message, int code = 500)
        {
            return ApiResponse<T>.Error(message, code);
        }

        /// <summary>
        /// 错误响应
        /// </summary>
        protected ApiResponse Error(string message, int code = 500)
        {
            return ApiResponse.Error(message, code);
        }

        /// <summary>
        /// 分页响应
        /// </summary>
        protected ApiResponse<PageResponse<T>> PageResult<T>(List<T> list, int total, int pageIndex = 1, int pageSize = 10)
        {
            var pageData = PageResponse<T>.Create(list, total, pageIndex, pageSize);
            return Success(pageData);
        }
    }
}
