using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWechatModels.Entitys
{
    /// <summary>
    /// 角色表
    /// </summary>
    [Table("base_role")]
    public class BaseRole
    {
        /// <summary>
        /// 主键 ID
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色编码
        /// </summary>
        public string RoleCode { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 状态 (0:禁用 1:正常)
        /// </summary>
        public int Status { get; set; } = 1;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}
