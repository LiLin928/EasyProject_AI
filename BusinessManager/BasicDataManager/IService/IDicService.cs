using CommonManager.Base;
using EasyWechatModels.Dto;
using System.Threading.Tasks;

namespace BusinessManager.BasicDataManager.IService
{
    public interface IDicService
    {
        Task<PageResponse<BaseDicRes>> GetPageDataAsync(int pageIndex, int pageSize, string keyword = null);
        Task<BaseDicRes> GetByIdAsync(string id);
        Task<string> CreateAsync(BaseDicReq req);
        Task<bool> UpdateAsync(BaseDicReq req);
        Task<bool> DeleteAsync(string id);
    }
}
