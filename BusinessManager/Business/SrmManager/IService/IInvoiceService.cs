using CommonManager.Base;
using EasyWechatModels.Dto;
using EasyWechatModels.Entitys;
using System.Threading.Tasks;

namespace BusinessManager.Business.SrmManager.IService
{
    public interface IInvoiceService
    {
        Task<PageResponse<SrmInvoiceRes>> GetPageDataAsync(int pageIndex, int pageSize, int? status = null, string keyword = null);
        Task<SrmInvoiceRes> GetByIdAsync(string id);
        Task<string> CreateAsync(SrmInvoiceReq req);
        Task<bool> UpdateAsync(SrmInvoiceReq req);
        Task<bool> DeleteAsync(string id);
        Task<bool> AuditAsync(string id, bool approved);
    }
}
