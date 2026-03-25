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
    public class AddressService : BaseService<MallAddress>, IAddressService
    {
        public AddressService(ISqlSugarClient db) : base(db) { }

        public async Task<PageResponse<MallAddressRes>> GetPageDataAsync(int pageIndex, int pageSize, string keyword = null)
        {
            var query = _db.Queryable<MallAddress>()
                .WhereIF(!string.IsNullOrEmpty(keyword), a => a.Receiver.Contains(keyword) || a.Phone.Contains(keyword))
                .OrderByDescending(a => a.CreateTime);

            var list = await query.ToPageListAsync(pageIndex, pageSize);
            return PageResponse<MallAddressRes>.Create(list.Adapt<List<MallAddressRes>>(), list.Count, pageIndex, pageSize);
        }

        public async Task<MallAddressRes> GetByIdAsync(string id)
        {
            var address = await _db.Queryable<MallAddress>().Where(a => a.Id == id).FirstAsync();
            return address?.Adapt<MallAddressRes>();
        }

        public async Task<string> CreateAsync(MallAddressReq req)
        {
            var entity = req.Adapt<MallAddress>();
            entity.Id = Guid.NewGuid().ToString("N");
            entity.CreateTime = DateTime.Now;
            await _db.Insertable(entity).ExecuteCommandAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(MallAddressReq req)
        {
            var entity = req.Adapt<MallAddress>();
            return await _db.Updateable(entity).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> DeleteAsync(string userId, string id)
        {
            return await _db.Deleteable<MallAddress>()
                .Where(a => a.Id == id && a.UserId == userId)
                .ExecuteCommandHasChangeAsync();
        }
    }
}
