using BusinessManager.Business.SrmManager.IService;
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
    public class PurchaseOrderController : BaseController
    {
        private readonly IPurchaseOrderService _service;
        private readonly ILogger<PurchaseOrderController> _logger;

        public PurchaseOrderController(IPurchaseOrderService service, ILogger<PurchaseOrderController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("list")]
        public async Task<ApiResponse<PageResponse<SrmPurchaseOrderRes>>> GetList(
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
                _logger.LogError(ex, "获取采购订单列表失败");
                return Error<PageResponse<SrmPurchaseOrderRes>>(ex.Message);
            }
        }

        [HttpGet("detail")]
        public async Task<ApiResponse<SrmPurchaseOrderRes>> GetDetail([FromQuery] long id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                return result == null ? ApiResponse<SrmPurchaseOrderRes>.Error("采购订单不存在", 404) : ApiResponse<SrmPurchaseOrderRes>.Success(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取采购订单详情失败");
                return ApiResponse<SrmPurchaseOrderRes>.Error(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ApiResponse<string>> Create([FromBody] SrmPurchaseOrderReq req)
        {
            try
            {
                var userId = GetCurrentUserId() ?? string.Empty;
                var id = await _service.CreateAsync(userId, req);
                return Success<string>(id, "创建成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "创建采购订单失败");
                return Error<string>(ex.Message);
            }
        }

        [HttpPost("update")]
        public async Task<ApiResponse<bool>> Update([FromBody] SrmPurchaseOrderReq req)
        {
            try
            {
                var result = await _service.UpdateAsync(req);
                return Success(result, "更新成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "更新采购订单失败");
                return Error<bool>(ex.Message);
            }
        }

        [HttpPost("delete")]
        public async Task<ApiResponse<bool>> Delete([FromQuery] string id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
                return Success(result, "删除成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "删除采购订单失败");
                return Error<bool>(ex.Message);
            }
        }

        [HttpPost("confirm")]
        public async Task<ApiResponse<bool>> Confirm([FromQuery] string id)
        {
            try
            {
                var result = await _service.ConfirmAsync(id);
                return Success(result, "确认成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "确认采购订单失败");
                return Error<bool>(ex.Message);
            }
        }

        [HttpPost("receive")]
        public async Task<ApiResponse<bool>> Receive([FromQuery] string id)
        {
            try
            {
                var result = await _service.ReceiveAsync(id);
                return Success(result, "收货成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "收货失败");
                return Error<bool>(ex.Message);
            }
        }
    }
}
