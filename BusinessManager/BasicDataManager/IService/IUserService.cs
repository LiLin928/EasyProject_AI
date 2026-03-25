using CommonManager.Base;
using EasyWechatModels.Dto;
using System.Threading.Tasks;

namespace BusinessManager.BasicDataManager.IService
{
    public interface IUserService
    {
        Task<PageResponse<BaseUsersRes>> GetPageDataAsync(int pageIndex, int pageSize, string keyword = null);
        Task<BaseUsersRes> GetByIdAsync(string id);
        Task<string> CreateAsync(BaseUsersReq req);
        Task<bool> UpdateAsync(BaseUsersReq req);
        Task<bool> DeleteAsync(string id);
        Task<BaseUsersRes> LoginAsync(string username, string password);
        Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
    }
}
