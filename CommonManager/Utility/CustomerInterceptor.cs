using Castle.DynamicProxy;
using System;

namespace CommonManager.Utility
{
    /// <summary>
    /// 自定义拦截器（AOP）
    /// </summary>
    public class CustomerInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                // 前置处理
                Console.WriteLine($"[AOP] 调用方法：{invocation.Method.Name}");

                // 执行方法
                invocation.Proceed();

                // 后置处理
                Console.WriteLine($"[AOP] 方法执行完成：{invocation.Method.Name}");
            }
            catch (Exception ex)
            {
                // 异常处理
                Console.WriteLine($"[AOP] 方法执行异常：{invocation.Method.Name} - {ex.Message}");
                throw;
            }
        }
    }

    /// <summary>
    /// 日志拦截器
    /// </summary>
    public class LogInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var methodName = invocation.Method.Name;
            var startTime = DateTime.Now;

            try
            {
                Console.WriteLine($"[LOG] 开始执行：{methodName}");
                invocation.Proceed();
                Console.WriteLine($"[LOG] 执行完成：{methodName} - 耗时：{(DateTime.Now - startTime).TotalMilliseconds}ms");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LOG] 执行异常：{methodName} - {ex.Message}");
                throw;
            }
        }
    }

    /// <summary>
    /// 事务拦截器
    /// </summary>
    public class TransactionInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            // TODO: 实现事务逻辑
            // 这里需要集成 SqlSugar 的事务管理
            invocation.Proceed();
        }
    }
}
