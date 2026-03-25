using CommonManager.Base;
using EasyWechatModels.Dto;
using EasyWechatModels.Entitys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using EasyWechatModels.Entitys;

namespace EasyWechatWeb.Controllers.SystemConfig
{
    [ApiController]
    [Route("api/System/[controller]")]
    [Authorize]
    public class ConfigController : BaseController
    {
        private readonly ISqlSugarClient _db;
        private readonly ILogger<ConfigController> _logger;

        public ConfigController(ISqlSugarClient db, ILogger<ConfigController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet("list")]
        public async Task<ApiResponse<PageResponse<SysConfigRes>>> GetList(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string type = null,
            [FromQuery] string keyword = null)
        {
            try
            {
                var query = _db.Queryable<SysConfig>()
                    .WhereIF(!string.IsNullOrEmpty(type), c => c.ConfigType == type)
                    .WhereIF(!string.IsNullOrEmpty(keyword), c => c.ConfigKey.Contains(keyword) || c.Description.Contains(keyword))
                    .OrderByDescending(c => c.CreateTime);

                var list = await query.ToPageListAsync(pageIndex, pageSize);
                return Success(PageResponse<SysConfigRes>.Create(list.Adapt<List<SysConfigRes>>(), list.Count, pageIndex, pageSize));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取配置列表失败");
                return Error<PageResponse<SysConfigRes>>(ex.Message);
            }
        }

        [HttpGet("value")]
        public async Task<ApiResponse<string>> GetValue([FromQuery] string key)
        {
            try
            {
                var config = await _db.Queryable<SysConfig>().Where(c => c.ConfigKey == key).FirstAsync();
                return config != null ? Success<string>(config.ConfigValue) : Success<string>(string.Empty);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取配置值失败");
                return Error<string>(ex.Message);
            }
        }

        [HttpPost("save")]
        public async Task<ApiResponse<bool>> Save([FromBody] SysConfig req)
        {
            try
            {
                var config = await _db.Queryable<SysConfig>().Where(c => c.ConfigKey == req.ConfigKey).FirstAsync();
                if (config != null)
                {
                    config.ConfigValue = req.ConfigValue;
                    config.Description = req.Description;
                    return Success(await _db.Updateable(config).ExecuteCommandHasChangeAsync());
                }
                else
                {
                    req.CreateTime = DateTime.Now;
                    return Success(await _db.Insertable(req).ExecuteReturnIdentityAsync() > 0);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "保存配置失败");
                return Error<bool>(ex.Message);
            }
        }
    }
}
