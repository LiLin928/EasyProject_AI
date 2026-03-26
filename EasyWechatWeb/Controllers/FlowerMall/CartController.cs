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
    public class CartController : BaseController
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        [HttpGet("list")]
        public async Task<ApiResponse<List<MallCartRes>>> GetList()
        {
            try
            {
                var userId = long.Parse(GetCurrentUserId() ?? "0");
                var result = await _cartService.GetListAsync(userId);
                return Success(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取购物车失败");
                return Error<List<MallCartRes>>(ex.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ApiResponse<long>> Add([FromBody] MallCartReq req)
        {
            try
            {
                var userId = long.Parse(GetCurrentUserId() ?? "0");
                var id = await _cartService.AddAsync(userId, req);
                return Success(id, "添加成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "添加购物车失败");
                return Error<long>(ex.Message);
            }
        }

        [HttpPost("update")]
        public async Task<ApiResponse<bool>> Update([FromBody] MallCartReq req)
        {
            try
            {
                var result = await _cartService.UpdateAsync(req);
                return Success(result, "更新成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "更新购物车失败");
                return Error<bool>(ex.Message);
            }
        }

        [HttpPost("delete")]
        public async Task<ApiResponse<bool>> Delete([FromQuery] long goodsId)
        {
            try
            {
                var userId = long.Parse(GetCurrentUserId() ?? "0");
                var result = await _cartService.DeleteAsync(userId, goodsId);
                return Success(result, "删除成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "删除购物车失败");
                return Error<bool>(ex.Message);
            }
        }

        [HttpPost("clear")]
        public async Task<ApiResponse<bool>> Clear()
        {
            try
            {
                var userId = GetCurrentUserId() ?? string.Empty;
                var cartList = await _cartService.GetListAsync(userId);
                foreach (var item in cartList)
                {
                    await _cartService.DeleteAsync(userId, item.GoodsId?.ToString() ?? string.Empty);
                }
                return Success(true, "清空成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "清空购物车失败");
                return Error<bool>(ex.Message);
            }
        }
    }
}
