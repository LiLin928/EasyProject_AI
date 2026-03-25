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
    public class SettlementService : BaseService<SrmSettlement>, BusinessManager.Business.SrmManager.IService.ISettlementService
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

        public async Task<SrmSettlementRes> GetByIdAsync(string id)
        {
            var settlement = await _db.Queryable<SrmSettlement>().Where(s => s.Id == id).FirstAsync();
            return settlement?.Adapt<SrmSettlementRes>();
        }

        public async Task<string> CreateAsync(SrmSettlement settlement)
        {
            settlement.Id = Guid.NewGuid().ToString("N");
            settlement.SettlementNo = "SET" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999);
            settlement.CreateTime = DateTime.Now;
            await _db.Insertable(settlement).ExecuteCommandAsync();
            return settlement.Id;
        }

        public async Task<bool> CompleteAsync(string id)
        {
            return await _db.Updateable<SrmSettlement>()
                .SetColumns(s => s.Status, 2)
                .Where(s => s.Id == id)
                .ExecuteCommandHasChangeAsync();
        }
    }
}
