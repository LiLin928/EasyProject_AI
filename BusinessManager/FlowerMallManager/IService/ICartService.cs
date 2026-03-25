using EasyWechatModels.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessManager.FlowerMallManager.IService
{
    public interface ICartService
    {
        Task<List<MallCartRes>> GetListAsync(long userId);
        Task<long> AddAsync(long userId, MallCartReq req);
        Task<bool> UpdateAsync(MallCartReq req);
        Task<bool> DeleteAsync(long userId, long goodsId);
        Task<bool> ClearAsync(long userId);
    }
}
