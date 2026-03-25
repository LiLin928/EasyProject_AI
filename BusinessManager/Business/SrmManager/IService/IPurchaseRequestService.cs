using CommonManager.Base;
using EasyWechatModels.Dto;
using EasyWechatModels.Entitys;
using System.Threading.Tasks;

namespace BusinessManager.Business.SrmManager.IService
{
    public interface IPurchaseRequestService
    {
        Task<PageResponse<SrmPurchaseRequestRes>> GetPageDataAsync(int pageIndex, int pageSize, int? status = null, string keyword = null);
        Task<SrmPurchaseRequestRes> GetByIdAsync(string id);
        Task<string> CreateAsync(string userId, SrmPurchaseRequestReq req);
        Task<bool> UpdateAsync(SrmPurchaseRequestReq req);
        Task<bool> DeleteAsync(string id);
        Task<bool> ApproveAsync(string userId, string id, bool approved);
    }
}
