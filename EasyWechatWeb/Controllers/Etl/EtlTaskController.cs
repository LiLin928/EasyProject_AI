using CommonManager.Base;
using EasyWechatModels.Dto;
using EasyWechatModels.Entitys;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyWechatWeb.Controllers.Etl
{
    [ApiController]
    [Route("api/Etl/[controller]")]
    [Authorize]
    public class EtlTaskController : BaseController
    {
        private readonly ISqlSugarClient _db;
        private readonly ILogger<EtlTaskController> _logger;

        public EtlTaskController(ISqlSugarClient db, ILogger<EtlTaskController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet("list")]
        public async Task<ApiResponse<PageResponse<EtlTaskRes>>> GetList(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string keyword = null)
        {
            try
            {
                var query = _db.Queryable<EtlTask>()
                    .WhereIF(!string.IsNullOrEmpty(keyword), t => t.TaskName.Contains(keyword) || t.TaskCode.Contains(keyword))
                    .OrderByDescending(t => t.CreateTime);

                var list = await query.ToPageListAsync(pageIndex, pageSize);
                return Success(PageResponse<EtlTaskRes>.Create(list.Adapt<List<EtlTaskRes>>(), list.Count, pageIndex, pageSize));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取 ETL 任务列表失败");
                return Error<PageResponse<EtlTaskRes>>(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ApiResponse<long>> Create([FromBody] EtlTaskReq req)
        {
            try
            {
                var entity = req.Adapt<EtlTask>();
                entity.CreateTime = DateTime.Now;
                var id = await _db.Insertable(entity).ExecuteReturnIdentityAsync();
                return Success<long>(id, "创建成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "创建 ETL 任务失败");
                return Error<long>(ex.Message);
            }
        }

        [HttpPost("run")]
        public async Task<ApiResponse<bool>> Run([FromQuery] long id)
        {
            try
            {
                // TODO: 实现 ETL 任务执行逻辑
                var task = await _db.Queryable<EtlTask>().Where(t => t.Id == id).FirstAsync();
                if (task != null) {
                    task.LastRunTime = DateTime.Now;
                    task.LastRunStatus = 1;
                    await _db.Updateable(task).ExecuteCommandAsync();
                }
                return Success(true, "执行成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "执行 ETL 任务失败");
                return Error<bool>(ex.Message);
            }
        }
    }
}
