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
    /// <summary>
    /// 购物车服务实现（集成 BaseService）
    /// </summary>
    public class CartService : BaseService<MallCart>, ICartService
    {
        public CartService(ISqlSugarClient db) : base(db)
        {
        }

        public async Task<List<MallCartRes>> GetListAsync(long userId)
        {
            var cartList = await _db.Queryable<MallCart>()
                .Where(c => c.UserId == userId)
                .LeftJoin<MallGoods>((c, g) => c.GoodsId == g.Id)
                .Select<MallCartRes>((c, g) => new MallCartRes
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    GoodsId = c.GoodsId,
                    Count = c.Count,
                    Goods = new MallGoodsRes
                    {
                        Id = g.Id,
                        Name = g.Name,
                        Price = g.Price,
                        Image = g.Image
                    }
                })
                .ToListAsync();

            return cartList;
        }

        public async Task<long> AddAsync(long userId, MallCartReq req)
        {
            var cart = await _db.Queryable<MallCart>()
                .Where(c => c.UserId == userId && c.GoodsId == req.GoodsId)
                .FirstAsync();

            if (cart != null)
            {
                cart.Count += req.Count;
                cart.UpdateTime = DateTime.Now;
                await _db.Updateable(cart).ExecuteCommandAsync();
                return cart.Id;
            }
            else
            {
                cart = new MallCart
                {
                    UserId = userId,
                    GoodsId = req.GoodsId,
                    Count = req.Count,
                    CreateTime = DateTime.Now
                };
                return await _db.Insertable(cart).ExecuteReturnIdentityAsync();
            }
        }

        public async Task<bool> UpdateAsync(MallCartReq req)
        {
            return await _db.Updateable<MallCart>()
                .SetColumns(c => c.Count == req.Count)
                .Where(c => c.Id == req.Id)
                .ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> DeleteAsync(string userId, string goodsId)
        {
            return await _db.Deleteable<MallCart>()
                .Where(c => c.UserId == userId && c.GoodsId == goodsId)
                .ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> ClearAsync(long userId)
        {
            return await _db.Deleteable<MallCart>()
                .Where(c => c.UserId == userId)
                .ExecuteCommandHasChangeAsync();
        }
    }
}
