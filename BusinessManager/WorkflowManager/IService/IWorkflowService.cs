using CommonManager.Base;
using EasyWechatModels.Dto;
using System.Threading.Tasks;

namespace BusinessManager.WorkflowManager.IService
{
    public interface IWorkflowService
    {
        Task<PageResponse<WorkflowDefinitionRes>> GetPageDataAsync(int pageIndex, int pageSize, string keyword = null);
        Task<WorkflowDefinitionRes> GetByIdAsync(string id);
        Task<string> CreateAsync(WorkflowDefinitionReq req);
        Task<bool> UpdateAsync(WorkflowDefinitionReq req);
        Task<bool> DeleteAsync(string id);
        Task<bool> PublishAsync(string id);
    }
}
