using BusinessManager.SrmManager.IService;
using BusinessManager.SrmManager.Service;
using CommonManager.Base;
using EasyWechatModels.Dto;
using EasyWechatModels.Entitys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyWechatWeb.Controllers.Srm
{
    [ApiController]
    [Route("api/Srm/[controller]")]
    [Authorize]
    public class SettlementController : BaseController
    {
        private readonly ISettlementService _service;
        private readonly ILogger<SettlementController> _logger;

        public SettlementController(ISettlementService service, ILogger<SettlementController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("list")]
        public async Task<ApiResponse<PageResponse<SrmSettlementRes>>> GetList(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? status = null,
            [FromQuery] string keyword = null)
        {
            try
            {
                var result = await _service.GetPageDataAsync(pageIndex, pageSize, status, keyword);
                return Success(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取结算列表失败");
                return Error<PageResponse<SrmSettlementRes>>(ex.Message);
            }
        }

        [HttpGet("detail")]
        public async Task<ApiResponse<SrmSettlementRes>> GetDetail([FromQuery] long id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                return result == null ? ApiResponse<SrmSettlementRes>.Error("结算单不存在", 404) : ApiResponse<SrmSettlementRes>.Success(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取结算详情失败");
                return ApiResponse<SrmSettlementRes>.Error(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ApiResponse<long>> Create([FromBody] SrmSettlement req)
        {
            try
            {
                var id = await _service.CreateAsync(req);
                return Success(id, "创建成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "创建结算单失败");
                return Error<long>(ex.Message);
            }
        }

        [HttpPost("complete")]
        public async Task<ApiResponse<bool>> Complete([FromQuery] long id)
        {
            try
            {
                var result = await _service.CompleteAsync(id);
                return Success(result, "结算完成");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "结算失败");
                return Error<bool>(ex.Message);
            }
        }
    }
}
