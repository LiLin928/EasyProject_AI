using System;

namespace EasyWechatModels.Dto
{
    /// <summary>
    /// 用户响应 DTO
    /// </summary>
    public class BaseUsersRes
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Nickname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public int Gender { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? LastLoginTime { get; set; }
    }

    /// <summary>
    /// 用户请求 DTO
    /// </summary>
    public class BaseUsersReq
    {
        public long? Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public int Gender { get; set; }
        public int Status { get; set; } = 1;
    }

    /// <summary>
    /// 角色响应 DTO
    /// </summary>
    public class BaseRoleRes
    {
        public long Id { get; set; }
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 角色请求 DTO
    /// </summary>
    public class BaseRoleReq
    {
        public long? Id { get; set; }
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public string Description { get; set; }
        public int Status { get; set; } = 1;
    }

    /// <summary>
    /// 菜单响应 DTO
    /// </summary>
    public class BaseMenuRes
    {
        public long Id { get; set; }
        public long ParentId { get; set; }
        public string MenuName { get; set; }
        public int MenuType { get; set; }
        public string Path { get; set; }
        public string Component { get; set; }
        public string Permission { get; set; }
        public string Icon { get; set; }
        public int Sort { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 菜单请求 DTO
    /// </summary>
    public class BaseMenuReq
    {
        public long? Id { get; set; }
        public long ParentId { get; set; }
        public string MenuName { get; set; }
        public int MenuType { get; set; }
        public string Path { get; set; }
        public string Component { get; set; }
        public string Permission { get; set; }
        public string Icon { get; set; }
        public int Sort { get; set; }
        public int Status { get; set; } = 1;
    }

    /// <summary>
    /// 字典响应 DTO
    /// </summary>
    public class BaseDicRes
    {
        public long Id { get; set; }
        public string DicName { get; set; }
        public string DicCode { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 字典请求 DTO
    /// </summary>
    public class BaseDicReq
    {
        public long? Id { get; set; }
        public string DicName { get; set; }
        public string DicCode { get; set; }
        public string Description { get; set; }
        public int Status { get; set; } = 1;
    }
}
