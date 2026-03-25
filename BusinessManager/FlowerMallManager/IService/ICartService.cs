using CommonManager.Base;
using EasyWechatModels.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessManager.FlowerMallManager.IService
{
    public interface ICartService
    {
        Task<List<MallCartRes>> GetListAsync(string userId);
        Task<string> AddAsync(string userId, MallCartReq req);
        Task<bool> DeleteAsync(string userId, string goodsId);
    }
}
