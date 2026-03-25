using CommonManager.Base;
using EasyWechatModels.Dto;
using EasyWechatModels.Entitys;
using System.Threading.Tasks;

namespace BusinessManager.Business.SrmManager.IService
{
    public interface IPaymentService
    {
        Task<PageResponse<SrmPaymentRequestRes>> GetPageDataAsync(int pageIndex, int pageSize, int? status = null, string keyword = null);
        Task<SrmPaymentRequestRes> GetByIdAsync(long id);
        Task<long> CreateAsync(long userId, SrmPaymentRequestReq req);
        Task<bool> ApproveAsync(long id, bool approved);
        Task<bool> PayAsync(long id);
    }
}
