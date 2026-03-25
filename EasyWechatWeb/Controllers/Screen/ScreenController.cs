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

namespace EasyWechatWeb.Controllers.Screen
{
    [ApiController]
    [Route("api/Screen/[controller]")]
    [Authorize]
    public class ScreenController : BaseController
    {
        private readonly ISqlSugarClient _db;
        private readonly ILogger<ScreenController> _logger;

        public ScreenController(ISqlSugarClient db, ILogger<ScreenController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet("list")]
        public async Task<ApiResponse<PageResponse<ScreenProjectRes>>> GetList(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string keyword = null)
        {
            try
            {
                var query = _db.Queryable<ScreenProject>()
                    .WhereIF(!string.IsNullOrEmpty(keyword), s => s.ProjectName.Contains(keyword))
                    .OrderByDescending(s => s.CreateTime);

                var list = await query.ToPageListAsync(pageIndex, pageSize);
                return Success(PageResponse<ScreenProjectRes>.Create(list.Adapt<List<ScreenProjectRes>>(), list.Count, pageIndex, pageSize));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取大屏项目列表失败");
                return Error<PageResponse<ScreenProjectRes>>(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ApiResponse<long>> Create([FromBody] ScreenProject req)
        {
            try
            {
                req.CreateTime = DateTime.Now;
                var id = await _db.Insertable(req).ExecuteReturnIdentityAsync();
                return Success<long>(id, "创建成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "创建大屏项目失败");
                return Error<long>(ex.Message);
            }
        }
    }
}
