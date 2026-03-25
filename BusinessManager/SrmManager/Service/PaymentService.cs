using BusinessManager.SrmManager.IService;
using CommonManager.Base;
using EasyWechatModels.Dto;
using EasyWechatModels.Entitys;
using Mapster;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessManager.SrmManager.Service
{
    public class PaymentService : BaseService<SrmPaymentRequest>, IPaymentService
    {
        public PaymentService(ISqlSugarClient db) : base(db) { }

        public async Task<PageResponse<SrmPaymentRequestRes>> GetPageDataAsync(int pageIndex, int pageSize, int? status = null, string keyword = null)
        {
            var query = _db.Queryable<SrmPaymentRequest>()
                .WhereIF(status.HasValue, p => p.Status == status.Value)
                .WhereIF(!string.IsNullOrEmpty(keyword), p => p.RequestNo.Contains(keyword) || p.SupplierName.Contains(keyword))
                .OrderByDescending(p => p.CreateTime);

            var list = await query.ToPageListAsync(pageIndex, pageSize);
            return PageResponse<SrmPaymentRequestRes>.Create(list.Adapt<List<SrmPaymentRequestRes>>(), list.Count, pageIndex, pageSize);
        }

        public async Task<SrmPaymentRequestRes> GetByIdAsync(long id)
        {
            var payment = await _db.Queryable<SrmPaymentRequest>().Where(p => p.Id == id).FirstAsync();
            return payment?.Adapt<SrmPaymentRequestRes>();
        }

        public async Task<long> CreateAsync(long userId, SrmPaymentRequestReq req)
        {
            var entity = req.Adapt<SrmPaymentRequest>();
            entity.RequestNo = "PAY" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999);
            entity.Status = 1;
            entity.RequestDate = DateTime.Now;
            entity.CreateTime = DateTime.Now;
            return await _db.Insertable(entity).ExecuteReturnIdentityAsync();
        }

        public async Task<bool> ApproveAsync(long id, bool approved)
        {
            var payment = await _db.Queryable<SrmPaymentRequest>().Where(p => p.Id == id).FirstAsync();
            if (payment == null || payment.Status != 1) return false;
            
            payment.Status = approved ? 2 : 4;
            if (approved) payment.ApprovedDate = DateTime.Now;
            return await _db.Updateable(payment).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> PayAsync(long id)
        {
            var payment = await _db.Queryable<SrmPaymentRequest>().Where(p => p.Id == id).FirstAsync();
            if (payment == null) return false;
            payment.Status = 3;
            payment.PaymentDate = DateTime.Now;
            return await _db.Updateable(payment).ExecuteCommandHasChangeAsync();
        }
    }
}
