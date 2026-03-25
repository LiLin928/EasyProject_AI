using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWechatModels.Entitys
{
    /// <summary>
    /// 菜单表
    /// </summary>
    [Table("base_menu")]
    [SugarTable("base_menu", "菜单表")]
    public class BaseMenu
    {
        /// <summary>
        /// 主键 ID (GUID)
        /// </summary>
        [Key]
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "varchar", Length = 50)]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// 父级 ID (GUID)
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string ParentId { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string MenuName { get; set; }

        /// <summary>
        /// 菜单类型 (1:目录 2:菜单 3:按钮)
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? MenuType { get; set; }

        /// <summary>
        /// 菜单路径
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true)]
        public string Path { get; set; }

        /// <summary>
        /// 组件路径
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true)]
        public string Component { get; set; }

        /// <summary>
        /// 权限标识
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string Permission { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string Icon { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? Sort { get; set; } = 0;

        /// <summary>
        /// 状态 (0:隐藏 1:显示)
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
