using BusinessManager.FlowerMallManager.IService;
using CommonManager.Base;
using CommonManager.Base;
using EasyWechatModels.Dto;
using EasyWechatModels.Entitys;
using Mapster;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessManager.FlowerMallManager.Service
{
    public class GoodsService : BaseService<MallGoods>, IGoodsService
    {
        public GoodsService(ISqlSugarClient db) : base(db)
        {
        }

        public async Task<PageResponse<MallGoodsRes>> GetPageDataAsync(int pageIndex, int pageSize, int? categoryId = null, string keyword = null)
        {
            var query = _db.Queryable<MallGoods>()
                .Where(g => g.Status == 1)
                .WhereIF(categoryId.HasValue, g => g.CategoryId == categoryId.Value)
                .WhereIF(!string.IsNullOrEmpty(keyword), g => g.Name.Contains(keyword))
                .OrderByDescending(g => g.Sort)
                .OrderByDescending(g => g.CreateTime);

            var list = await query.ToPageListAsync(pageIndex, pageSize);
            return PageResponse<MallGoodsRes>.Create(list.Adapt<List<MallGoodsRes>>(), list.Count, pageIndex, pageSize);
        }

        public async Task<MallGoodsRes> GetByIdAsync(long id)
        {
            var goods = await _db.Queryable<MallGoods>().Where(g => g.Id == id).FirstAsync();
            return goods?.Adapt<MallGoodsRes>();
        }

        public async Task<long> CreateAsync(MallGoodsReq req)
        {
            var entity = req.Adapt<MallGoods>();
            entity.CreateTime = DateTime.Now;
            return await _db.Insertable(entity).ExecuteReturnIdentityAsync();
        }

        public async Task<bool> UpdateAsync(MallGoodsReq req)
        {
            var entity = req.Adapt<MallGoods>();
            entity.UpdateTime = DateTime.Now;
            return await _db.Updateable(entity).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _db.Updateable<MallGoods>()
                .SetColumns(g => g.Status == 0)
                .Where(g => g.Id == id)
                .ExecuteCommandHasChangeAsync();
        }
    }
}
