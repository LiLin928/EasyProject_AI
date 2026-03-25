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

namespace EasyWechatWeb.Controllers.Report
{
    [ApiController]
    [Route("api/Report/[controller]")]
    [Authorize]
    public class ReportController : BaseController
    {
        private readonly ISqlSugarClient _db;
        private readonly ILogger<ReportController> _logger;

        public ReportController(ISqlSugarClient db, ILogger<ReportController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet("list")]
        public async Task<ApiResponse<PageResponse<RptReportRes>>> GetList(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string keyword = null)
        {
            try
            {
                var query = _db.Queryable<RptReport>()
                    .WhereIF(!string.IsNullOrEmpty(keyword), r => r.ReportName.Contains(keyword))
                    .OrderByDescending(r => r.CreateTime);

                var list = await query.ToPageListAsync(pageIndex, pageSize);
                return Success(PageResponse<RptReportRes>.Create(list.Adapt<List<RptReportRes>>(), list.Count, pageIndex, pageSize));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取报表列表失败");
                return Error<PageResponse<RptReportRes>>(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ApiResponse<long>> Create([FromBody] RptReport req)
        {
            try
            {
                req.CreateTime = DateTime.Now;
                var id = await _db.Insertable(req).ExecuteReturnIdentityAsync();
                return Success<long>(id, "创建成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "创建报表失败");
                return Error<long>(ex.Message);
            }
        }

        [HttpPost("execute")]
        public async Task<ApiResponse<object>> Execute([FromQuery] long id)
        {
            try
            {
                var report = await _db.Queryable<RptReport>().Where(r => r.Id == id).FirstAsync();
                if (report == null) return Error<object>("报表不存在", 404);

                // TODO: 执行报表 SQL
                var data = await _db.SqlQueryable<object>(report.SqlContent).ToListAsync();
                return Success(data as object);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "执行报表失败");
                return Error<object>(ex.Message);
            }
        }
    }
}
