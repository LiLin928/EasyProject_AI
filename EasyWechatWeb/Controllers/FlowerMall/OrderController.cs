using BusinessManager.FlowerMallManager.IService;
using CommonManager.Base;
using EasyWechatModels.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyWechatWeb.Controllers.FlowerMall
{
    [ApiController]
    [Route("api/Mall/[controller]")]
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet("list")]
        public async Task<ApiResponse<PageResponse<MallOrderRes>>> GetList(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? status = null)
        {
            try
            {
                var userId = long.Parse(GetCurrentUserId() ?? "0");
                var result = await _orderService.GetPageDataAsync(userId, pageIndex, pageSize, status);
                return Success(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取订单列表失败");
                return Error<PageResponse<MallOrderRes>>(ex.Message);
            }
        }

        [HttpGet("detail")]
        public async Task<ApiResponse<MallOrderRes>> GetDetail([FromQuery] long id)
        {
            try
            {
                var result = await _orderService.GetByIdAsync(id);
                return result == null ? ApiResponse<MallOrderRes>.Error("订单不存在", 404) : ApiResponse<MallOrderRes>.Success(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取订单详情失败");
                return ApiResponse<MallOrderRes>.Error(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ApiResponse<long>> Create([FromBody] MallOrderReq req)
        {
            try
            {
                var userId = long.Parse(GetCurrentUserId() ?? "0");
                var id = await _orderService.CreateAsync(userId, req);
                return Success(id, "创建成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "创建订单失败");
                return Error<long>(ex.Message);
            }
        }

        [HttpPost("pay")]
        public async Task<ApiResponse<bool>> Pay([FromQuery] long id)
        {
            try
            {
                var userId = long.Parse(GetCurrentUserId() ?? "0");
                var result = await _orderService.PayAsync(userId, id);
                return Success(result, "支付成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "支付失败");
                return Error<bool>(ex.Message);
            }
        }

        [HttpPost("cancel")]
        public async Task<ApiResponse<bool>> Cancel([FromQuery] long id)
        {
            try
            {
                var userId = long.Parse(GetCurrentUserId() ?? "0");
                var result = await _orderService.CancelAsync(userId, id);
                return Success(result, "取消成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "取消订单失败");
                return Error<bool>(ex.Message);
            }
        }
    }
}
