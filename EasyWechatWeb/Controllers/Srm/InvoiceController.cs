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
    public class InvoiceController : BaseController
    {
        private readonly IInvoiceService _service;
        private readonly ILogger<InvoiceController> _logger;

        public InvoiceController(IInvoiceService service, ILogger<InvoiceController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("list")]
        public async Task<ApiResponse<PageResponse<SrmInvoiceRes>>> GetList(
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
                _logger.LogError(ex, "获取发票列表失败");
                return Error<PageResponse<SrmInvoiceRes>>(ex.Message);
            }
        }

        [HttpGet("detail")]
        public async Task<ApiResponse<SrmInvoiceRes>> GetDetail([FromQuery] long id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                return result == null ? ApiResponse<SrmInvoiceRes>.Error("发票不存在", 404) : ApiResponse<SrmInvoiceRes>.Success(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取发票详情失败");
                return ApiResponse<SrmInvoiceRes>.Error(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ApiResponse<long>> Create([FromBody] SrmInvoiceReq req)
        {
            try
            {
                var id = await _service.CreateAsync(req);
                return Success(id, "创建成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "创建发票失败");
                return Error<long>(ex.Message);
            }
        }

        [HttpPost("update")]
        public async Task<ApiResponse<bool>> Update([FromBody] SrmInvoiceReq req)
        {
            try
            {
                var result = await _service.UpdateAsync(req);
                return Success(result, "更新成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "更新发票失败");
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
                _logger.LogError(ex, "删除发票失败");
                return Error<bool>(ex.Message);
            }
        }

        [HttpPost("audit")]
        public async Task<ApiResponse<bool>> Audit([FromQuery] long id, [FromQuery] bool approved)
        {
            try
            {
                var result = await _service.AuditAsync(id, approved);
                return Success(result, approved ? "审核通过" : "审核拒绝");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "审核发票失败");
                return Error<bool>(ex.Message);
            }
        }
    }
}
