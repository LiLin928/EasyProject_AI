using CommonManager.Base;
using EasyWechatModels.Dto;
using System.Threading.Tasks;

namespace BusinessManager.FlowerMallManager.IService
{
    public interface IAddressService
    {
        Task<PageResponse<MallAddressRes>> GetPageDataAsync(int pageIndex, int pageSize, string keyword = null);
        Task<MallAddressRes> GetByIdAsync(string id);
        Task<string> CreateAsync(MallAddressReq req);
        Task<bool> UpdateAsync(MallAddressReq req);
        Task<bool> DeleteAsync(string userId, string id);
    }
}
