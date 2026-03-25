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
        public async Task<ApiResponse<SrmInvoiceRes>> GetDetail([FromQuery] string id)
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
        public async Task<ApiResponse<string>> Create([FromBody] SrmInvoiceReq req)
        {
            try
            {
                var id = await _service.CreateAsync(req);
                return Success(id, "创建成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "创建发票失败");
                return Error<string>(ex.Message);
            }
        }

        [HttpPost("audit")]
        public async Task<ApiResponse<bool>> Audit([FromQuery] string id, [FromQuery] bool approved)
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
