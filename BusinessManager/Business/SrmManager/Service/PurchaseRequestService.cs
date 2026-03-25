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
    public class PurchaseRequestService : BaseService<SrmPurchaseRequest>, BusinessManager.Business.SrmManager.IService.IPurchaseRequestService
    {
        public PurchaseRequestService(ISqlSugarClient db) : base(db) { }

        public async Task<PageResponse<SrmPurchaseRequestRes>> GetPageDataAsync(int pageIndex, int pageSize, int? status = null, string keyword = null)
        {
            var query = _db.Queryable<SrmPurchaseRequest>()
                .WhereIF(status.HasValue, r => r.Status == status.Value)
                .WhereIF(!string.IsNullOrEmpty(keyword), r => r.RequestNo.Contains(keyword) || r.ApplicantName.Contains(keyword))
                .OrderByDescending(r => r.CreateTime);

            var list = await query.ToPageListAsync(pageIndex, pageSize);
            
            var resList = new List<SrmPurchaseRequestRes>();
            foreach (var request in list)
            {
                var items = await _db.Queryable<SrmPurchaseRequestItem>()
                    .Where(i => i.RequestId == request.Id)
                    .ToListAsync();
                
                resList.Add(new SrmPurchaseRequestRes
                {
                    Id = request.Id,
                    RequestNo = request.RequestNo,
                    ApplicantId = request.ApplicantId,
                    ApplicantName = request.ApplicantName,
                    DepartmentId = request.DepartmentId,
                    DepartmentName = request.DepartmentName,
                    Status = request.Status,
                    StatusText = GetStatusText(request.Status),
                    TotalAmount = request.TotalAmount,
                    Remark = request.Remark,
                    RequestDate = request.RequestDate,
                    ApprovedDate = request.ApprovedDate,
                    CreateTime = request.CreateTime,
                    Items = items.Adapt<List<SrmPurchaseRequestItemRes>>()
                });
            }

            return PageResponse<SrmPurchaseRequestRes>.Create(resList, list.Count, pageIndex, pageSize);
        }

        public async Task<SrmPurchaseRequestRes> GetByIdAsync(string id)
        {
            var request = await _db.Queryable<SrmPurchaseRequest>().Where(r => r.Id == id).FirstAsync();
            if (request == null) return null;

            var items = await _db.Queryable<SrmPurchaseRequestItem>()
                .Where(i => i.RequestId == request.Id)
                .ToListAsync();

            return new SrmPurchaseRequestRes
            {
                Id = request.Id,
                RequestNo = request.RequestNo,
                ApplicantId = request.ApplicantId,
                ApplicantName = request.ApplicantName,
                DepartmentId = request.DepartmentId,
                DepartmentName = request.DepartmentName,
                Status = request.Status,
                StatusText = GetStatusText(request.Status),
                TotalAmount = request.TotalAmount,
                Remark = request.Remark,
                RequestDate = request.RequestDate,
                ApprovedDate = request.ApprovedDate,
                CreateTime = request.CreateTime,
                Items = items.Adapt<List<SrmPurchaseRequestItemRes>>()
            };
        }

        public async Task<string> CreateAsync(string userId, SrmPurchaseRequestReq req)
        {
            _db.Ado.BeginTran();
            try
            {
                var request = new SrmPurchaseRequest
                {
                    RequestNo = "PR" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999),
                    ApplicantId = userId,
                    DepartmentId = req.DepartmentId,
                    Status = 1,
                    TotalAmount = 0,
                    Remark = req.Remark,
                    RequestDate = DateTime.Now,
                    CreateTime = DateTime.Now
                };

                var requestId = await _db.Insertable(request).ExecuteReturnIdentityAsync();
                if (requestId == null) requestId = Guid.NewGuid().ToString("N");

                decimal totalAmount = 0;
                foreach (var item in req.Items)
                {
                    var amount = (item.UnitPrice ?? 0) * (item.Quantity ?? 0);
                    totalAmount += amount;

                    var requestItem = new SrmPurchaseRequestItem
                    {
                        RequestId = requestId,
                        GoodsId = item.GoodsId,
                        GoodsName = item.GoodsName,
                        Specification = item.Specification,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Amount = amount,
                        Remark = item.Remark
                    };

                    await _db.Insertable(requestItem).ExecuteCommandAsync();
                }

                request.TotalAmount = totalAmount;
                await _db.Updateable(request)
                    .SetColumns(r => r.TotalAmount, totalAmount)
                    .Where(r => r.Id == requestId)
                    .ExecuteCommandAsync();

                _db.Ado.CommitTran();
                return requestId;
            }
            catch
            {
                _db.Ado.RollbackTran();
                throw;
            }
        }

        public async Task<bool> UpdateAsync(SrmPurchaseRequestReq req)
        {
            _db.Ado.BeginTran();
            try
            {
                var request = await _db.Queryable<SrmPurchaseRequest>().Where(r => r.Id == req.Id).FirstAsync();
                if (request == null || request.Status != 0) return false;

                request.DepartmentId = req.DepartmentId;
                request.Remark = req.Remark;
                await _db.Updateable(request).ExecuteCommandAsync();

                await _db.Deleteable<SrmPurchaseRequestItem>().Where(i => i.RequestId == req.Id).ExecuteCommandAsync();

                decimal totalAmount = 0;
                foreach (var item in req.Items)
                {
                    var amount = (item.UnitPrice ?? 0) * (item.Quantity ?? 0);
                    totalAmount += amount;

                    var requestItem = new SrmPurchaseRequestItem
                    {
                        RequestId = request.Id,
                        GoodsId = item.GoodsId,
                        GoodsName = item.GoodsName,
                        Specification = item.Specification,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Amount = amount,
                        Remark = item.Remark
                    };

                    await _db.Insertable(requestItem).ExecuteCommandAsync();
                }

                await _db.Updateable<SrmPurchaseRequest>()
                    .SetColumns(r => r.TotalAmount, totalAmount)
                    .Where(r => r.Id == req.Id)
                    .ExecuteCommandAsync();

                _db.Ado.CommitTran();
                return true;
            }
            catch
            {
                _db.Ado.RollbackTran();
                throw;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            _db.Ado.BeginTran();
            try
            {
                await _db.Deleteable<SrmPurchaseRequestItem>().Where(i => i.RequestId == id).ExecuteCommandAsync();
                var result = await _db.Deleteable<SrmPurchaseRequest>().Where(r => r.Id == id).ExecuteCommandHasChangeAsync();
                _db.Ado.CommitTran();
                return result;
            }
            catch
            {
                _db.Ado.RollbackTran();
                throw;
            }
        }

        public async Task<bool> ApproveAsync(string userId, string id, bool approved)
        {
            var request = await _db.Queryable<SrmPurchaseRequest>().Where(r => r.Id == id).FirstAsync();
            if (request == null || request.Status != 1) return false;

            request.Status = approved ? 2 : 3;
            request.ApprovedDate = DateTime.Now;
            request.ApproverId = userId;

            return await _db.Updateable(request).ExecuteCommandHasChangeAsync();
        }

        private string GetStatusText(int? status)
        {
            return status switch
            {
                0 => "草稿",
                1 => "待审批",
                2 => "已批准",
                3 => "已拒绝",
                4 => "已取消",
                _ => "未知"
            };
        }
    }
}
