using CommonManager.Base;
using EasyWechatModels.Dto;
using System.Threading.Tasks;

namespace BusinessManager.FlowerMallManager.IService
{
    public interface IGoodsService
    {
        Task<PageResponse<MallGoodsRes>> GetPageDataAsync(int pageIndex, int pageSize, int? categoryId = null, string keyword = null);
        Task<MallGoodsRes> GetByIdAsync(string id);
        Task<string> CreateAsync(MallGoodsReq req);
        Task<bool> UpdateAsync(MallGoodsReq req);
        Task<bool> DeleteAsync(string id);
    }
}
