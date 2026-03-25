using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWechatModels.Entitys
{
    /// <summary>
    /// 菜单表
    /// </summary>
    [Table("base_menu")]
    public class BaseMenu
    {
        /// <summary>
        /// 主键 ID
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// 父级 ID
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 菜单类型 (1:目录 2:菜单 3:按钮)
        /// </summary>
        public int MenuType { get; set; }

        /// <summary>
        /// 菜单路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 组件路径
        /// </summary>
        public string Component { get; set; }

        /// <summary>
        /// 权限标识
        /// </summary>
        public string Permission { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; } = 0;

        /// <summary>
        /// 状态 (0:隐藏 1:显示)
        /// </summary>
        public int Status { get; set; } = 1;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
