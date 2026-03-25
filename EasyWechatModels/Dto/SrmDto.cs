using System;
using System.Collections.Generic;

namespace EasyWechatModels.Dto
{
    #region 采购申请

    public class SrmPurchaseRequestRes
    {
        public long Id { get; set; }
        public string RequestNo { get; set; }
        public long ApplicantId { get; set; }
        public string ApplicantName { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public decimal TotalAmount { get; set; }
        public string Remark { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime CreateTime { get; set; }
        public List<SrmPurchaseRequestItemRes> Items { get; set; }
    }

    public class SrmPurchaseRequestReq
    {
        public long? Id { get; set; }
        public long DepartmentId { get; set; }
        public string Remark { get; set; }
        public List<PurchaseRequestItemReq> Items { get; set; }
    }

    public class SrmPurchaseRequestItemRes
    {
        public long Id { get; set; }
        public long RequestId { get; set; }
        public long GoodsId { get; set; }
        public string GoodsName { get; set; }
        public string Specification { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; }
    }

    public class PurchaseRequestItemReq
    {
        public long GoodsId { get; set; }
        public string GoodsName { get; set; }
        public string Specification { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Remark { get; set; }
    }

    #endregion

    #region 采购订单

    public class SrmPurchaseOrderRes
    {
        public long Id { get; set; }
        public string OrderNo { get; set; }
        public long RequestId { get; set; }
        public long SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliverDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public List<SrmPurchaseOrderItemRes> Items { get; set; }
    }

    public class SrmPurchaseOrderReq
    {
        public long? Id { get; set; }
        public long RequestId { get; set; }
        public long SupplierId { get; set; }
        public string Remark { get; set; }
        public List<PurchaseOrderItemReq> Items { get; set; }
    }

    public class SrmPurchaseOrderItemRes
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public long GoodsId { get; set; }
        public string GoodsName { get; set; }
        public string Specification { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public int ReceivedQuantity { get; set; }
    }

    public class PurchaseOrderItemReq
    {
        public long GoodsId { get; set; }
        public string GoodsName { get; set; }
        public string Specification { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    #endregion

    #region 发票管理

    public class SrmInvoiceRes
    {
        public long Id { get; set; }
        public string InvoiceNo { get; set; }
        public long OrderId { get; set; }
        public long SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int InvoiceType { get; set; }
        public string InvoiceTypeText { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class SrmInvoiceReq
    {
        public long? Id { get; set; }
        public long OrderId { get; set; }
        public long SupplierId { get; set; }
        public string InvoiceNo { get; set; }
        public int InvoiceType { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxAmount { get; set; }
        public DateTime InvoiceDate { get; set; }
    }

    #endregion

    #region 付款管理

    public class SrmPaymentRequestRes
    {
        public long Id { get; set; }
        public string RequestNo { get; set; }
        public long OrderId { get; set; }
        public long InvoiceId { get; set; }
        public long SupplierId { get; set; }
        public string SupplierName { get; set; }
        public decimal Amount { get; set; }
        public int PaymentType { get; set; }
        public string PaymentTypeText { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class SrmPaymentRequestReq
    {
        public long? Id { get; set; }
        public long OrderId { get; set; }
        public long InvoiceId { get; set; }
        public long SupplierId { get; set; }
        public decimal Amount { get; set; }
        public int PaymentType { get; set; }
        public string Remark { get; set; }
    }

    #endregion

    #region 结算管理

    public class SrmSettlementRes
    {
        public long Id { get; set; }
        public string SettlementNo { get; set; }
        public long SupplierId { get; set; }
        public string SupplierName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal UnpaidAmount { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class SrmSettlementReq
    {
        public long? Id { get; set; }
        public long SupplierId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    #endregion

    #region 供应商管理

    public class SrmSupplierRes
    {
        public long Id { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string Address { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string TaxNo { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public int Level { get; set; }
        public string LevelText { get; set; }
        public decimal CreditLimit { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class SrmSupplierReq
    {
        public long? Id { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string Address { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string TaxNo { get; set; }
        public int Status { get; set; } = 1;
        public int Level { get; set; } = 1;
        public decimal CreditLimit { get; set; }
    }

    #endregion

    #region 报表管理

    public class RptReportRes
    {
        public long Id { get; set; }
        public string ReportCode { get; set; }
        public string ReportName { get; set; }
        public string ReportType { get; set; }
        public string SqlContent { get; set; }
        public string Columns { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
    }

    #endregion

    #region ETL 管理

    public class EtlTaskRes
    {
        public long Id { get; set; }
        public string TaskCode { get; set; }
        public string TaskName { get; set; }
        public string SourceType { get; set; }
        public string SourceConfig { get; set; }
        public string TargetType { get; set; }
        public string TargetConfig { get; set; }
        public string TransformScript { get; set; }
        public string Schedule { get; set; }
        public int Status { get; set; }
        public DateTime? LastRunTime { get; set; }
        public int LastRunStatus { get; set; }
        public string LastRunError { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class EtlTaskReq
    {
        public long? Id { get; set; }
        public string TaskCode { get; set; }
        public string TaskName { get; set; }
        public string SourceType { get; set; }
        public string SourceConfig { get; set; }
        public string TargetType { get; set; }
        public string TargetConfig { get; set; }
        public string TransformScript { get; set; }
        public string Schedule { get; set; }
        public int Status { get; set; } = 1;
    }

    #endregion

    #region 大屏管理

    public class ScreenProjectRes
    {
        public long Id { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string LayoutConfig { get; set; }
        public string ComponentConfig { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
    }

    #endregion

    #region 系统配置

    public class SysConfigRes
    {
        public long Id { get; set; }
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
        public string ConfigType { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class SysConfigReq
    {
        public long? Id { get; set; }
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
        public string ConfigType { get; set; }
        public string Description { get; set; }
        public int Status { get; set; } = 1;
    }

    #endregion
}
