using EasyWechatModels.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessManager.FlowerMallManager.IService
{
    public interface IAddressService
    {
        Task<List<MallAddressRes>> GetListAsync(long userId);
        Task<MallAddressRes> GetByIdAsync(long id);
        Task<long> CreateAsync(long userId, MallAddressReq req);
        Task<bool> UpdateAsync(long userId, MallAddressReq req);
        Task<bool> DeleteAsync(long userId, long id);
        Task<bool> SetDefaultAsync(long userId, long id);
    }
}
