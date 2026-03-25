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
    public class InvoiceService : BaseService<SrmInvoice>, BusinessManager.Business.SrmManager.IService.IInvoiceService
    {
        public InvoiceService(ISqlSugarClient db) : base(db) { }

        public async Task<PageResponse<SrmInvoiceRes>> GetPageDataAsync(int pageIndex, int pageSize, int? status = null, string keyword = null)
        {
            var query = _db.Queryable<SrmInvoice>()
                .WhereIF(status.HasValue, i => i.Status == status.Value)
                .WhereIF(!string.IsNullOrEmpty(keyword), i => i.InvoiceNo.Contains(keyword) || i.SupplierName.Contains(keyword))
                .OrderByDescending(i => i.CreateTime);

            var list = await query.ToPageListAsync(pageIndex, pageSize);
            return PageResponse<SrmInvoiceRes>.Create(list.Adapt<List<SrmInvoiceRes>>(), list.Count, pageIndex, pageSize);
        }

        public async Task<SrmInvoiceRes> GetByIdAsync(string id)
        {
            var invoice = await _db.Queryable<SrmInvoice>().Where(i => i.Id == id).FirstAsync();
            return invoice?.Adapt<SrmInvoiceRes>();
        }

        public async Task<string> CreateAsync(SrmInvoiceReq req)
        {
            var entity = req.Adapt<SrmInvoice>();
            entity.Id = Guid.NewGuid().ToString("N");
            entity.Status = 1;
            entity.CreateTime = DateTime.Now;
            await _db.Insertable(entity).ExecuteCommandAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(SrmInvoiceReq req)
        {
            var entity = req.Adapt<SrmInvoice>();
            return await _db.Updateable(entity).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _db.Deleteable<SrmInvoice>().Where(i => i.Id == id).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> AuditAsync(string id, bool approved)
        {
            return await _db.Updateable<SrmInvoice>()
                .SetColumns(i => i.Status, approved ? 2 : 3)
                .Where(i => i.Id == id)
                .ExecuteCommandHasChangeAsync();
        }
    }
}
