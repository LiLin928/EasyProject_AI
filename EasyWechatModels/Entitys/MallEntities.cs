using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWechatModels.Entitys
{
    /// <summary>
    /// 商品表
    /// </summary>
    [Table("mall_goods")]
    public class MallGoods
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Stock { get; set; }
        public string Image { get; set; }
        public string Images { get; set; }
        public int CategoryId { get; set; }
        public int Status { get; set; } = 1;
        public int Sort { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public DateTime? UpdateTime { get; set; }
    }

    /// <summary>
    /// 商品分类表
    /// </summary>
    [Table("mall_category")]
    public class MallCategory
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int Sort { get; set; }
        public int Status { get; set; } = 1;
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 购物车表
    /// </summary>
    [Table("mall_cart")]
    public class MallCart
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        public long GoodsId { get; set; }
        public int Count { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public DateTime? UpdateTime { get; set; }
    }

    /// <summary>
    /// 订单表
    /// </summary>
    [Table("mall_order")]
    public class MallOrder
    {
        [Key]
        public long Id { get; set; }
        public string OrderNo { get; set; }
        public long UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PayAmount { get; set; }
        public int Status { get; set; } = 0;
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public DateTime? PayTime { get; set; }
        public DateTime? DeliverTime { get; set; }
        public DateTime? ReceiveTime { get; set; }
    }

    /// <summary>
    /// 订单详情表
    /// </summary>
    [Table("mall_order_item")]
    public class MallOrderItem
    {
        [Key]
        public long Id { get; set; }
        public long OrderId { get; set; }
        public long GoodsId { get; set; }
        public string GoodsName { get; set; }
        public string GoodsImage { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    /// <summary>
    /// 收货地址表
    /// </summary>
    [Table("mall_address")]
    public class MallAddress
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Receiver { get; set; }
        public string Phone { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Detail { get; set; }
        public int IsDefault { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 轮播图表
    /// </summary>
    [Table("mall_banner")]
    public class MallBanner
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public int Sort { get; set; }
        public int Status { get; set; } = 1;
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
