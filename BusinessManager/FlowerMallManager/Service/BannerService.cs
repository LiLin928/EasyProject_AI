using EasyWechatModels.Dto;
using EasyWechatModels.Entitys;
using Mapster;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessManager.FlowerMallManager.Service
{
    public class BannerService : BusinessManager.FlowerMallManager.IService.IBannerService
    {
        private readonly ISqlSugarClient _db;

        public BannerService(ISqlSugarClient db)
        {
            _db = db;
        }

        public async Task<List<MallBannerRes>> GetListAsync()
        {
            var list = await _db.Queryable<EasyWechatModels.Entitys.MallBanner>()
                .Where(b => b.Status == 1)
                .OrderBy(b => b.Sort)
                .ToListAsync();

            return list.Adapt<List<MallBannerRes>>();
        }
    }
}
