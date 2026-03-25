using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWechatModels.Entitys
{
    /// <summary>
    /// 角色表
    /// </summary>
    [Table("base_role")]
    [SugarTable("base_role", "角色表")]
    public class BaseRole
    {
        /// <summary>
        /// 主键 ID (GUID)
        /// </summary>
        [Key]
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "varchar", Length = 50)]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// 角色名称
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string RoleName { get; set; }

        /// <summary>
        /// 角色编码
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string RoleCode { get; set; }

        /// <summary>
        /// 角色描述
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

        /// <summary>
        /// 更新时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? UpdateTime { get; set; }
    }
}
