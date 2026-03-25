using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWechatModels.Entitys
{
    /// <summary>
    /// 用户表
    /// </summary>
    [Table("base_users")]
    [SugarTable("base_users", "用户表")]
    public class BaseUsers
    {
        /// <summary>
        /// 主键 ID (GUID)
        /// </summary>
        [Key]
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "varchar", Length = 50)]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// 用户名
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string Username { get; set; }

        /// <summary>
        /// 密码（加密）
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string Nickname { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [SugarColumn(Length = 20, IsNullable = true)]
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string Email { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [SugarColumn(Length = 500, IsNullable = true)]
        public string Avatar { get; set; }

        /// <summary>
        /// 性别 (0:未知 1:男 2:女)
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? Gender { get; set; }

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

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? LastLoginTime { get; set; }
    }
}
