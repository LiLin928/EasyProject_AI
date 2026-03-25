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
    public class OrderService : BaseService<MallOrder>, IOrderService
    {
        public OrderService(ISqlSugarClient db) : base(db) { }

        public async Task<PageResponse<MallOrderRes>> GetPageDataAsync(string userId, int pageIndex, int pageSize, int? status = null)
        {
            var query = _db.Queryable<MallOrder>()
                .Where(o => o.UserId == userId)
                .WhereIF(status.HasValue, o => o.Status == status.Value)
                .OrderByDescending(o => o.CreateTime);

            var list = await query.ToPageListAsync(pageIndex, pageSize);
            return PageResponse<MallOrderRes>.Create(list.Adapt<List<MallOrderRes>>(), list.Count, pageIndex, pageSize);
        }

        public async Task<MallOrderRes> GetByIdAsync(string id)
        {
            var order = await _db.Queryable<MallOrder>().Where(o => o.Id == id).FirstAsync();
            return order?.Adapt<MallOrderRes>();
        }

        public async Task<string> CreateAsync(string userId, MallOrderReq req)
        {
            var entity = req.Adapt<MallOrder>();
            entity.Id = Guid.NewGuid().ToString("N");
            entity.UserId = userId;
            entity.CreateTime = DateTime.Now;
            await _db.Insertable(entity).ExecuteCommandAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(MallOrderReq req)
        {
            var entity = req.Adapt<MallOrder>();
            return await _db.Updateable(entity).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _db.Deleteable<MallOrder>().Where(o => o.Id == id).ExecuteCommandHasChangeAsync();
        }
    }
}
