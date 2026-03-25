# CLAUDE.md - EasyWechatWeb 开发指南

> 本文件帮助 AI 助手快速理解 EasyWechatWeb 项目结构、开发规范和常见任务

---

## 🎯 项目概述

**EasyWechatWeb** 是 EasyProject 项目的后端 API 服务：
- **框架**: ASP.NET Core 8.0 WebAPI
- **ORM**: SqlSugar 5.1
- **数据库**: MySQL 8.0  
- **认证**: JWT Token
- **文档**: Swagger

---

## 📁 核心目录说明

```
EasyWechatWeb/
├── Controllers/              # 【API 控制器】修改接口、业务逻辑看这里
│   ├── Basic/               # 基础管理控制器 (Users/Roles/Menus)
│   ├── Buz/                 # 业务控制器
│   ├── Etl/                 # ETL 控制器
│   ├── Report/              # 报表控制器
│   ├── Screen/              # 大屏控制器
│   ├── Workflow/            # 工作流控制器
│   └── FlowerMall/          # 鲜花商城控制器
├── Program.cs               # 【启动配置】中间件注册、服务注册
├── appsettings.json         # 【应用配置】数据库/Redis/JWT配置
└── Config/                  # 配置文件目录
```

### 依赖项目
```
EasyWechatWebApi
├── CommonManager            # 通用组件 (基类/工具类/缓存)
├── EasyWechatModels         # 数据模型 (实体/DTO)
└── BusinessManager          # 业务管理器
    ├── EtlManager           # ETL 模块
    ├── ReportManager        # 报表模块
    ├── ScreenManager        # 大屏模块
    ├── WorkflowManager      # 工作流模块
    └── FlowerMallManager    # 鲜花商城模块
├── EasyWechatWeb            # 控制器，启动文件
├── WechatWeb/                  # 微信小程序Web
├── MobileWeb/                  # 移动端 Web
├── PCWeb/                      # PC 端 Web
├── TestProject/                # 测试项目
└── docs/                       # 文档
```

---

## 🔧 常见开发任务

### 1. 添加新 API 接口

**步骤**:
1. 在 `EasyWechatModels/Entitys/` 创建实体类
2. 在 `EasyWechatModels/Dto/` 创建 DTO
3. 在对应 Manager 模块创建 Service
4. 在 `Controllers/` 创建 Controller

**示例**:
```csharp
// Controllers/Product/ProductController.cs
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;
    
    public ProductController(IProductService productService, ILogger<ProductController> logger)
    {
        _productService = productService;
        _logger = logger;
    }
    
    /// <summary>
    /// 获取产品列表
    /// </summary>
    [HttpGet("list")]
    [ProducesResponseType(typeof(ApiResponse<PageResponse<Product>>), 200)]
    public async Task<ApiResponse<PageResponse<Product>>> GetList(
        [FromQuery] int pageIndex = 1, 
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var list = await _productService.GetListAsync();
            var pageData = PageResponse<Product>.Create(list, list.Count, pageIndex, pageSize);
            return ApiResponse<PageResponse<Product>>.Success(pageData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取产品列表失败");
            return ApiResponse<PageResponse<Product>>.Error("获取产品列表失败");
        }
    }
}
```

### 2. 配置数据库连接

**文件**: `appsettings.json`
```json
{
  "MasterSlaveConnectionStrings": [
    {
      "ConnectionString": "Server=127.0.0.1;Database=EasyProject;User ID=root;Password=your_password;Charset=utf8mb4;",
      "DbType": 0,
      "IsAutoCloseConnection": true,
      "InitKeyType": 1
    }
  ]
}
```

### 3. 添加服务注册

**文件**: `Program.cs`
```csharp
// 注册业务服务
builder.Services.AddScoped<IProductService, ProductService>();

// 注册缓存
builder.Services.AddRedisCache();

// 注册 JWT 认证
builder.Services.AddJwtAuthentication();
```

### 4. 添加 Swagger 文档

**文件**: `Program.cs`
```csharp
if (builder.Configuration.GetValue<bool>("IsUseSwagger"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

---

## 📊 代码规范

### 命名规范
- **Controller**: `XXXController.cs`
- **Service**: `XXXService.cs`
- **Entity**: `XXX.cs` (与表名对应)
- **DTO**: `XXXDto.cs` 

### 注释规范
```csharp
/// <summary>
/// 获取产品列表
/// </summary>
/// <param name="pageIndex">页码</param>
/// <param name="pageSize">每页数量</param>
/// <returns>产品列表</returns>
[HttpGet("list")]
public async Task<ApiResponse<PageResponse<Product>>> GetList(...)
```

### 异常处理
```csharp
try
{
    // 业务逻辑
}
catch (Exception ex)
{
    _logger.LogError(ex, "操作失败");
    return ApiResponse.Error("操作失败");
}
```

---

## 🚀 运行和调试

### 启动项目
```bash
cd EasyWechatWeb
dotnet run
```

### 访问 Swagger
```
http://localhost:7016/swagger
```

### 查看日志
```bash
tail -f logs/log*.txt
```

---

## 📝 数据库操作

### SqlSugar 使用示例
```csharp
// 查询列表
var list = await _db.Queryable<Product>()
    .WhereIF(!string.IsNullOrEmpty(keyword), p => p.Name.Contains(keyword))
    .OrderByDescending(p => p.CreateTime)
    .ToPageListAsync(pageIndex, pageSize);

// 查询详情
var product = await _db.Queryable<Product>()
    .Where(p => p.Id == id)
    .FirstAsync();

// 插入
var id = await _db.Insertable(product).ExecuteReturnIdentityAsync();

// 更新
await _db.Updateable(product).ExecuteCommandAsync();

// 删除
await _db.Deleteable<Product>(id).ExecuteCommandAsync();
```

---

## 🔒 认证授权

### JWT Token 配置
```json
{
  "JWTTokenOptions": {
    "Audience": "http://localhost:51062",
    "Issuer": "http://localhost:5106",
    "SecurityKey": "your-secret-key-here"
  }
}
```

### Controller 中使用
```csharp
[Authorize]
public class ProductController : ControllerBase
{
    // 需要认证的接口
}

[AllowAnonymous]
public class AuthController : ControllerBase
{
    // 不需要认证的接口 (如登录)
}
```

---

## 📋 测试指南

### 单元测试
```csharp
[Fact]
public async Task GetList_ShouldReturnProducts()
{
    // Arrange
    var mockService = new Mock<IProductService>();
    var controller = new ProductController(mockService.Object, _logger);
    
    // Act
    var result = await controller.GetList(1, 10);
    
    // Assert
    Assert.NotNull(result);
    Assert.Equal(200, result.Code);
}
```

### API 测试
```bash
# 获取产品列表
curl -X GET "http://localhost:7016/api/Product/list?pageIndex=1&pageSize=10" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

---

## 🐛 常见问题

### 1. 数据库连接失败
**检查**: `appsettings.json` 中的连接字符串  
**解决**: 确认 MySQL 服务运行，连接字符串正确

### 2. JWT 认证失败
**检查**: JWT Secret 配置  
**解决**: 确保 SecurityKey 长度足够 (至少 16 字符)

### 3. Swagger 无法访问
**检查**: `IsUseSwagger` 配置  
**解决**: 设置 `IsUseSwagger: true`

---

## 📚 相关文档

- [需求方案.md](需求方案.md) - 项目需求说明
- [README.md](../README.md) - 项目总览
- [docs/](../docs/) - 技术文档

---

**最后更新**: 2026-03-24  
**维护人**: 开发团队
