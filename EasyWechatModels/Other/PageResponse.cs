using System;
using System.Collections.Generic;

namespace EasyWechatModels.Other
{
    /// <summary>
    /// 分页响应
    /// </summary>
    public class PageResponse<T>
    {
        public List<T> List { get; set; } = new();
        public int Total { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling(Total / (double)PageSize);

        public static PageResponse<T> Create(List<T> list, int total, int pageIndex = 1, int pageSize = 10)
        {
            return new PageResponse<T>
            {
                List = list,
                Total = total,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }
    }

    /// <summary>
    /// 分页请求
    /// </summary>
    public class PageRequest
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
