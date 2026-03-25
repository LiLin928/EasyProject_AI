using BusinessManager.FlowerMallManager.IService;
using CommonManager.Base;
using EasyWechatModels.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyWechatWeb.Controllers.FlowerMall
{
    [ApiController]
    [Route("api/Mall/[controller]")]
    public class BannerController : BaseController
    {
        private readonly IBannerService _bannerService;
        private readonly ILogger<BannerController> _logger;

        public BannerController(IBannerService bannerService, ILogger<BannerController> logger)
        {
            _bannerService = bannerService;
            _logger = logger;
        }

        [HttpGet("list")]
        [AllowAnonymous]
        public async Task<ApiResponse<List<MallBannerRes>>> GetList()
        {
            try
            {
                var result = await _bannerService.GetListAsync();
                return Success(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取轮播图失败");
                return Error<List<MallBannerRes>>(ex.Message);
            }
        }
    }
}
