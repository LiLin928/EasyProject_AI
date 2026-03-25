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
    public class SupplierService : BaseService<SrmSupplier>, BusinessManager.Business.SrmManager.IService.ISupplierService
    {
        public SupplierService(ISqlSugarClient db) : base(db) { }

        public async Task<PageResponse<SrmSupplierRes>> GetPageDataAsync(int pageIndex, int pageSize, string keyword = null)
        {
            var query = _db.Queryable<SrmSupplier>()
                .WhereIF(!string.IsNullOrEmpty(keyword), s => s.SupplierName.Contains(keyword) || s.SupplierCode.Contains(keyword) || s.ContactPhone.Contains(keyword))
                .OrderByDescending(s => s.CreateTime);

            var list = await query.ToPageListAsync(pageIndex, pageSize);
            return PageResponse<SrmSupplierRes>.Create(list.Adapt<List<SrmSupplierRes>>(), list.Count, pageIndex, pageSize);
        }

        public async Task<SrmSupplierRes> GetByIdAsync(string id)
        {
            var supplier = await _db.Queryable<SrmSupplier>().Where(s => s.Id == id).FirstAsync();
            return supplier?.Adapt<SrmSupplierRes>();
        }

        public async Task<string> CreateAsync(SrmSupplierReq req)
        {
            var entity = req.Adapt<SrmSupplier>();
            entity.CreateTime = DateTime.Now;
            return await _db.Insertable(entity).ExecuteReturnIdentityAsync();
        }

        public async Task<bool> UpdateAsync(SrmSupplierReq req)
        {
            var entity = req.Adapt<SrmSupplier>();
            return await _db.Updateable(entity).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _db.Deleteable<SrmSupplier>().Where(s => s.Id == id).ExecuteCommandHasChangeAsync();
        }
    }
}
