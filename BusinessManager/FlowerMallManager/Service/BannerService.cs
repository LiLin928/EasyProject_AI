using BusinessManager.FlowerMallManager.IService;
using CommonManager.Base;
using EasyWechatModels.Dto;
using EasyWechatModels.Entitys;
using Mapster;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessManager.FlowerMallManager.Service
{
    /// <summary>
    /// 轮播图服务实现（集成 BaseService）
    /// </summary>
    public class BannerService : BaseService<MallBanner>, IBannerService
    {
        public BannerService(ISqlSugarClient db) : base(db)
        {
        }

        public async Task<List<MallBannerRes>> GetListAsync()
        {
            var list = await _db.Queryable<MallBanner>()
                .Where(b => b.Status == 1)
                .OrderBy(b => b.Sort)
                .ToListAsync();

            return list.Adapt<List<MallBannerRes>>();
        }
    }
}
