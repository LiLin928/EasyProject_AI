using CommonManager.Base;
using EasyWechatModels.Dto;
using EasyWechatModels.Entitys;
using System.Threading.Tasks;

namespace BusinessManager.Business.SrmManager.IService
{
    public interface IPurchaseOrderService
    {
        Task<PageResponse<SrmPurchaseOrderRes>> GetPageDataAsync(int pageIndex, int pageSize, int? status = null, string keyword = null);
        Task<SrmPurchaseOrderRes> GetByIdAsync(long id);
        Task<long> CreateAsync(long userId, SrmPurchaseOrderReq req);
        Task<bool> UpdateAsync(SrmPurchaseOrderReq req);
        Task<bool> DeleteAsync(long id);
        Task<bool> ConfirmAsync(long id);
        Task<bool> ReceiveAsync(long id);
    }
}
