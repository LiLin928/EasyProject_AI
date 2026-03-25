using BusinessManager.FlowerMallManager.IService;
using CommonManager.Base;
using EasyWechatModels.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyWechatWeb.Controllers.FlowerMall
{
    [ApiController]
    [Route("api/Mall/[controller]")]
    public class GoodsController : BaseController
    {
        private readonly IGoodsService _goodsService;
        private readonly ILogger<GoodsController> _logger;

        public GoodsController(IGoodsService goodsService, ILogger<GoodsController> logger)
        {
            _goodsService = goodsService;
            _logger = logger;
        }

        [HttpGet("list")]
        [AllowAnonymous]
        public async Task<ApiResponse<PageResponse<MallGoodsRes>>> GetList(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? categoryId = null,
            [FromQuery] string keyword = null)
        {
            try
            {
                var result = await _goodsService.GetPageDataAsync(pageIndex, pageSize, categoryId, keyword);
                return Success(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取商品列表失败");
                return Error<PageResponse<MallGoodsRes>>(ex.Message);
            }
        }

        [HttpGet("detail")]
        [AllowAnonymous]
        public async Task<ApiResponse<MallGoodsRes>> GetDetail([FromQuery] long id)
        {
            try
            {
                var result = await _goodsService.GetByIdAsync(id);
                return result == null ? ApiResponse<MallGoodsRes>.Error("商品不存在", 404) : ApiResponse<MallGoodsRes>.Success(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取商品详情失败");
                return ApiResponse<MallGoodsRes>.Error(ex.Message);
            }
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<ApiResponse<long>> Create([FromBody] MallGoodsReq req)
        {
            try
            {
                var id = await _goodsService.CreateAsync(req);
                return Success(id, "创建成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "创建商品失败");
                return Error<long>(ex.Message);
            }
        }

        [HttpPost("update")]
        [Authorize]
        public async Task<ApiResponse<bool>> Update([FromBody] MallGoodsReq req)
        {
            try
            {
                var result = await _goodsService.UpdateAsync(req);
                return Success(result, "更新成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "更新商品失败");
                return Error<bool>(ex.Message);
            }
        }

        [HttpPost("delete")]
        [Authorize]
        public async Task<ApiResponse<bool>> Delete([FromQuery] long id)
        {
            try
            {
                var result = await _goodsService.DeleteAsync(id);
                return Success(result, "删除成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "删除商品失败");
                return Error<bool>(ex.Message);
            }
        }
    }
}
