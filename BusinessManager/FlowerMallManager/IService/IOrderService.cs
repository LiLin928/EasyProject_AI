using CommonManager.Base;
using EasyWechatModels.Dto;
using System.Threading.Tasks;

namespace BusinessManager.FlowerMallManager.IService
{
    public interface IOrderService
    {
        Task<PageResponse<MallOrderRes>> GetPageDataAsync(long userId, int pageIndex, int pageSize, int? status = null);
        Task<MallOrderRes> GetByIdAsync(long id);
        Task<long> CreateAsync(long userId, MallOrderReq req);
        Task<bool> PayAsync(long userId, long orderId);
        Task<bool> CancelAsync(long userId, long orderId);
    }
}
