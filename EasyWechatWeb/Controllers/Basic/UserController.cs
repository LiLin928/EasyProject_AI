using BusinessManager.BasicDataManager.IService;
using CommonManager.Base;
using CommonManager.Helper;
using EasyWechatModels.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace EasyWechatWeb.Controllers.Basic
{
    /// <summary>
    /// 用户管理控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        [HttpGet("list")]
        [ProducesResponseType(typeof(ApiResponse<PageResponse<BaseUsersRes>>), 200)]
        public async Task<ApiResponse<PageResponse<BaseUsersRes>>> GetList(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string keyword = null)
        {
            try
            {
                var result = await _userService.GetPageDataAsync(pageIndex, pageSize, keyword);
                return Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取用户列表失败");
                return Error<PageResponse<BaseUsersRes>>(ex.Message);
            }
        }

        /// <summary>
        /// 获取用户详情
        /// </summary>
        [HttpGet("detail")]
        [ProducesResponseType(typeof(ApiResponse<BaseUsersRes>), 200)]
        public async Task<ApiResponse<BaseUsersRes>> GetDetail([FromQuery] long id)
        {
            try
            {
                var result = await _userService.GetByIdAsync(id);
                if (result == null)
                    return ApiResponse<BaseUsersRes>.Error("用户不存在", 404);
                return ApiResponse<BaseUsersRes>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取用户详情失败");
                return ApiResponse<BaseUsersRes>.Error(ex.Message);
            }
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<long>), 200)]
        public async Task<ApiResponse<long>> Create([FromBody] BaseUsersReq req)
        {
            try
            {
                var id = await _userService.CreateAsync(req);
                return Success(id, "创建成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建用户失败");
                return Error<long>(ex.Message);
            }
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        [HttpPost("update")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<ApiResponse<bool>> Update([FromBody] BaseUsersReq req)
        {
            try
            {
                var result = await _userService.UpdateAsync(req);
                return Success(result, "更新成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新用户失败");
                return Error<bool>(ex.Message);
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        [HttpPost("delete")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<ApiResponse<bool>> Delete([FromQuery] long id)
        {
            try
            {
                var result = await _userService.DeleteAsync(id);
                return Success(result, "删除成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除用户失败");
                return Error<bool>(ex.Message);
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        [HttpPost("changePassword")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<ApiResponse<bool>> ChangePassword(
            [FromQuery] string oldPassword,
            [FromQuery] string newPassword)
        {
            try
            {
                var userId = long.Parse(GetCurrentUserId() ?? "0");
                var result = await _userService.ChangePasswordAsync(userId, oldPassword, newPassword);
                return Success(result, "密码修改成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "修改密码失败");
                return Error<bool>(ex.Message);
            }
        }
    }
}
