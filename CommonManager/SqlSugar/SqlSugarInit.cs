using CommonManager.Helper;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace CommonManager.SqlSugar
{
    /// <summary>
    /// SqlSugar 初始化配置
    /// 注意配置 MasterSlaveConnectionStrings；IsWriteLog
    /// </summary>
    public static class SqlSugarInit
    {
        /// <summary>
        /// 添加 SqlSugar 服务
        /// </summary>
        public static ISqlSugarClient CreateClient(List<ConnectionConfig> configList, bool isWriteLog = false)
        {
            var client = new SqlSugarScope(configList);
            
            // SQL 执行前事件
            client.Aop.OnLogExecuting = (sql, pars) =>
            {
                if (isWriteLog)
                {
                    LogHelper.MysqlInfo($"SQL: {sql}");
                }
            };
            
            // SQL 执行后事件
            client.Aop.OnLogExecuted = (sql, pars) =>
            {
                if (isWriteLog)
                {
                    LogHelper.MysqlInfo($"耗时：{client.Ado.SqlExecutionTime.TotalMilliseconds}ms");
                }
            };
            
            // 错误事件
            client.Aop.OnError = (exp) =>
            {
                if (isWriteLog)
                {
                    LogHelper.MysqlError(exp, "SQL 执行错误");
                }
            };
            
            // 删除/更新强制 WHERE 条件检查
            client.Aop.OnExecutingChangeSql = (sql, pars) =>
            {
                if (sql.TrimStart().IndexOf("delete ", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (sql.IndexOf("where", StringComparison.OrdinalIgnoreCase) <= 0)
                    {
                        throw new Exception("delete 删除方法需要有 where 条件！！");
                    }
                }
                else if (sql.TrimStart().IndexOf("update ", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (sql.IndexOf("where", StringComparison.OrdinalIgnoreCase) <= 0)
                    {
                        throw new Exception("update 更新方法需要有 where 条件！！");
                    }
                }
                return new KeyValuePair<string, SugarParameter[]>(sql, pars);
            };
            
            return client;
        }
    }
}
