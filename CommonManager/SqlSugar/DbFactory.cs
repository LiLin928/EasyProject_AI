using SqlSugar;
using System;
using System.Collections.Generic;

namespace CommonManager.SqlSugar
{
    /// <summary>
    /// 数据库工厂
    /// </summary>
    public static class DbFactory
    {
        private static ISqlSugarClient _instance;
        private static readonly object LockObj = new object();

        /// <summary>
        /// 获取数据库实例（单例）
        /// </summary>
        public static ISqlSugarClient GetInstance(List<ConnectionConfig> configList, bool isWriteLog = false)
        {
            if (_instance == null)
            {
                lock (LockObj)
                {
                    if (_instance == null)
                    {
                        _instance = SqlSugarInit.CreateClient(configList, isWriteLog);
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// 创建新的数据库实例
        /// </summary>
        public static ISqlSugarClient CreateNew(List<ConnectionConfig> configList, bool isWriteLog = false)
        {
            return SqlSugarInit.CreateClient(configList, isWriteLog);
        }
    }
}
