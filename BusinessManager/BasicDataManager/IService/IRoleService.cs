using CommonManager.Base;
using EasyWechatModels.Dto;
using System.Threading.Tasks;

namespace BusinessManager.BasicDataManager.IService
{
    /// <summary>
    /// 角色服务接口
    /// </summary>
    public interface IRoleService
    {
        Task<PageResponse<BaseRoleRes>> GetPageDataAsync(int pageIndex, int pageSize, string keyword = null);
        Task<BaseRoleRes> GetByIdAsync(long id);
        Task<long> CreateAsync(BaseRoleReq req);
        Task<bool> UpdateAsync(BaseRoleReq req);
        Task<bool> DeleteAsync(long id);
    }
}
