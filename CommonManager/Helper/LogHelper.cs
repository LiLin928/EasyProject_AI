using Serilog;
using System;

namespace CommonManager.Helper
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// 信息日志
        /// </summary>
        public static void Info(string message)
        {
            Log.Information(message);
        }

        /// <summary>
        /// 信息日志（带参数）
        /// </summary>
        public static void Info(string message, params object[] args)
        {
            Log.Information(message, args);
        }

        /// <summary>
        /// 警告日志
        /// </summary>
        public static void Warning(string message)
        {
            Log.Warning(message);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        public static void Error(string message)
        {
            Log.Error(message);
        }

        /// <summary>
        /// 错误日志（带异常）
        /// </summary>
        public static void Error(Exception ex, string message)
        {
            Log.Error(ex, message);
        }

        /// <summary>
        /// 调试日志
        /// </summary>
        public static void Debug(string message)
        {
            Log.Debug(message);
        }

        /// <summary>
        /// MySQL 信息日志
        /// </summary>
        public static void MysqlInfo(string message)
        {
            Log.Information("[MySQL] {Message}", message);
        }

        /// <summary>
        /// MySQL 错误日志
        /// </summary>
        public static void MysqlError(string message)
        {
            Log.Error("[MySQL] {Message}", message);
        }

        /// <summary>
        /// MySQL 错误日志（带异常）
        /// </summary>
        public static void MysqlError(Exception ex, string message)
        {
            Log.Error(ex, "[MySQL] {Message}", message);
        }
    }
}
