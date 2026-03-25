using BusinessManager.SrmManager.IService;
using CommonManager.Base;
using EasyWechatModels.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyWechatWeb.Controllers.Srm
{
    [ApiController]
    [Route("api/Srm/[controller]")]
    [Authorize]
    public class PurchaseRequestController : BaseController
    {
        private readonly IPurchaseRequestService _service;
        private readonly ILogger<PurchaseRequestController> _logger;

        public PurchaseRequestController(IPurchaseRequestService service, ILogger<PurchaseRequestController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("list")]
        public async Task<ApiResponse<PageResponse<SrmPurchaseRequestRes>>> GetList(
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
                _logger.LogError(ex, "获取采购申请列表失败");
                return Error<PageResponse<SrmPurchaseRequestRes>>(ex.Message);
            }
        }

        [HttpGet("detail")]
        public async Task<ApiResponse<SrmPurchaseRequestRes>> GetDetail([FromQuery] long id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                return result == null ? ApiResponse<SrmPurchaseRequestRes>.Error("采购申请不存在", 404) : ApiResponse<SrmPurchaseRequestRes>.Success(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取采购申请详情失败");
                return ApiResponse<SrmPurchaseRequestRes>.Error(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ApiResponse<long>> Create([FromBody] SrmPurchaseRequestReq req)
        {
            try
            {
                var userId = long.Parse(GetCurrentUserId() ?? "0");
                var id = await _service.CreateAsync(userId, req);
                return Success(id, "创建成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "创建采购申请失败");
                return Error<long>(ex.Message);
            }
        }

        [HttpPost("update")]
        public async Task<ApiResponse<bool>> Update([FromBody] SrmPurchaseRequestReq req)
        {
            try
            {
                var result = await _service.UpdateAsync(req);
                return Success(result, "更新成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "更新采购申请失败");
                return Error<bool>(ex.Message);
            }
        }

        [HttpPost("delete")]
        public async Task<ApiResponse<bool>> Delete([FromQuery] long id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
                return Success(result, "删除成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "删除采购申请失败");
                return Error<bool>(ex.Message);
            }
        }

        [HttpPost("approve")]
        public async Task<ApiResponse<bool>> Approve([FromQuery] long id, [FromQuery] bool approved)
        {
            try
            {
                var userId = long.Parse(GetCurrentUserId() ?? "0");
                var result = await _service.ApproveAsync(userId, id, approved);
                return Success(result, approved ? "审批通过" : "审批拒绝");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "审批采购申请失败");
                return Error<bool>(ex.Message);
            }
        }
    }
}
