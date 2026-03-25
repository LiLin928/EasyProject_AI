using EasyWechatModels.Other;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CommonManager.Base.IService
{
    /// <summary>
    /// 基础服务接口
    /// </summary>
    public interface IBaseService<T> where T : class, new()
    {
        Task<T> GetByIdAsync(object id);
        T GetById(object id);
        Task<List<T>> GetListAsync();
        List<T> GetList();
        Task<List<T>> GetQueryListAsync(Expression<Func<T, bool>> whereExpression);
        Task<T> GetSingleByFilterAsync(Expression<Func<T, bool>> whereExpression);
        Task<bool> GetAnyByFilterAsync(Expression<Func<T, bool>> whereExpression);
        Task<PageResponse<T>> GetPageDataAsync(Expression<Func<T, bool>> whereExpression, int pageIndex, int pageSize, Expression<Func<T, object>> orderBySelector = null, bool isAsc = false);
        Task<int> AddAsync(T entity);
        Task<int> AddReturnIdentityAsync(T entity);
        Task<int> AddListAsync(List<T> entities);
        Task<bool> UpdateAsync(T entity);
        Task<int> UpdateListAsync(List<T> entities);
        Task<bool> DeleteByIdAsync(object id);
        Task<bool> DeleteByIdsAsync(List<object> ids);
        Task<bool> DeleteAsync(T entity);
        Task<bool> DeleteListAsync(List<T> entities);
        void BeginTran();
        void CommitTran();
        void RollbackTran();
        Task<List<T>> ExecuteSqlAsync(string sql);
        List<T> ExecuteSql(string sql);
    }
}
