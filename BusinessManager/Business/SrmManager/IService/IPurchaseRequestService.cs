using CommonManager.Base;
using EasyWechatModels.Dto;
using EasyWechatModels.Entitys;
using System.Threading.Tasks;

namespace BusinessManager.Business.SrmManager.IService
{
    public interface IPurchaseRequestService
    {
        Task<PageResponse<SrmPurchaseRequestRes>> GetPageDataAsync(int pageIndex, int pageSize, int? status = null, string keyword = null);
        Task<SrmPurchaseRequestRes> GetByIdAsync(long id);
        Task<long> CreateAsync(long userId, SrmPurchaseRequestReq req);
        Task<bool> UpdateAsync(SrmPurchaseRequestReq req);
        Task<bool> DeleteAsync(long id);
        Task<bool> ApproveAsync(long userId, long id, bool approved);
    }
}
