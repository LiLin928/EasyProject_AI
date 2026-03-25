using EasyWechatModels.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessManager.FlowerMallManager.IService
{
    public interface IBannerService
    {
        Task<List<MallBannerRes>> GetListAsync();
    }
}
