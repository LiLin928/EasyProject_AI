using System;
using System.Collections.Generic;

namespace EasyWechatModels.Dto
{
    /// <summary>
    /// 商品响应 DTO
    /// </summary>
    public class MallGoodsRes
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Stock { get; set; }
        public string Image { get; set; }
        public string Images { get; set; }
        public int CategoryId { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 商品请求 DTO
    /// </summary>
    public class MallGoodsReq
    {
        public long? Id { get; set; }
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
    }

    /// <summary>
    /// 分类响应 DTO
    /// </summary>
    public class MallCategoryRes
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int Sort { get; set; }
        public int Status { get; set; }
    }

    /// <summary>
    /// 分类请求 DTO
    /// </summary>
    public class MallCategoryReq
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int Sort { get; set; }
        public int Status { get; set; } = 1;
    }

    /// <summary>
    /// 购物车响应 DTO
    /// </summary>
    public class MallCartRes
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long GoodsId { get; set; }
        public MallGoodsRes Goods { get; set; }
        public int Count { get; set; }
    }

    /// <summary>
    /// 购物车请求 DTO
    /// </summary>
    public class MallCartReq
    {
        public long? Id { get; set; }
        public long GoodsId { get; set; }
        public int Count { get; set; }
    }

    /// <summary>
    /// 订单响应 DTO
    /// </summary>
    public class MallOrderRes
    {
        public long Id { get; set; }
        public string OrderNo { get; set; }
        public long UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PayAmount { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? PayTime { get; set; }
        public List<MallOrderItemRes> Items { get; set; }
    }

    /// <summary>
    /// 订单请求 DTO
    /// </summary>
    public class MallOrderReq
    {
        public long? Id { get; set; }
        public List<OrderItemReq> Items { get; set; }
        public string Remark { get; set; }
        public long AddressId { get; set; }
    }

    /// <summary>
    /// 订单详情响应 DTO
    /// </summary>
    public class MallOrderItemRes
    {
        public long Id { get; set; }
        public long GoodsId { get; set; }
        public string GoodsName { get; set; }
        public string GoodsImage { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    /// <summary>
    /// 订单项请求 DTO
    /// </summary>
    public class OrderItemReq
    {
        public long GoodsId { get; set; }
        public int Quantity { get; set; }
    }

    /// <summary>
    /// 地址响应 DTO
    /// </summary>
    public class MallAddressRes
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Receiver { get; set; }
        public string Phone { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Detail { get; set; }
        public int IsDefault { get; set; }
    }

    /// <summary>
    /// 地址请求 DTO
    /// </summary>
    public class MallAddressReq
    {
        public long? Id { get; set; }
        public string Receiver { get; set; }
        public string Phone { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Detail { get; set; }
        public int IsDefault { get; set; }
    }

    /// <summary>
    /// 轮播图响应 DTO
    /// </summary>
    public class MallBannerRes
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public int Sort { get; set; }
    }
}
