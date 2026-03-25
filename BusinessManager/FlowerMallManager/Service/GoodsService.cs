using BusinessManager.FlowerMallManager.IService;
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
        public GoodsService(ISqlSugarClient db) : base(db) { }

        public async Task<PageResponse<MallGoodsRes>> GetPageDataAsync(int pageIndex, int pageSize, int? categoryId = null, string keyword = null)
        {
            var query = _db.Queryable<MallGoods>()
                .WhereIF(categoryId.HasValue, g => g.CategoryId == categoryId.ToString())
                .WhereIF(!string.IsNullOrEmpty(keyword), g => g.Name.Contains(keyword))
                .OrderByDescending(g => g.Sort);

            var list = await query.ToPageListAsync(pageIndex, pageSize);
            return PageResponse<MallGoodsRes>.Create(list.Adapt<List<MallGoodsRes>>(), list.Count, pageIndex, pageSize);
        }

        public async Task<MallGoodsRes> GetByIdAsync(string id)
        {
            var goods = await _db.Queryable<MallGoods>().Where(g => g.Id == id).FirstAsync();
            return goods?.Adapt<MallGoodsRes>();
        }

        public async Task<string> CreateAsync(MallGoodsReq req)
        {
            var entity = req.Adapt<MallGoods>();
            entity.Id = Guid.NewGuid().ToString("N");
            entity.CreateTime = DateTime.Now;
            await _db.Insertable(entity).ExecuteCommandAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(MallGoodsReq req)
        {
            var entity = req.Adapt<MallGoods>();
            return await _db.Updateable(entity).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _db.Deleteable<MallGoods>().Where(g => g.Id == id).ExecuteCommandHasChangeAsync();
        }
    }
}
