using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWechatModels.Entitys
{
    #region 采购申请

    [Table("srm_purchase_request")]
    [SugarTable("srm_purchase_request", "采购申请单")]
    public class SrmPurchaseRequest
    {
        [Key]
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "varchar")]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [SugarColumn(Length = 50, IsNullable = true)]
        public string RequestNo { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string ApplicantId { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)]
        public string ApplicantName { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string DepartmentId { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)]
        public string DepartmentName { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? Status { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? TotalAmount { get; set; }

        [SugarColumn(Length = 500, IsNullable = true)]
        public string Remark { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? RequestDate { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? ApprovedDate { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string ApproverId { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; } = DateTime.Now;
    }

    [Table("srm_purchase_request_item")]
    [SugarTable("srm_purchase_request_item", "采购申请明细")]
    public class SrmPurchaseRequestItem
    {
        [Key]
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "varchar")]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [SugarColumn(Length = 50, IsNullable = true)]
        public string RequestId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string GoodsId { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string GoodsName { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string Specification { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? Quantity { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? UnitPrice { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? Amount { get; set; }

        [SugarColumn(Length = 500, IsNullable = true)]
        public string Remark { get; set; }
    }

    #endregion

    #region 采购订单

    [Table("srm_purchase_order")]
    [SugarTable("srm_purchase_order", "采购订单")]
    public class SrmPurchaseOrder
    {
        [Key]
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "varchar")]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [SugarColumn(Length = 50, IsNullable = true)]
        public string OrderNo { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string RequestId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string SupplierId { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string SupplierName { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? Status { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? TotalAmount { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? PaidAmount { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? OrderDate { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? DeliverDate { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? ReceiveDate { get; set; }

        [SugarColumn(Length = 500, IsNullable = true)]
        public string Remark { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; } = DateTime.Now;
    }

    [Table("srm_purchase_order_item")]
    [SugarTable("srm_purchase_order_item", "采购订单明细")]
    public class SrmPurchaseOrderItem
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

        [SugarColumn(Length = 200, IsNullable = true)]
        public string Specification { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? Quantity { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? UnitPrice { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? Amount { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? ReceivedQuantity { get; set; }
    }

    #endregion

    #region 发票管理

    [Table("srm_invoice")]
    [SugarTable("srm_invoice", "采购发票")]
    public class SrmInvoice
    {
        [Key]
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "varchar")]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [SugarColumn(Length = 100, IsNullable = true)]
        public string InvoiceNo { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string OrderId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string SupplierId { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string SupplierName { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? InvoiceType { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? Amount { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? TaxAmount { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? TotalAmount { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? Status { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? InvoiceDate { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; } = DateTime.Now;
    }

    #endregion

    #region 付款管理

    [Table("srm_payment_request")]
    [SugarTable("srm_payment_request", "付款申请")]
    public class SrmPaymentRequest
    {
        [Key]
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "varchar")]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [SugarColumn(Length = 50, IsNullable = true)]
        public string RequestNo { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string OrderId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string InvoiceId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string SupplierId { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string SupplierName { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? Amount { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? PaymentType { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? Status { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? RequestDate { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? ApprovedDate { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? PaymentDate { get; set; }

        [SugarColumn(Length = 500, IsNullable = true)]
        public string Remark { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; } = DateTime.Now;
    }

    #endregion

    #region 结算管理

    [Table("srm_settlement")]
    [SugarTable("srm_settlement", "结算单")]
    public class SrmSettlement
    {
        [Key]
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "varchar")]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [SugarColumn(Length = 50, IsNullable = true)]
        public string SettlementNo { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string SupplierId { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string SupplierName { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? TotalAmount { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? PaidAmount { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? UnpaidAmount { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? Status { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? StartDate { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? EndDate { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; } = DateTime.Now;
    }

    #endregion

    #region 供应商管理

    [Table("srm_supplier")]
    [SugarTable("srm_supplier", "供应商")]
    public class SrmSupplier
    {
        [Key]
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "varchar")]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [SugarColumn(Length = 50, IsNullable = true)]
        public string SupplierCode { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string SupplierName { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string ContactPerson { get; set; }

        [SugarColumn(Length = 20, IsNullable = true)]
        public string ContactPhone { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)]
        public string ContactEmail { get; set; }

        [SugarColumn(Length = 500, IsNullable = true)]
        public string Address { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)]
        public string BankName { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string BankAccount { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string TaxNo { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? Status { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? Level { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? CreditLimit { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; } = DateTime.Now;
    }

    #endregion

    #region 工作流

    [Table("wf_definition")]
    [SugarTable("wf_definition", "工作流定义")]
    public class WorkflowDefinition
    {
        [Key]
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "varchar")]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [SugarColumn(Length = 50, IsNullable = true)]
        public string Code { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)]
        public string Name { get; set; }

        [SugarColumn(Length = 20, IsNullable = true)]
        public string Version { get; set; }

        [SugarColumn(Length = -1, IsNullable = true)]
        public string Config { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? Status { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; } = DateTime.Now;
    }

    #endregion
}
