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
        public AddressService(ISqlSugarClient db) : base(db)
        {
        }

        public async Task<List<MallAddressRes>> GetListAsync(long userId)
        {
            var list = await _db.Queryable<MallAddress>()
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.IsDefault)
                .OrderByDescending(a => a.CreateTime)
                .ToListAsync();

            return list.Adapt<List<MallAddressRes>>();
        }

        public async Task<MallAddressRes> GetByIdAsync(long id)
        {
            var address = await _db.Queryable<MallAddress>().Where(a => a.Id == id).FirstAsync();
            return address?.Adapt<MallAddressRes>();
        }

        public async Task<long> CreateAsync(long userId, MallAddressReq req)
        {
            if (req.IsDefault == 1)
            {
                await _db.Updateable<MallAddress>()
                    .SetColumns(a => a.IsDefault, 0)
                    .Where(a => a.UserId == userId)
                    .ExecuteCommandAsync();
            }

            var entity = req.Adapt<MallAddress>();
            entity.UserId = userId;
            entity.CreateTime = DateTime.Now;
            return await _db.Insertable(entity).ExecuteReturnIdentityAsync();
        }

        public async Task<bool> UpdateAsync(long userId, MallAddressReq req)
        {
            if (req.IsDefault == 1)
            {
                await _db.Updateable<MallAddress>()
                    .SetColumns(a => a.IsDefault, 0)
                    .Where(a => a.UserId == userId && a.Id != req.Id)
                    .ExecuteCommandAsync();
            }

            var entity = req.Adapt<MallAddress>();
            return await _db.Updateable(entity)
                .IgnoreColumns(a => a.UserId)
                .ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> DeleteAsync(string userId, string id)
        {
            return await _db.Deleteable<MallAddress>()
                .Where(a => a.Id == id && a.UserId == userId)
                .ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> SetDefaultAsync(long userId, long id)
        {
            _db.Ado.BeginTran();
            try
            {
                var all = await _db.Queryable<MallAddress>()
                    .Where(a => a.UserId == userId)
                    .ToListAsync();
                
                foreach (var addr in all)
                {
                    addr.IsDefault = 0;
                    await _db.Updateable(addr).IgnoreColumns(a => a.CreateTime).ExecuteCommandAsync();
                }

                var target = await _db.Queryable<MallAddress>()
                    .Where(a => a.Id == id && a.UserId == userId)
                    .FirstAsync();
                
                if (target == null)
                {
                    _db.Ado.RollbackTran();
                    return false;
                }
                
                target.IsDefault = 1;
                var result = await _db.Updateable(target).IgnoreColumns(a => a.CreateTime).ExecuteCommandHasChangeAsync();

                _db.Ado.CommitTran();
                return result;
            }
            catch
            {
                _db.Ado.RollbackTran();
                throw;
            }
        }
    }
}
