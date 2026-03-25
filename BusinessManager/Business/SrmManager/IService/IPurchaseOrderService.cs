using CommonManager.Base;
using EasyWechatModels.Dto;
using EasyWechatModels.Entitys;
using System.Threading.Tasks;

namespace BusinessManager.Business.SrmManager.IService
{
    public interface IPurchaseOrderService
    {
        Task<PageResponse<SrmPurchaseOrderRes>> GetPageDataAsync(int pageIndex, int pageSize, int? status = null, string keyword = null);
        Task<SrmPurchaseOrderRes> GetByIdAsync(string id);
        Task<string> CreateAsync(string userId, SrmPurchaseOrderReq req);
        Task<bool> UpdateAsync(SrmPurchaseOrderReq req);
        Task<bool> DeleteAsync(string id);
        Task<bool> ConfirmAsync(string id);
        Task<bool> ReceiveAsync(string id);
    }
}
