using CommonManager.Base;
using EasyWechatModels.Dto;
using System.Threading.Tasks;

namespace BusinessManager.FlowerMallManager.IService
{
    public interface IOrderService
    {
        Task<PageResponse<MallOrderRes>> GetPageDataAsync(string userId, int pageIndex, int pageSize, int? status = null);
        Task<MallOrderRes> GetByIdAsync(string id);
        Task<string> CreateAsync(string userId, MallOrderReq req);
        Task<bool> UpdateAsync(MallOrderReq req);
        Task<bool> DeleteAsync(string id);
    }
}
