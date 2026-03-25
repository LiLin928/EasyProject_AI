using CommonManager.Base;
using System.Collections.Generic;

namespace CommonManager.Helper
{
    /// <summary>
    /// 统一结果帮助类
    /// </summary>
    public static class ResultHelper
    {
        /// <summary>
        /// 成功响应
        /// </summary>
        public static ApiResponse<T> Success<T>(T data, string message = "success")
        {
            return ApiResponse<T>.Success(data, message);
        }

        /// <summary>
        /// 成功响应（无数据）
        /// </summary>
        public static ApiResponse Success(string message = "success")
        {
            return ApiResponse.Success(message);
        }

        /// <summary>
        /// 错误响应
        /// </summary>
        public static ApiResponse<T> Error<T>(string message, int code = 500)
        {
            return ApiResponse<T>.Error(message, code);
        }

        /// <summary>
        /// 错误响应
        /// </summary>
        public static ApiResponse Error(string message, int code = 500)
        {
            return ApiResponse.Error(message, code);
        }

        /// <summary>
        /// 分页响应
        /// </summary>
        public static ApiResponse<PageResponse<T>> PageResult<T>(List<T> list, int total, int pageIndex = 1, int pageSize = 10)
        {
            var pageData = PageResponse<T>.Create(list, total, pageIndex, pageSize);
            return Success(pageData);
        }

        /// <summary>
        /// 未授权响应
        /// </summary>
        public static ApiResponse Unauthorized(string message = "未授权访问")
        {
            return ApiResponse.Error(message, 401);
        }

        /// <summary>
        /// 禁止访问响应
        /// </summary>
        public static ApiResponse Forbidden(string message = "禁止访问")
        {
            return ApiResponse.Error(message, 403);
        }

        /// <summary>
        /// 未找到响应
        /// </summary>
        public static ApiResponse NotFound(string message = "资源未找到")
        {
            return ApiResponse.Error(message, 404);
        }
    }
}
