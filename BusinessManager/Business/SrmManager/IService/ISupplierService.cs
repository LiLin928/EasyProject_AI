using CommonManager.Base;
using EasyWechatModels.Dto;
using EasyWechatModels.Entitys;
using System.Threading.Tasks;

namespace BusinessManager.Business.SrmManager.IService
{
    public interface ISupplierService
    {
        Task<PageResponse<SrmSupplierRes>> GetPageDataAsync(int pageIndex, int pageSize, string keyword = null);
        Task<SrmSupplierRes> GetByIdAsync(long id);
        Task<long> CreateAsync(SrmSupplierReq req);
        Task<bool> UpdateAsync(SrmSupplierReq req);
        Task<bool> DeleteAsync(long id);
    }
}
