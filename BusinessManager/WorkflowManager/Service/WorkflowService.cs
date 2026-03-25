using CommonManager.Base;
using EasyWechatModels.Dto;
using EasyWechatModels.Entitys;
using Mapster;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessManager.WorkflowManager.Service
{
    public class WorkflowService : BaseService<WorkflowDefinition>, BusinessManager.WorkflowManager.IService.IWorkflowService
    {
        public WorkflowService(ISqlSugarClient db) : base(db) { }

        public async Task<PageResponse<WorkflowDefinitionRes>> GetPageDataAsync(int pageIndex, int pageSize, string keyword = null)
        {
            var query = _db.Queryable<WorkflowDefinition>()
                .WhereIF(!string.IsNullOrEmpty(keyword), w => w.Name.Contains(keyword) || w.Code.Contains(keyword))
                .OrderByDescending(w => w.CreateTime);

            var list = await query.ToPageListAsync(pageIndex, pageSize);
            return PageResponse<WorkflowDefinitionRes>.Create(list.Adapt<List<WorkflowDefinitionRes>>(), list.Count, pageIndex, pageSize);
        }

        public async Task<WorkflowDefinitionRes> GetByIdAsync(long id)
        {
            var workflow = await _db.Queryable<WorkflowDefinition>().Where(w => w.Id == id).FirstAsync();
            return workflow?.Adapt<WorkflowDefinitionRes>();
        }

        public async Task<long> CreateAsync(WorkflowDefinitionReq req)
        {
            var entity = req.Adapt<WorkflowDefinition>();
            entity.Status = 0;
            entity.CreateTime = DateTime.Now;
            return await _db.Insertable(entity).ExecuteReturnIdentityAsync();
        }

        public async Task<bool> UpdateAsync(WorkflowDefinitionReq req)
        {
            var entity = req.Adapt<WorkflowDefinition>();
            return await _db.Updateable(entity).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _db.Deleteable<WorkflowDefinition>().Where(w => w.Id == id).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> PublishAsync(long id)
        {
            return await _db.Updateable<WorkflowDefinition>()
                .SetColumns(w => w.Status, 1)
                .Where(w => w.Id == id)
                .ExecuteCommandHasChangeAsync();
        }
    }
}
