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
    public class AddressController : BaseController
    {
        private readonly IAddressService _addressService;
        private readonly ILogger<AddressController> _logger;

        public AddressController(IAddressService addressService, ILogger<AddressController> logger)
        {
            _addressService = addressService;
            _logger = logger;
        }

        [HttpGet("list")]
        public async Task<ApiResponse<List<MallAddressRes>>> GetList()
        {
            try
            {
                var userId = long.Parse(GetCurrentUserId() ?? "0");
                var result = await _addressService.GetListAsync(userId);
                return Success(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取地址列表失败");
                return Error<List<MallAddressRes>>(ex.Message);
            }
        }

        [HttpGet("detail")]
        public async Task<ApiResponse<MallAddressRes>> GetDetail([FromQuery] long id)
        {
            try
            {
                var result = await _addressService.GetByIdAsync(id);
                return result == null ? ApiResponse<MallAddressRes>.Error("地址不存在", 404) : ApiResponse<MallAddressRes>.Success(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取地址详情失败");
                return ApiResponse<MallAddressRes>.Error(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ApiResponse<long>> Create([FromBody] MallAddressReq req)
        {
            try
            {
                var userId = long.Parse(GetCurrentUserId() ?? "0");
                var id = await _addressService.CreateAsync(userId, req);
                return Success(id, "创建成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "创建地址失败");
                return Error<long>(ex.Message);
            }
        }

        [HttpPost("update")]
        public async Task<ApiResponse<bool>> Update([FromBody] MallAddressReq req)
        {
            try
            {
                var userId = long.Parse(GetCurrentUserId() ?? "0");
                var result = await _addressService.UpdateAsync(userId, req);
                return Success(result, "更新成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "更新地址失败");
                return Error<bool>(ex.Message);
            }
        }

        [HttpPost("delete")]
        public async Task<ApiResponse<bool>> Delete([FromQuery] long id)
        {
            try
            {
                var userId = long.Parse(GetCurrentUserId() ?? "0");
                var result = await _addressService.DeleteAsync(userId, id);
                return Success(result, "删除成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "删除地址失败");
                return Error<bool>(ex.Message);
            }
        }

        [HttpPost("setDefault")]
        public async Task<ApiResponse<bool>> SetDefault([FromQuery] string id)
        {
            try
            {
                // 简化实现：直接更新
                var address = await _addressService.GetByIdAsync(id);
                if (address == null) return Error<bool>("地址不存在", 404);
                
                var userId = GetCurrentUserId() ?? string.Empty;
                // 先将该用户所有地址设为非默认
                var allAddresses = await _addressService.GetPageDataAsync(1, 100, userId);
                foreach (var addr in allAddresses.Data?.List ?? new List<MallAddressRes>())
                {
                    var updateReq = addr.Adapt<MallAddressReq>();
                    updateReq.IsDefault = (addr.Id == id) ? 1 : 0;
                    await _addressService.UpdateAsync(updateReq);
                }
                
                return Success(true, "设置成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "设置默认地址失败");
                return Error<bool>(ex.Message);
            }
        }
    }
}
