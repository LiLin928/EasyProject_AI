using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWechatModels.Entitys
{
    #region 轮播图

    [Table("mall_banner")]
    [SugarTable("mall_banner", "轮播图表")]
    public class MallBanner
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "varchar", Length = 50)]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [SugarColumn(Length = 200, IsNullable = true)]
        public string Title { get; set; }

        [SugarColumn(Length = 500, IsNullable = true)]
        public string Image { get; set; }

        [SugarColumn(Length = 500, IsNullable = true)]
        public string Link { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? Sort { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? Status { get; set; } = 1;

        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; } = DateTime.Now;
    }

    #endregion

    #region 商品

    [Table("mall_goods")]
    [SugarTable("mall_goods", "商品表")]
    public class MallGoods
    {
        [Key]
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "varchar")]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [SugarColumn(Length = 200, IsNullable = true)]
        public string Name { get; set; }

        [SugarColumn(Length = 1000, IsNullable = true)]
        public string Description { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? Price { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? OriginalPrice { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? Stock { get; set; }

        [SugarColumn(Length = 500, IsNullable = true)]
        public string Image { get; set; }

        [SugarColumn(Length = 2000, IsNullable = true)]
        public string Images { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string CategoryId { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? Status { get; set; } = 1;

        [SugarColumn(IsNullable = true)]
        public int? Sort { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; } = DateTime.Now;

        [SugarColumn(IsNullable = true)]
        public DateTime? UpdateTime { get; set; }
    }

    #endregion

    #region 购物车

    [Table("mall_cart")]
    [SugarTable("mall_cart", "购物车表")]
    public class MallCart
    {
        [Key]
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "varchar")]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [SugarColumn(Length = 50, IsNullable = true)]
        public string UserId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string GoodsId { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? Quantity { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; } = DateTime.Now;

        [SugarColumn(IsNullable = true)]
        public DateTime? UpdateTime { get; set; }
    }

    #endregion

    #region 地址

    [Table("mall_address")]
    [SugarTable("mall_address", "地址表")]
    public class MallAddress
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "varchar", Length = 50)]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [SugarColumn(Length = 50, IsNullable = true)]
        public string UserId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string Receiver { get; set; }

        [SugarColumn(Length = 20, IsNullable = true)]
        public string Phone { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)]
        public string Province { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)]
        public string City { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)]
        public string District { get; set; }

        [SugarColumn(Length = 500, IsNullable = true)]
        public string Detail { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? IsDefault { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; } = DateTime.Now;
    }

    #endregion

    #region 订单

    [Table("mall_order")]
    [SugarTable("mall_order", "订单表")]
    public class MallOrder
    {
        [Key]
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "varchar", Length = 50)]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [SugarColumn(Length = 50, IsNullable = true)]
        public string UserId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string OrderNo { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string RequestId { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? Status { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? TotalAmount { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? PaidAmount { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? OrderDate { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; } = DateTime.Now;
    }

    [Table("mall_order_item")]
    [SugarTable("mall_order_item", "订单明细表")]
    public class MallOrderItem
    {
        [Key]
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "varchar")]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [SugarColumn(Length = 50, IsNullable = true)]
        public string OrderId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string GoodsId { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string GoodsName { get; set; }

        [SugarColumn(Length = 500, IsNullable = true)]
        public string GoodsImage { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? Price { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? Quantity { get; set; }
    }

    #endregion
}
