using BusinessManager.BasicDataManager.IService;
using CommonManager.Base;
using EasyWechatModels.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyWechatWeb.Controllers.Basic
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DicController : BaseController
    {
        private readonly IDicService _dicService;
        private readonly ILogger<DicController> _logger;

        public DicController(IDicService dicService, ILogger<DicController> logger)
        {
            _dicService = dicService;
            _logger = logger;
        }

        [HttpGet("list")]
        public async Task<ApiResponse<PageResponse<BaseDicRes>>> GetList(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string keyword = null)
        {
            try
            {
                var result = await _dicService.GetPageDataAsync(pageIndex, pageSize, keyword);
                return Success(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取字典列表失败");
                return Error<PageResponse<BaseDicRes>>(ex.Message);
            }
        }

        [HttpGet("detail")]
        public async Task<ApiResponse<BaseDicRes>> GetDetail([FromQuery] long id)
        {
            try
            {
                var result = await _dicService.GetByIdAsync(id);
                return result == null ? ApiResponse<BaseDicRes>.Error("字典不存在", 404) : ApiResponse<BaseDicRes>.Success(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取字典详情失败");
                return ApiResponse<BaseDicRes>.Error(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ApiResponse<long>> Create([FromBody] BaseDicReq req)
        {
            try
            {
                var id = await _dicService.CreateAsync(req);
                return Success(id, "创建成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "创建字典失败");
                return Error<long>(ex.Message);
            }
        }

        [HttpPost("update")]
        public async Task<ApiResponse<bool>> Update([FromBody] BaseDicReq req)
        {
            try
            {
                var result = await _dicService.UpdateAsync(req);
                return Success(result, "更新成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "更新字典失败");
                return Error<bool>(ex.Message);
            }
        }

        [HttpPost("delete")]
        public async Task<ApiResponse<bool>> Delete([FromQuery] long id)
        {
            try
            {
                var result = await _dicService.DeleteAsync(id);
                return Success(result, "删除成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "删除字典失败");
                return Error<bool>(ex.Message);
            }
        }
    }
}
