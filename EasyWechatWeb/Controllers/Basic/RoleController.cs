using BusinessManager.BasicDataManager.IService;
using CommonManager.Base;
using EasyWechatModels.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace EasyWechatWeb.Controllers.Basic
{
    /// <summary>
    /// 角色管理控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleService roleService, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        [HttpGet("list")]
        public async Task<ApiResponse<PageResponse<BaseRoleRes>>> GetList(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string keyword = null)
        {
            try
            {
                var result = await _roleService.GetPageDataAsync(pageIndex, pageSize, keyword);
                return Success(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取角色列表失败");
                return Error<PageResponse<BaseRoleRes>>(ex.Message);
            }
        }

        /// <summary>
        /// 获取角色详情
        /// </summary>
        [HttpGet("detail")]
        [ProducesResponseType(typeof(ApiResponse<BaseRoleRes>), 200)]
        public async Task<ApiResponse<BaseRoleRes>> GetDetail([FromQuery] long id)
        {
            try
            {
                var result = await _roleService.GetByIdAsync(id);
                return result == null ? ApiResponse<BaseRoleRes>.Error("角色不存在", 404) : ApiResponse<BaseRoleRes>.Success(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取角色详情失败");
                return ApiResponse<BaseRoleRes>.Error(ex.Message);
            }
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        [HttpPost("create")]
        public async Task<ApiResponse<long>> Create([FromBody] BaseRoleReq req)
        {
            try
            {
                var id = await _roleService.CreateAsync(req);
                return Success(id, "创建成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "创建角色失败");
                return Error<long>(ex.Message);
            }
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        [HttpPost("update")]
        public async Task<ApiResponse<bool>> Update([FromBody] BaseRoleReq req)
        {
            try
            {
                var result = await _roleService.UpdateAsync(req);
                return Success(result, "更新成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "更新角色失败");
                return Error<bool>(ex.Message);
            }
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        [HttpPost("delete")]
        public async Task<ApiResponse<bool>> Delete([FromQuery] long id)
        {
            try
            {
                var result = await _roleService.DeleteAsync(id);
                return Success(result, "删除成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "删除角色失败");
                return Error<bool>(ex.Message);
            }
        }
    }
}
