using BusinessManager.BasicDataManager.IService;
using CommonManager.Base;
using CommonManager.Helper;
using EasyWechatModels.Dto;
using EasyWechatModels.Entitys;
using Mapster;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessManager.BasicDataManager.Service
{
    /// <summary>
    /// 用户服务实现（集成 BaseService）
    /// </summary>
    public class UserService : BaseService<BaseUsers>, IUserService
    {
        public UserService(ISqlSugarClient db) : base(db)
        {
        }

        public async Task<PageResponse<BaseUsersRes>> GetPageDataAsync(int pageIndex, int pageSize, string keyword = null)
        {
            var query = _db.Queryable<BaseUsers>()
                .WhereIF(!string.IsNullOrEmpty(keyword), u => u.Username.Contains(keyword) || u.Nickname.Contains(keyword) || u.Phone.Contains(keyword))
                .OrderByDescending(u => u.CreateTime);

            var list = await query.ToPageListAsync(pageIndex, pageSize);
            return PageResponse<BaseUsersRes>.Create(list.Adapt<List<BaseUsersRes>>(), list.Count, pageIndex, pageSize);
        }

        public async Task<BaseUsersRes> GetByIdAsync(string id)
        {
            var user = await _db.Queryable<BaseUsers>().Where(u => u.Id == id).FirstAsync();
            return user?.Adapt<BaseUsersRes>();
        }

        public async Task<string> CreateAsync(BaseUsersReq req)
        {
            var entity = req.Adapt<BaseUsers>();
            entity.Id = Guid.NewGuid().ToString("N");
            entity.Password = SecurityHelper.HashMD5(req.Password);
            entity.CreateTime = DateTime.Now;
            await _db.Insertable(entity).ExecuteCommandAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(BaseUsersReq req)
        {
            var entity = req.Adapt<BaseUsers>();
            entity.UpdateTime = DateTime.Now;

            if (!string.IsNullOrEmpty(req.Password))
            {
                entity.Password = SecurityHelper.HashMD5(req.Password);
                return await _db.Updateable(entity).ExecuteCommandHasChangeAsync();
            }
            else
            {
                return await _db.Updateable(entity)
                    .IgnoreColumns(u => u.Password)
                    .ExecuteCommandHasChangeAsync();
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _db.Deleteable<BaseUsers>().Where(u => u.Id == id).ExecuteCommandHasChangeAsync();
        }

        public async Task<BaseUsersRes> LoginAsync(string username, string password)
        {
            var encryptedPassword = SecurityHelper.HashMD5(password);

            var user = await _db.Queryable<BaseUsers>()
                .Where(u => u.Username == username && u.Password == encryptedPassword)
                .FirstAsync();

            if (user == null)
                throw new Exception("用户名或密码错误");

            if (user.Status != 1)
                throw new Exception("用户已被禁用");

            user.LastLoginTime = DateTime.Now;
            await _db.Updateable(user)
                .IgnoreColumns(true)
                .UpdateColumns(u => u.LastLoginTime)
                .ExecuteCommandAsync();

            return user.Adapt<BaseUsersRes>();
        }

        public async Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            var user = await GetByIdAsync(userId);
            if (user == null)
                throw new Exception("用户不存在");

            var encryptedOldPassword = SecurityHelper.HashMD5(oldPassword);
            var encryptedNewPassword = SecurityHelper.HashMD5(newPassword);

            var updateSuccess = await _db.Updateable<BaseUsers>()
                .SetColumns(u => u.Password, encryptedNewPassword)
                .Where(u => u.Id == userId && u.Password == encryptedOldPassword)
                .ExecuteCommandHasChangeAsync();

            if (!updateSuccess)
                throw new Exception("原密码错误");

            return true;
        }
    }
}
