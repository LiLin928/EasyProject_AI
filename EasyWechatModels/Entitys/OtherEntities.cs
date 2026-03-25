using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWechatModels.Entitys
{
    #region 报表管理

    [Table("rpt_report")]
    [SugarTable("rpt_report", "报表定义")]
    public class RptReport
    {
        [Key]
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "varchar")]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [SugarColumn(Length = 50, IsNullable = true)]
        public string ReportCode { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)]
        public string ReportName { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string ReportType { get; set; }

        [SugarColumn(Length = -1, IsNullable = true)]
        public string SqlContent { get; set; }

        [SugarColumn(Length = -1, IsNullable = true)]
        public string Columns { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? Status { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; } = DateTime.Now;
    }

    #endregion

    #region ETL 管理

    [Table("etl_task")]
    [SugarTable("etl_task", "ETL 任务")]
    public class EtlTask
    {
        [Key]
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "varchar")]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [SugarColumn(Length = 50, IsNullable = true)]
        public string TaskCode { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)]
        public string TaskName { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string SourceType { get; set; }

        [SugarColumn(Length = -1, IsNullable = true)]
        public string SourceConfig { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string TargetType { get; set; }

        [SugarColumn(Length = -1, IsNullable = true)]
        public string TargetConfig { get; set; }

        [SugarColumn(Length = -1, IsNullable = true)]
        public string TransformScript { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string Schedule { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? Status { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? LastRunTime { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? LastRunStatus { get; set; }

        [SugarColumn(Length = -1, IsNullable = true)]
        public string LastRunError { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; } = DateTime.Now;
    }

    [Table("etl_task_log")]
    [SugarTable("etl_task_log", "ETL 任务日志")]
    public class EtlTaskLog
    {
        [Key]
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "varchar")]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [SugarColumn(Length = 50, IsNullable = true)]
        public string TaskId { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? Status { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? RecordsProcessed { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? StartTime { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? EndTime { get; set; }

        [SugarColumn(Length = -1, IsNullable = true)]
        public string ErrorMessage { get; set; }
    }

    #endregion

    #region 大屏管理

    [Table("screen_project")]
    [SugarTable("screen_project", "大屏项目")]
    public class ScreenProject
    {
        [Key]
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "varchar")]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [SugarColumn(Length = 50, IsNullable = true)]
        public string ProjectCode { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)]
        public string ProjectName { get; set; }

        [SugarColumn(Length = -1, IsNullable = true)]
        public string LayoutConfig { get; set; }

        [SugarColumn(Length = -1, IsNullable = true)]
        public string ComponentConfig { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? Status { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; } = DateTime.Now;
    }

    #endregion

    #region 系统配置

    [Table("sys_config")]
    [SugarTable("sys_config", "系统配置")]
    public class SysConfig
    {
        [Key]
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "varchar")]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [SugarColumn(Length = 100, IsNullable = true)]
        public string ConfigKey { get; set; }

        [SugarColumn(Length = -1, IsNullable = true)]
        public string ConfigValue { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string ConfigType { get; set; }

        [SugarColumn(Length = 500, IsNullable = true)]
        public string Description { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? Status { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; } = DateTime.Now;
    }

    #endregion
}
