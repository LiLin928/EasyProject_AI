using BusinessManager.SrmManager.IService;
using BusinessManager.SrmManager.Service;
using CommonManager.Base;
using EasyWechatModels.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyWechatWeb.Controllers.Srm
{
    [ApiController]
    [Route("api/Srm/[controller]")]
    [Authorize]
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _service;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService service, ILogger<PaymentController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("list")]
        public async Task<ApiResponse<PageResponse<SrmPaymentRequestRes>>> GetList(
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
                _logger.LogError(ex, "获取付款列表失败");
                return Error<PageResponse<SrmPaymentRequestRes>>(ex.Message);
            }
        }

        [HttpGet("detail")]
        public async Task<ApiResponse<SrmPaymentRequestRes>> GetDetail([FromQuery] long id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                return result == null ? ApiResponse<SrmPaymentRequestRes>.Error("付款申请不存在", 404) : ApiResponse<SrmPaymentRequestRes>.Success(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取付款详情失败");
                return ApiResponse<SrmPaymentRequestRes>.Error(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ApiResponse<long>> Create([FromBody] SrmPaymentRequestReq req)
        {
            try
            {
                var userId = long.Parse(GetCurrentUserId() ?? "0");
                var id = await _service.CreateAsync(userId, req);
                return Success(id, "创建成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "创建付款申请失败");
                return Error<long>(ex.Message);
            }
        }

        [HttpPost("approve")]
        public async Task<ApiResponse<bool>> Approve([FromQuery] long id, [FromQuery] bool approved)
        {
            try
            {
                var result = await _service.ApproveAsync(id, approved);
                return Success(result, approved ? "审批通过" : "审批拒绝");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "审批付款失败");
                return Error<bool>(ex.Message);
            }
        }

        [HttpPost("pay")]
        public async Task<ApiResponse<bool>> Pay([FromQuery] long id)
        {
            try
            {
                var result = await _service.PayAsync(id);
                return Success(result, "付款成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "付款失败");
                return Error<bool>(ex.Message);
            }
        }
    }
}
