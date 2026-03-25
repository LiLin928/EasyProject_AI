using CommonManager.Base;
using EasyWechatModels.Dto;
using EasyWechatModels.Entitys;
using Mapster;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessManager.SrmManager.IService
{
    public interface ISettlementService
    {
        Task<PageResponse<SrmSettlementRes>> GetPageDataAsync(int pageIndex, int pageSize, int? status = null, string keyword = null);
        Task<SrmSettlementRes> GetByIdAsync(long id);
        Task<long> CreateAsync(SrmSettlement settlement);
        Task<bool> CompleteAsync(long id);
    }
}

namespace BusinessManager.SrmManager.Service
{
    public class SettlementService : BaseService<SrmSettlement>, BusinessManager.SrmManager.IService.ISettlementService
    {
        public SettlementService(ISqlSugarClient db) : base(db) { }

        public async Task<PageResponse<SrmSettlementRes>> GetPageDataAsync(int pageIndex, int pageSize, int? status = null, string keyword = null)
        {
            var query = _db.Queryable<SrmSettlement>()
                .WhereIF(status.HasValue, s => s.Status == status.Value)
                .WhereIF(!string.IsNullOrEmpty(keyword), s => s.SettlementNo.Contains(keyword) || s.SupplierName.Contains(keyword))
                .OrderByDescending(s => s.CreateTime);

            var list = await query.ToPageListAsync(pageIndex, pageSize);
            return PageResponse<SrmSettlementRes>.Create(list.Adapt<List<SrmSettlementRes>>(), list.Count, pageIndex, pageSize);
        }

        public async Task<SrmSettlementRes> GetByIdAsync(long id)
        {
            var settlement = await _db.Queryable<SrmSettlement>().Where(s => s.Id == id).FirstAsync();
            return settlement?.Adapt<SrmSettlementRes>();
        }

        public async Task<long> CreateAsync(SrmSettlement settlement)
        {
            settlement.SettlementNo = "SET" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999);
            settlement.CreateTime = DateTime.Now;
            return await _db.Insertable(settlement).ExecuteReturnIdentityAsync();
        }

        public async Task<bool> CompleteAsync(long id)
        {
            return await _db.Updateable<SrmSettlement>()
                .SetColumns(s => s.Status, 2)
                .Where(s => s.Id == id)
                .ExecuteCommandHasChangeAsync();
        }
    }
}
