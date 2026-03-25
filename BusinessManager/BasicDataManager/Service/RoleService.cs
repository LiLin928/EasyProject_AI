using BusinessManager.BasicDataManager.IService;
using CommonManager.Base;
using EasyWechatModels.Dto;
using EasyWechatModels.Entitys;
using Mapster;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessManager.BasicDataManager.Service
{
    /// <summary>
    /// 角色服务实现（集成 BaseService）
    /// </summary>
    public class RoleService : BaseService<BaseRole>, IRoleService
    {
        public RoleService(ISqlSugarClient db) : base(db)
        {
        }

        public async Task<PageResponse<BaseRoleRes>> GetPageDataAsync(int pageIndex, int pageSize, string keyword = null)
        {
            var query = _db.Queryable<BaseRole>()
                .WhereIF(!string.IsNullOrEmpty(keyword), r => r.RoleName.Contains(keyword) || r.RoleCode.Contains(keyword))
                .OrderByDescending(r => r.CreateTime);

            var list = await query.ToPageListAsync(pageIndex, pageSize);
            var totalCount = list.Count;

            return PageResponse<BaseRoleRes>.Create(list.Adapt<List<BaseRoleRes>>(), totalCount, pageIndex, pageSize);
        }

        public async Task<BaseRoleRes> GetByIdAsync(long id)
        {
            var role = await _db.Queryable<BaseRole>().Where(r => r.Id == id).FirstAsync();
            return role?.Adapt<BaseRoleRes>();
        }

        public async Task<long> CreateAsync(BaseRoleReq req)
        {
            var entity = req.Adapt<BaseRole>();
            entity.CreateTime = DateTime.Now;
            return await _db.Insertable(entity).ExecuteReturnIdentityAsync();
        }

        public async Task<bool> UpdateAsync(BaseRoleReq req)
        {
            var entity = req.Adapt<BaseRole>();
            entity.UpdateTime = DateTime.Now;
            return await _db.Updateable(entity).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _db.Deleteable<BaseRole>().Where(r => r.Id == id).ExecuteCommandHasChangeAsync();
        }
    }
}
