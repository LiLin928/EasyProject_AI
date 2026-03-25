using CommonManager.Base;
using EasyWechatModels.Dto;
using System.Threading.Tasks;

namespace BusinessManager.FlowerMallManager.IService
{
    public interface IGoodsService
    {
        // 基础 CRUD 方法已集成 BaseService
        Task<PageResponse<MallGoodsRes>> GetPageDataAsync(int pageIndex, int pageSize, int? categoryId = null, string keyword = null);
        Task<MallGoodsRes> GetByIdAsync(long id);
        Task<long> CreateAsync(MallGoodsReq req);
        Task<bool> UpdateAsync(MallGoodsReq req);
        // DeleteAsync 已集成 BaseService，这里软删除
        Task<bool> DeleteAsync(long id);
    }
}
