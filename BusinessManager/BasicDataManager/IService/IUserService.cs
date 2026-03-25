using CommonManager.Base;
using EasyWechatModels.Dto;
using EasyWechatModels.Entitys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessManager.BasicDataManager.IService
{
    /// <summary>
    /// 用户服务接口
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 获取用户列表
        /// </summary>
        Task<PageResponse<BaseUsersRes>> GetPageDataAsync(int pageIndex, int pageSize, string keyword = null);

        /// <summary>
        /// 根据 ID 获取用户
        /// </summary>
        Task<BaseUsersRes> GetByIdAsync(long id);

        /// <summary>
        /// 创建用户
        /// </summary>
        Task<long> CreateAsync(BaseUsersReq req);

        /// <summary>
        /// 更新用户
        /// </summary>
        Task<bool> UpdateAsync(BaseUsersReq req);

        /// <summary>
        /// 删除用户
        /// </summary>
        Task<bool> DeleteAsync(long id);

        /// <summary>
        /// 用户登录
        /// </summary>
        Task<BaseUsersRes> LoginAsync(string username, string password);

        /// <summary>
        /// 修改密码
        /// </summary>
        Task<bool> ChangePasswordAsync(long userId, string oldPassword, string newPassword);
    }
}
