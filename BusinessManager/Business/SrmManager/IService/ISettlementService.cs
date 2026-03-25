using CommonManager.Base;
using EasyWechatModels.Dto;
using EasyWechatModels.Entitys;
using System.Threading.Tasks;

namespace BusinessManager.Business.SrmManager.IService
{
    public interface ISettlementService
    {
        Task<PageResponse<SrmSettlementRes>> GetPageDataAsync(int pageIndex, int pageSize, int? status = null, string keyword = null);
        Task<SrmSettlementRes> GetByIdAsync(string id);
        Task<string> CreateAsync(SrmSettlement settlement);
        Task<bool> CompleteAsync(string id);
    }
}
