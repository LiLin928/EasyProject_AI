using CommonManager.Base;
using EasyWechatModels.Dto;
using EasyWechatModels.Entitys;
using Mapster;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessManager.Business.SrmManager.Service
{
    public class PurchaseOrderService : BaseService<SrmPurchaseOrder>, BusinessManager.Business.SrmManager.IService.IPurchaseOrderService
    {
        public PurchaseOrderService(ISqlSugarClient db) : base(db) { }

        public async Task<PageResponse<SrmPurchaseOrderRes>> GetPageDataAsync(int pageIndex, int pageSize, int? status = null, string keyword = null)
        {
            var query = _db.Queryable<SrmPurchaseOrder>()
                .WhereIF(status.HasValue, o => o.Status == status.Value)
                .WhereIF(!string.IsNullOrEmpty(keyword), o => o.OrderNo.Contains(keyword) || o.SupplierName.Contains(keyword))
                .OrderByDescending(o => o.CreateTime);

            var list = await query.ToPageListAsync(pageIndex, pageSize);
            
            var resList = new List<SrmPurchaseOrderRes>();
            foreach (var order in list)
            {
                var items = await _db.Queryable<SrmPurchaseOrderItem>()
                    .Where(i => i.OrderId == order.Id)
                    .ToListAsync();
                
                resList.Add(new SrmPurchaseOrderRes
                {
                    Id = order.Id,
                    OrderNo = order.OrderNo,
                    RequestId = order.RequestId,
                    SupplierId = order.SupplierId,
                    SupplierName = order.SupplierName,
                    Status = order.Status,
                    StatusText = GetStatusText(order.Status),
                    TotalAmount = order.TotalAmount,
                    PaidAmount = order.PaidAmount,
                    OrderDate = order.OrderDate,
                    DeliverDate = order.DeliverDate,
                    ReceiveDate = order.ReceiveDate,
                    Remark = order.Remark,
                    CreateTime = order.CreateTime,
                    Items = items.Adapt<List<SrmPurchaseOrderItemRes>>()
                });
            }

            return PageResponse<SrmPurchaseOrderRes>.Create(resList, list.Count, pageIndex, pageSize);
        }

        public async Task<SrmPurchaseOrderRes> GetByIdAsync(string id)
        {
            var order = await _db.Queryable<SrmPurchaseOrder>().Where(o => o.Id == id).FirstAsync();
            if (order == null) return null;

            var items = await _db.Queryable<SrmPurchaseOrderItem>()
                .Where(i => i.OrderId == order.Id)
                .ToListAsync();

            return new SrmPurchaseOrderRes
            {
                Id = order.Id,
                OrderNo = order.OrderNo,
                RequestId = order.RequestId,
                SupplierId = order.SupplierId,
                SupplierName = order.SupplierName,
                Status = order.Status,
                StatusText = GetStatusText(order.Status),
                TotalAmount = order.TotalAmount,
                PaidAmount = order.PaidAmount,
                OrderDate = order.OrderDate,
                DeliverDate = order.DeliverDate,
                ReceiveDate = order.ReceiveDate,
                Remark = order.Remark,
                CreateTime = order.CreateTime,
                Items = items.Adapt<List<SrmPurchaseOrderItemRes>>()
            };
        }

        public async Task<string> CreateAsync(string userId, SrmPurchaseOrderReq req)
        {
            _db.Ado.BeginTran();
            try
            {
                var order = new SrmPurchaseOrder
                {
                    OrderNo = "PO" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999),
                    RequestId = req.RequestId,
                    SupplierId = req.SupplierId,
                    Status = 1,
                    TotalAmount = 0,
                    OrderDate = DateTime.Now,
                    Remark = req.Remark,
                    CreateTime = DateTime.Now
                };

                var orderId = await _db.Insertable(order).ExecuteReturnIdentityAsync();

                decimal totalAmount = 0;
                foreach (var item in req.Items)
                {
                    var amount = (item.UnitPrice ?? 0) * (item.Quantity ?? 0);
                    totalAmount += amount;

                    var orderItem = new SrmPurchaseOrderItem
                    {
                        OrderId = orderId.ToString(),
                        GoodsId = item.GoodsId,
                        GoodsName = item.GoodsName,
                        Specification = item.Specification,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Amount = amount,
                        ReceivedQuantity = 0
                    };

                    await _db.Insertable(orderItem).ExecuteCommandAsync();
                }

                order.TotalAmount = totalAmount;
                await _db.Updateable(order)
                    .SetColumns(o => o.TotalAmount, totalAmount)
                    .Where(o => o.Id == orderId.ToString())
                    .ExecuteCommandAsync();

                _db.Ado.CommitTran();
                return orderId != null ? long.Parse(orderId) : 0;
            }
            catch
            {
                _db.Ado.RollbackTran();
                throw;
            }
        }

        public async Task<bool> UpdateAsync(SrmPurchaseOrderReq req)
        {
            var order = await _db.Queryable<SrmPurchaseOrder>().Where(o => o.Id == req.Id).FirstAsync();
            if (order == null || order.Status != 1) return false;

            order.SupplierId = req.SupplierId;
            order.Remark = req.Remark;
            return await _db.Updateable(order).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            _db.Ado.BeginTran();
            try
            {
                await _db.Deleteable<SrmPurchaseOrderItem>().Where(i => i.OrderId == id).ExecuteCommandAsync();
                var result = await _db.Deleteable<SrmPurchaseOrder>().Where(o => o.Id == id).ExecuteCommandHasChangeAsync();
                _db.Ado.CommitTran();
                return result;
            }
            catch
            {
                _db.Ado.RollbackTran();
                throw;
            }
        }

        public async Task<bool> ConfirmAsync(string id)
        {
            var order = await _db.Queryable<SrmPurchaseOrder>().Where(o => o.Id == id).FirstAsync();
            if (order == null) return false;
            order.Status = 2;
            return await _db.Updateable(order).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> ReceiveAsync(string id)
        {
            var order = await _db.Queryable<SrmPurchaseOrder>().Where(o => o.Id == id).FirstAsync();
            if (order == null) return false;
            order.Status = 4;
            order.ReceiveDate = DateTime.Now;
            return await _db.Updateable(order).ExecuteCommandHasChangeAsync();
        }

        private string GetStatusText(int? status)
        {
            return status switch
            {
                0 => "草稿",
                1 => "待确认",
                2 => "已确认",
                3 => "发货中",
                4 => "已完成",
                5 => "已取消",
                _ => "未知"
            };
        }
    }
}
