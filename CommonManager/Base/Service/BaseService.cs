using CommonManager.Base.IService;
using EasyWechatModels.Other;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CommonManager.Base
{
    /// <summary>
    /// 基础服务（泛型仓储）
    /// 所有 BusinessManager 的 Service 都集成此类
    /// </summary>
    public class BaseService<T> : IBaseService<T> where T : class, new()
    {
        protected readonly ISqlSugarClient _db;

        public BaseService(ISqlSugarClient db)
        {
            _db = db;
        }

        #region 查询

        public virtual async Task<T> GetByIdAsync(object id)
        {
            return await _db.Queryable<T>().In(id).SingleAsync();
        }

        public virtual T GetById(object id)
        {
            return _db.Queryable<T>().In(id).Single();
        }

        public virtual async Task<List<T>> GetListAsync()
        {
            return await _db.Queryable<T>().ToListAsync();
        }

        public virtual List<T> GetList()
        {
            return _db.Queryable<T>().ToList();
        }

        public virtual async Task<List<T>> GetQueryListAsync(Expression<Func<T, bool>> whereExpression)
        {
            return await _db.Queryable<T>().WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }

        public virtual async Task<T> GetSingleByFilterAsync(Expression<Func<T, bool>> whereExpression)
        {
            return await _db.Queryable<T>().FirstAsync(whereExpression);
        }

        public virtual async Task<bool> GetAnyByFilterAsync(Expression<Func<T, bool>> whereExpression)
        {
            return await _db.Queryable<T>().AnyAsync(whereExpression);
        }

        #endregion

        #region 分页

        public virtual async Task<PageResponse<T>> GetPageDataAsync(Expression<Func<T, bool>> whereExpression, int pageIndex, int pageSize, Expression<Func<T, object>> orderBySelector = null, bool isAsc = false)
        {
            var query = _db.Queryable<T>().WhereIF(whereExpression != null, whereExpression);
            
            if (orderBySelector != null)
            {
                query = isAsc 
                    ? query.OrderBy(orderBySelector) 
                    : query.OrderByDescending(orderBySelector);
            }

            var list = await query.ToPageListAsync(pageIndex, pageSize);
            return PageResponse<T>.Create(list, list.Count, pageIndex, pageSize);
        }

        #endregion

        #region 新增

        public virtual async Task<int> AddAsync(T entity)
        {
            return await _db.Insertable(entity).ExecuteCommandAsync();
        }

        public virtual async Task<int> AddReturnIdentityAsync(T entity)
        {
            return await _db.Insertable(entity).ExecuteReturnIdentityAsync();
        }

        public virtual async Task<int> AddListAsync(List<T> entities)
        {
            return await _db.Insertable(entities.ToArray()).ExecuteCommandAsync();
        }

        #endregion

        #region 修改

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            return await _db.Updateable(entity).ExecuteCommandHasChangeAsync();
        }

        public virtual async Task<int> UpdateListAsync(List<T> entities)
        {
            return await _db.Updateable(entities).ExecuteCommandAsync();
        }

        #endregion

        #region 删除

        public virtual async Task<bool> DeleteByIdAsync(object id)
        {
            return await _db.Deleteable<T>(id).ExecuteCommandHasChangeAsync();
        }

        public virtual async Task<bool> DeleteByIdsAsync(List<object> ids)
        {
            return await _db.Deleteable<T>().In(ids).ExecuteCommandHasChangeAsync();
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            return await _db.Deleteable(entity).ExecuteCommandHasChangeAsync();
        }

        public virtual async Task<bool> DeleteListAsync(List<T> entities)
        {
            return await _db.Deleteable(entities).ExecuteCommandHasChangeAsync();
        }

        #endregion

        #region 事务

        public virtual void BeginTran()
        {
            _db.Ado.BeginTran();
        }

        public virtual void CommitTran()
        {
            _db.Ado.CommitTran();
        }

        public virtual void RollbackTran()
        {
            _db.Ado.RollbackTran();
        }

        #endregion

        #region SQL 执行

        public virtual async Task<List<T>> ExecuteSqlAsync(string sql)
        {
            return await _db.SqlQueryable<T>(sql).ToListAsync();
        }

        public virtual List<T> ExecuteSql(string sql)
        {
            return _db.SqlQueryable<T>(sql).ToList();
        }

        #endregion
    }
}
