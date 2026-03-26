using CommonManager.Base;
using EasyWechatModels.Dto;
using EasyWechatModels.Entitys;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace EasyWechatWeb.Controllers.Basic
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MenuController : BaseController
    {
        private readonly ISqlSugarClient _db;
        private readonly ILogger<MenuController> _logger;

        public MenuController(ISqlSugarClient db, ILogger<MenuController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet("list")]
        public async Task<ApiResponse<List<BaseMenuRes>>> GetList()
        {
            try
            {
                var list = await _db.Queryable<BaseMenu>()
                    .OrderBy(m => m.Sort)
                    .ToListAsync();
                return Success(list.Adapt<List<BaseMenuRes>>());
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取菜单列表失败");
                return Error<List<BaseMenuRes>>(ex.Message);
            }
        }

        [HttpGet("tree")]
        public async Task<ApiResponse<List<object>>> GetTree()
        {
            try
            {
                var list = await _db.Queryable<BaseMenu>()
                    .OrderBy(m => m.Sort)
                    .ToListAsync();

                var tree = BuildTree(list, 0);
                return Success(tree);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取菜单树失败");
                return Error<List<object>>(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ApiResponse<long>> Create([FromBody] BaseMenuReq req)
        {
            try
            {
                var entity = req.Adapt<BaseMenu>();
                entity.CreateTime = DateTime.Now;
                var id = await _db.Insertable(entity).ExecuteReturnIdentityAsync();
                return Success<long>(id, "创建成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "创建菜单失败");
                return Error<long>(ex.Message);
            }
        }

        [HttpPost("update")]
        public async Task<ApiResponse<bool>> Update([FromBody] BaseMenuReq req)
        {
            try
            {
                var entity = req.Adapt<BaseMenu>();
                var result = await _db.Updateable(entity).ExecuteCommandHasChangeAsync();
                return Success(result, "更新成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "更新菜单失败");
                return Error<bool>(ex.Message);
            }
        }

        [HttpPost("delete")]
        public async Task<ApiResponse<bool>> Delete([FromQuery] long id)
        {
            try
            {
                var result = await _db.Deleteable<BaseMenu>().Where(m => m.Id == id).ExecuteCommandHasChangeAsync();
                return Success(result, "删除成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "删除菜单失败");
                return Error<bool>(ex.Message);
            }
        }

        private List<object> BuildTree(List<BaseMenu> list, long parentId)
        {
            var tree = new List<object>();
            var children = list.Where(m => m.ParentId == parentId);

            foreach (var item in children)
            {
                tree.Add(new
                {
                    Id = item.Id,
                    MenuName = item.MenuName,
                    MenuType = item.MenuType,
                    Path = item.Path,
                    Component = item.Component,
                    Icon = item.Icon,
                    Sort = item.Sort,
                    Children = BuildTree(list, item.Id)
                });
            }

            return tree;
        }
    }
}
