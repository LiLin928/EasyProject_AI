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
    /// <summary>
    /// 订单服务实现（集成 BaseService）
    /// </summary>
    public class OrderService : BaseService<MallOrder>, IOrderService
    {
        public OrderService(ISqlSugarClient db) : base(db)
        {
        }

        public async Task<PageResponse<MallOrderRes>> GetPageDataAsync(long userId, int pageIndex, int pageSize, int? status = null)
        {
            var query = _db.Queryable<MallOrder>()
                .Where(o => o.UserId == userId)
                .WhereIF(status.HasValue, o => o.Status == status.Value)
                .OrderByDescending(o => o.CreateTime);

            var list = await query.ToPageListAsync(pageIndex, pageSize);
            
            var resList = new List<MallOrderRes>();
            foreach (var order in list)
            {
                var items = await _db.Queryable<MallOrderItem>()
                    .Where(i => i.OrderId == order.Id)
                    .ToListAsync();
                
                resList.Add(new MallOrderRes
                {
                    Id = order.Id,
                    OrderNo = order.OrderNo,
                    UserId = order.UserId,
                    TotalAmount = order.TotalAmount,
                    PayAmount = order.PayAmount,
                    Status = order.Status,
                    StatusText = GetStatusText(order.Status),
                    Remark = order.Remark,
                    CreateTime = order.CreateTime,
                    PayTime = order.PayTime,
                    Items = items.Adapt<List<MallOrderItemRes>>()
                });
            }

            return PageResponse<MallOrderRes>.Create(resList, list.Count, pageIndex, pageSize);
        }

        public async Task<MallOrderRes> GetByIdAsync(long id)
        {
            var order = await _db.Queryable<MallOrder>().Where(o => o.Id == id).FirstAsync();
            if (order == null) return null;

            var items = await _db.Queryable<MallOrderItem>()
                .Where(i => i.OrderId == order.Id)
                .ToListAsync();

            return new MallOrderRes
            {
                Id = order.Id,
                OrderNo = order.OrderNo,
                UserId = order.UserId,
                TotalAmount = order.TotalAmount,
                PayAmount = order.PayAmount,
                Status = order.Status,
                StatusText = GetStatusText(order.Status),
                Remark = order.Remark,
                CreateTime = order.CreateTime,
                PayTime = order.PayTime,
                Items = items.Adapt<List<MallOrderItemRes>>()
            };
        }

        public async Task<long> CreateAsync(long userId, MallOrderReq req)
        {
            _db.Ado.BeginTran();
            try
            {
                var order = new MallOrder
                {
                    OrderNo = GenerateOrderNo(),
                    UserId = userId,
                    TotalAmount = 0,
                    PayAmount = 0,
                    Status = 0,
                    Remark = req.Remark,
                    CreateTime = DateTime.Now
                };

                foreach (var item in req.Items)
                {
                    var goods = await _db.Queryable<MallGoods>()
                        .Where(g => g.Id == item.GoodsId)
                        .FirstAsync();
                    
                    if (goods != null)
                    {
                        order.TotalAmount += goods.Price * item.Quantity;
                        
                        var orderItem = new MallOrderItem
                        {
                            GoodsId = item.GoodsId,
                            GoodsName = goods.Name,
                            GoodsImage = goods.Image,
                            Price = goods.Price,
                            Quantity = item.Quantity
                        };
                        
                        await _db.Insertable(orderItem).ExecuteCommandAsync();
                    }
                }

                order.PayAmount = order.TotalAmount;
                var orderId = await _db.Insertable(order).ExecuteReturnIdentityAsync();

                _db.Ado.CommitTran();
                return orderId;
            }
            catch
            {
                _db.Ado.RollbackTran();
                throw;
            }
        }

        public async Task<bool> PayAsync(long userId, long orderId)
        {
            var order = await _db.Queryable<MallOrder>().Where(o => o.Id == orderId && o.UserId == userId).FirstAsync();
            if (order == null) return false;
            order.Status = 1;
            order.PayTime = DateTime.Now;
            return await _db.Updateable(order).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> CancelAsync(long userId, long orderId)
        {
            return await _db.Updateable<MallOrder>()
                .SetColumns(o => o.Status, -1)
                .Where(o => o.Id == orderId && o.UserId == userId && o.Status == 0)
                .ExecuteCommandHasChangeAsync();
        }

        private string GetStatusText(int status)
        {
            return status switch
            {
                -1 => "已取消",
                0 => "待付款",
                1 => "待发货",
                2 => "待收货",
                3 => "已完成",
                _ => "未知"
            };
        }

        private string GenerateOrderNo()
        {
            return "ORD" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999);
        }
    }
}
