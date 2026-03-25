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
    /// 字典服务实现（集成 BaseService）
    /// </summary>
    public class DicService : BaseService<BaseDic>, IDicService
    {
        public DicService(ISqlSugarClient db) : base(db)
        {
        }

        public async Task<PageResponse<BaseDicRes>> GetPageDataAsync(int pageIndex, int pageSize, string keyword = null)
        {
            var query = _db.Queryable<BaseDic>()
                .WhereIF(!string.IsNullOrEmpty(keyword), d => d.DicName.Contains(keyword) || d.DicCode.Contains(keyword))
                .OrderByDescending(d => d.CreateTime);

            var list = await query.ToPageListAsync(pageIndex, pageSize);
            return PageResponse<BaseDicRes>.Create(list.Adapt<List<BaseDicRes>>(), list.Count, pageIndex, pageSize);
        }

        public async Task<BaseDicRes> GetByIdAsync(long id)
        {
            var dic = await _db.Queryable<BaseDic>().Where(d => d.Id == id).FirstAsync();
            return dic?.Adapt<BaseDicRes>();
        }

        public async Task<long> CreateAsync(BaseDicReq req)
        {
            var entity = req.Adapt<BaseDic>();
            entity.CreateTime = DateTime.Now;
            return await _db.Insertable(entity).ExecuteReturnIdentityAsync();
        }

        public async Task<bool> UpdateAsync(BaseDicReq req)
        {
            var entity = req.Adapt<BaseDic>();
            return await _db.Updateable(entity).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _db.Deleteable<BaseDic>().Where(d => d.Id == id).ExecuteCommandHasChangeAsync();
        }
    }
}
