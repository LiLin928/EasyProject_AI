using BusinessManager.WorkflowManager.IService;
using BusinessManager.WorkflowManager.Service;
using CommonManager.Base;
using EasyWechatModels.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyWechatWeb.Controllers.Workflow
{
    [ApiController]
    [Route("api/Workflow/[controller]")]
    [Authorize]
    public class WorkflowController : BaseController
    {
        private readonly BusinessManager.WorkflowManager.IService.IWorkflowService _service;
        private readonly ILogger<WorkflowController> _logger;

        public WorkflowController(BusinessManager.WorkflowManager.IService.IWorkflowService service, ILogger<WorkflowController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("list")]
        public async Task<ApiResponse<PageResponse<WorkflowDefinitionRes>>> GetList(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string keyword = null)
        {
            try
            {
                var result = await _service.GetPageDataAsync(pageIndex, pageSize, keyword);
                return Success(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取工作流列表失败");
                return Error<PageResponse<WorkflowDefinitionRes>>(ex.Message);
            }
        }

        [HttpGet("detail")]
        public async Task<ApiResponse<WorkflowDefinitionRes>> GetDetail([FromQuery] long id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                return result == null ? ApiResponse<WorkflowDefinitionRes>.Error("工作流不存在", 404) : ApiResponse<WorkflowDefinitionRes>.Success(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取工作流详情失败");
                return ApiResponse<WorkflowDefinitionRes>.Error(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ApiResponse<long>> Create([FromBody] WorkflowDefinitionReq req)
        {
            try
            {
                var id = await _service.CreateAsync(req);
                return Success(id, "创建成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "创建工作流失败");
                return Error<long>(ex.Message);
            }
        }

        [HttpPost("update")]
        public async Task<ApiResponse<bool>> Update([FromBody] WorkflowDefinitionReq req)
        {
            try
            {
                var result = await _service.UpdateAsync(req);
                return Success(result, "更新成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "更新工作流失败");
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
                _logger.LogError(ex, "删除工作流失败");
                return Error<bool>(ex.Message);
            }
        }

        [HttpPost("publish")]
        public async Task<ApiResponse<bool>> Publish([FromQuery] long id)
        {
            try
            {
                var result = await _service.PublishAsync(id);
                return Success(result, "发布成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "发布工作流失败");
                return Error<bool>(ex.Message);
            }
        }
    }
}
