using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWechatModels.Entitys
{
    /// <summary>
    /// 字典表
    /// </summary>
    [Table("base_dic")]
    public class BaseDic
    {
        [Key]
        public long Id { get; set; }
        public string DicName { get; set; }
        public string DicCode { get; set; }
        public string Description { get; set; }
        public int Status { get; set; } = 1;
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 字典项表
    /// </summary>
    [Table("base_dic_item")]
    public class BaseDicItem
    {
        [Key]
        public long Id { get; set; }
        public long DicId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public int Sort { get; set; }
        public int Status { get; set; } = 1;
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
