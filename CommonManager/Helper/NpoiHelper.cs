using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CommonManager.Helper
{
    /// <summary>
    /// NPOI Excel 帮助类
    /// </summary>
    public static class NpoiHelper
    {
        /// <summary>
        /// 导出 Excel
        /// </summary>
        public static string ExportExcel<T>(IEnumerable<T> data, string fileName, string sheetName = "Sheet1")
        {
            try
            {
                // TODO: 实现 NPOI Excel 导出逻辑
                // 需要添加 NPOI NuGet 包
                LogHelper.Info($"导出 Excel: {fileName}");
                
                var defaultPath = Path.Combine(Directory.GetCurrentDirectory(), "upload", "file");
                if (!Directory.Exists(defaultPath))
                {
                    Directory.CreateDirectory(defaultPath);
                }

                var filePath = Path.Combine(defaultPath, fileName + ".xlsx");
                
                // 创建空文件占位
                File.WriteAllText(filePath, "Excel content placeholder");
                
                return "upload/file/" + fileName + ".xlsx";
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "导出 Excel 失败");
                throw;
            }
        }

        /// <summary>
        /// 读取 Excel 到 DataTable
        /// </summary>
        public static DataTable ReadExcelToDataTable(string filePath, string sheetName = null, bool isFirstRowColumn = true)
        {
            try
            {
                // TODO: 实现 NPOI Excel 读取逻辑
                LogHelper.Info($"读取 Excel: {filePath}");
                return new DataTable();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "读取 Excel 失败");
                throw;
            }
        }

        /// <summary>
        /// 从流读取 Excel
        /// </summary>
        public static DataTable ReadStreamToDataTable(Stream stream, string sheetName = null, bool isFirstRowColumn = true)
        {
            try
            {
                // TODO: 实现 NPOI 流读取逻辑
                LogHelper.Info("从流读取 Excel");
                return new DataTable();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "从流读取 Excel 失败");
                throw;
            }
        }

        /// <summary>
        /// 获取对象属性
        /// </summary>
        public static List<KeyValuePair<string, object>> GetProperties<T>(T obj)
        {
            var result = new List<KeyValuePair<string, object>>();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (var prop in properties)
            {
                var value = prop.GetValue(obj, null);
                result.Add(new KeyValuePair<string, object>(prop.Name, value ?? string.Empty));
            }
            
            return result;
        }
    }
}
