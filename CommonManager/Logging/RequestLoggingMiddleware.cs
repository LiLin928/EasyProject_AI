using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CommonManager.Logging
{
    /// <summary>
    /// 请求日志中间件
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                Log.Information(
                    "[HTTP] {Method} {Path} - 开始处理",
                    context.Request.Method,
                    context.Request.Path
                );

                await _next(context);

                stopwatch.Stop();
                Log.Information(
                    "[HTTP] {Method} {Path} - 响应 {StatusCode} - 耗时 {ElapsedMs}ms",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds
                );
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Log.Error(
                    ex,
                    "[HTTP] {Method} {Path} - 异常 - 耗时 {ElapsedMs}ms",
                    context.Request.Method,
                    context.Request.Path,
                    stopwatch.ElapsedMilliseconds
                );
                throw;
            }
        }
    }
}
