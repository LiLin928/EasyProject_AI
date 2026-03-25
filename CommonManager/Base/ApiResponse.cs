namespace CommonManager.Base
{
    /// <summary>
    /// 统一 API 响应格式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; } = 200;

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; } = "success";

        /// <summary>
        /// 数据
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// 成功响应
        /// </summary>
        public static ApiResponse<T> Success(T data, string message = "success")
        {
            return new ApiResponse<T>
            {
                Code = 200,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// 成功响应 (无数据)
        /// </summary>
        public static ApiResponse<T> Success(string message = "success")
        {
            return new ApiResponse<T>
            {
                Code = 200,
                Message = message,
                Data = default
            };
        }

        /// <summary>
        /// 错误响应
        /// </summary>
        public static ApiResponse<T> Error(string message, int code = 500)
        {
            return new ApiResponse<T>
            {
                Code = code,
                Message = message,
                Data = default
            };
        }
    }

    /// <summary>
    /// 非泛型响应
    /// </summary>
    public class ApiResponse
    {
        public int Code { get; set; } = 200;
        public string Message { get; set; } = "success";

        public static ApiResponse Success(string message = "success")
        {
            return new ApiResponse
            {
                Code = 200,
                Message = message
            };
        }

        public static ApiResponse Error(string message, int code = 500)
        {
            return new ApiResponse
            {
                Code = code,
                Message = message
            };
        }
    }

    /// <summary>
    /// 分页响应
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageResponse<T>
    {
        /// <summary>
        /// 数据列表
        /// </summary>
        public List<T> List { get; set; } = new();

        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages => (int)Math.Ceiling(Total / (double)PageSize);

        /// <summary>
        /// 创建分页响应
        /// </summary>
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
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
