using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWechatModels.Entitys
{
    /// <summary>
    /// 字典表
    /// </summary>
    [Table("base_dic")]
    [SugarTable("base_dic", "字典表")]
    public class BaseDic
    {
        /// <summary>
        /// 主键 ID (GUID)
        /// </summary>
        [Key]
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "varchar")]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// 字典名称
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string DicName { get; set; }

        /// <summary>
        /// 字典编码
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string DicCode { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [SugarColumn(Length = 500, IsNullable = true)]
        public string Description { get; set; }

        /// <summary>
        /// 状态 (0:禁用 1:正常)
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? Status { get; set; } = 1;

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 字典项表
    /// </summary>
    [Table("base_dic_item")]
    [SugarTable("base_dic_item", "字典项表")]
    public class BaseDicItem
    {
        /// <summary>
        /// 主键 ID (GUID)
        /// </summary>
        [Key]
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "varchar")]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// 字典 ID (GUID 外键)
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string DicId { get; set; }

        /// <summary>
        /// 字典项名称
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string ItemName { get; set; }

        /// <summary>
        /// 字典项编码
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string ItemCode { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? Sort { get; set; }

        /// <summary>
        /// 状态 (0:禁用 1:正常)
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? Status { get; set; } = 1;

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; } = DateTime.Now;
    }
}
