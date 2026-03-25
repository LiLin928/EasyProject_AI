using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CommonManager.Utility
{
    /// <summary>
    /// 自定义动作过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomerActionFilterAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 前置处理
            Console.WriteLine($"[Filter] Action 执行前：{context.ActionDescriptor.DisplayName}");

            var result = await next();

            // 后置处理
            Console.WriteLine($"[Filter] Action 执行后：{context.ActionDescriptor.DisplayName}");

            // 检查响应
            if (result.Exception != null)
            {
                Console.WriteLine($"[Filter] Action 异常：{result.Exception.Message}");
            }
        }
    }

    /// <summary>
    /// 授权检查过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthCheckFilterAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 检查用户是否登录
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new JsonResult(new
                {
                    code = 401,
                    message = "未授权访问"
                });
                return;
            }

            await next();
        }
    }

    /// <summary>
    /// 权限检查过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class PermissionCheckFilterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string _permission;

        public PermissionCheckFilterAttribute(string permission)
        {
            _permission = permission;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // TODO: 实现权限检查逻辑
            // if (!HasPermission(context.HttpContext.User, _permission))
            // {
            //     context.Result = new JsonResult(new
            //     {
            //         code = 403,
            //         message = "没有权限"
            //     });
            //     return;
            // }

            await next();
        }
    }
}
