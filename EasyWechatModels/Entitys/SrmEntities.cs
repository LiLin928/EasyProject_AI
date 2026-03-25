using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWechatModels.Entitys
{
    #region 采购申请

    /// <summary>
    /// 采购申请单
    /// </summary>
    [Table("srm_purchase_request")]
    public class SrmPurchaseRequest
    {
        [Key]
        public long Id { get; set; }
        public string RequestNo { get; set; }
        public long ApplicantId { get; set; }
        public string ApplicantName { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int Status { get; set; } // 0:草稿 1:待审批 2:已批准 3:已拒绝 4:已取消
        public decimal TotalAmount { get; set; }
        public string Remark { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public long? ApproverId { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 采购申请明细
    /// </summary>
    [Table("srm_purchase_request_item")]
    public class SrmPurchaseRequestItem
    {
        [Key]
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

    #endregion

    #region 采购订单

    /// <summary>
    /// 采购订单
    /// </summary>
    [Table("srm_purchase_order")]
    public class SrmPurchaseOrder
    {
        [Key]
        public long Id { get; set; }
        public string OrderNo { get; set; }
        public long RequestId { get; set; }
        public long SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int Status { get; set; } // 0:草稿 1:待确认 2:已确认 3:发货中 4:已完成 5:已取消
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliverDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 采购订单明细
    /// </summary>
    [Table("srm_purchase_order_item")]
    public class SrmPurchaseOrderItem
    {
        [Key]
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

    #endregion

    #region 发票管理

    /// <summary>
    /// 采购发票
    /// </summary>
    [Table("srm_invoice")]
    public class SrmInvoice
    {
        [Key]
        public long Id { get; set; }
        public string InvoiceNo { get; set; }
        public long OrderId { get; set; }
        public long SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int InvoiceType { get; set; } // 1:增值税专票 2:增值税普票 3:普通发票
        public decimal Amount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public int Status { get; set; } // 0:待录入 1:待审核 2:已审核 3:已驳回
        public DateTime InvoiceDate { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }

    #endregion

    #region 付款管理

    /// <summary>
    /// 付款申请
    /// </summary>
    [Table("srm_payment_request")]
    public class SrmPaymentRequest
    {
        [Key]
        public long Id { get; set; }
        public string RequestNo { get; set; }
        public long OrderId { get; set; }
        public long InvoiceId { get; set; }
        public long SupplierId { get; set; }
        public string SupplierName { get; set; }
        public decimal Amount { get; set; }
        public int PaymentType { get; set; } // 1:银行转账 2:支票 3:现金 4:承兑汇票
        public int Status { get; set; } // 0:草稿 1:待审批 2:已批准 3:已付款 4:已拒绝
        public DateTime RequestDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }

    #endregion

    #region 结算管理

    /// <summary>
    /// 结算单
    /// </summary>
    [Table("srm_settlement")]
    public class SrmSettlement
    {
        [Key]
        public long Id { get; set; }
        public string SettlementNo { get; set; }
        public long SupplierId { get; set; }
        public string SupplierName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal UnpaidAmount { get; set; }
        public int Status { get; set; } // 0:待结算 1:结算中 2:已完成
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }

    #endregion

    #region 供应商管理

    /// <summary>
    /// 供应商
    /// </summary>
    [Table("srm_supplier")]
    public class SrmSupplier
    {
        [Key]
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
        public int Status { get; set; } // 0:禁用 1:正常 2:黑名单
        public int Level { get; set; } // 1:普通 2:重要 3:战略
        public decimal CreditLimit { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }

    #endregion

    #region 报表配置

    /// <summary>
    /// 报表定义
    /// </summary>
    [Table("rpt_report")]
    public class RptReport
    {
        [Key]
        public long Id { get; set; }
        public string ReportCode { get; set; }
        public string ReportName { get; set; }
        public string ReportType { get; set; }
        public string SqlContent { get; set; }
        public string Columns { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }

    #endregion

    #region ETL 配置

    /// <summary>
    /// ETL 任务
    /// </summary>
    [Table("etl_task")]
    public class EtlTask
    {
        [Key]
        public long Id { get; set; }
        public string TaskCode { get; set; }
        public string TaskName { get; set; }
        public string SourceType { get; set; }
        public string SourceConfig { get; set; }
        public string TargetType { get; set; }
        public string TargetConfig { get; set; }
        public string TransformScript { get; set; }
        public string Schedule { get; set; }
        public int Status { get; set; } // 0:停用 1:启用
        public DateTime? LastRunTime { get; set; }
        public int LastRunStatus { get; set; }
        public string LastRunError { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// ETL 任务日志
    /// </summary>
    [Table("etl_task_log")]
    public class EtlTaskLog
    {
        [Key]
        public long Id { get; set; }
        public long TaskId { get; set; }
        public int Status { get; set; } // 0:失败 1:成功
        public int RecordsProcessed { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string ErrorMessage { get; set; }
    }

    #endregion

    #region 大屏配置

    /// <summary>
    /// 大屏项目
    /// </summary>
    [Table("screen_project")]
    public class ScreenProject
    {
        [Key]
        public long Id { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string LayoutConfig { get; set; }
        public string ComponentConfig { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }

    #endregion

    #region 系统配置

    /// <summary>
    /// 系统配置项
    /// </summary>
    [Table("sys_config")]
    public class SysConfig
    {
        [Key]
        public long Id { get; set; }
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
        public string ConfigType { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }

    #endregion

    #region 工作流

    [Table("wf_definition")]
    public class WorkflowDefinition
    {
        [Key]
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Config { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }

    #endregion
}
