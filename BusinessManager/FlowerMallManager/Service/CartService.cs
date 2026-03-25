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
    public class CartService : BaseService<MallCart>, ICartService
    {
        public CartService(ISqlSugarClient db) : base(db) { }

        public async Task<List<MallCartRes>> GetListAsync(string userId)
        {
            var list = await _db.Queryable<MallCart>()
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return list.Adapt<List<MallCartRes>>();
        }

        public async Task<string> AddAsync(string userId, MallCartReq req)
        {
            var cart = await _db.Queryable<MallCart>()
                .Where(c => c.UserId == userId && c.GoodsId == req.GoodsId.ToString())
                .FirstAsync();

            if (cart != null)
            {
                cart.Quantity = (cart.Quantity ?? 0) + req.Count;
                cart.UpdateTime = DateTime.Now;
                await _db.Updateable(cart).ExecuteCommandAsync();
                return cart.Id;
            }
            else
            {
                var entity = new MallCart
                {
                    Id = Guid.NewGuid().ToString("N"),
                    UserId = userId,
                    GoodsId = req.GoodsId.ToString(),
                    Quantity = req.Count,
                    CreateTime = DateTime.Now
                };
                await _db.Insertable(entity).ExecuteCommandAsync();
                return entity.Id;
            }
        }

        public async Task<bool> DeleteAsync(string userId, string goodsId)
        {
            return await _db.Deleteable<MallCart>()
                .Where(c => c.UserId == userId && c.GoodsId == goodsId)
                .ExecuteCommandHasChangeAsync();
        }
    }
}
