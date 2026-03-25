using Serilog;
using SqlSugar;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 配置 Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// 添加服务
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "EasyWechatWeb API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "请输入 token，格式为 Bearer xxxxx",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// 配置 JWT 认证
var jwtOptions = builder.Configuration.GetSection("JWTTokenOptions");
var securityKey = jwtOptions["SecurityKey"];
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions["Issuer"],
        ValidAudience = jwtOptions["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey))
    };
});

// 配置 CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 配置 Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // 注册 Autofac 模块
    containerBuilder.RegisterModule(new CommonManager.Utility.AutofacModuleRegister());
    
    // 注册业务服务（动态反射注入 BusinessManager 所有 Service）
    // 已集成 BaseService 的所有服务会自动注入
});

// 配置 Mapster
TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);
builder.Services.AddScoped<IMapper, ServiceMapper>();

// 注册业务服务
builder.Services.AddScoped<BusinessManager.BasicDataManager.IService.IUserService, BusinessManager.BasicDataManager.Service.UserService>();
builder.Services.AddScoped<BusinessManager.BasicDataManager.IService.IRoleService, BusinessManager.BasicDataManager.Service.RoleService>();
builder.Services.AddScoped<BusinessManager.BasicDataManager.IService.IDicService, BusinessManager.BasicDataManager.Service.DicService>();
builder.Services.AddScoped<BusinessManager.FlowerMallManager.IService.IGoodsService, BusinessManager.FlowerMallManager.Service.GoodsService>();
builder.Services.AddScoped<BusinessManager.FlowerMallManager.IService.ICartService, BusinessManager.FlowerMallManager.Service.CartService>();
builder.Services.AddScoped<BusinessManager.FlowerMallManager.IService.IOrderService, BusinessManager.FlowerMallManager.Service.OrderService>();
builder.Services.AddScoped<BusinessManager.FlowerMallManager.IService.IAddressService, BusinessManager.FlowerMallManager.Service.AddressService>();
builder.Services.AddScoped<BusinessManager.FlowerMallManager.IService.IBannerService, BusinessManager.FlowerMallManager.Service.BannerService>();
builder.Services.AddScoped<BusinessManager.SrmManager.IService.IPurchaseRequestService, BusinessManager.SrmManager.Service.PurchaseRequestService>();
builder.Services.AddScoped<BusinessManager.SrmManager.IService.ISupplierService, BusinessManager.SrmManager.Service.SupplierService>();
builder.Services.AddScoped<BusinessManager.SrmManager.IService.IPurchaseOrderService, BusinessManager.SrmManager.Service.PurchaseOrderService>();
builder.Services.AddScoped<BusinessManager.SrmManager.IService.IInvoiceService, BusinessManager.SrmManager.Service.InvoiceService>();
builder.Services.AddScoped<BusinessManager.SrmManager.IService.IPaymentService, BusinessManager.SrmManager.Service.PaymentService>();
builder.Services.AddScoped<BusinessManager.SrmManager.IService.ISettlementService, BusinessManager.SrmManager.Service.SettlementService>();
builder.Services.AddScoped<BusinessManager.WorkflowManager.IService.IWorkflowService, BusinessManager.WorkflowManager.Service.WorkflowService>();

// 配置 SqlSugar
builder.Services.AddScoped<ISqlSugarClient>(provider =>
{
    var options = builder.Configuration.GetSection("MasterSlaveConnectionStrings").Get<List<ConnectionConfig>>() ?? new List<ConnectionConfig>();
    var client = new SqlSugarScope(options);
    
    // SQL 执行前事件
    client.Aop.OnLogExecuting = (sql, pars) =>
    {
        Log.Information("SQL: {Sql}", sql);
    };
    
    // SQL 执行后事件
    client.Aop.OnLogExecuted = (sql, pars) =>
    {
        Log.Information("SQL 执行耗时：{Ms}ms", client.Ado.SqlExecutionTime.TotalMilliseconds);
    };
    
    // 错误事件
    client.Aop.OnError = (exp) =>
    {
        Log.Error(exp, "SQL 执行错误");
    };
    
    return client;
});

// 配置 Redis
var redisConfig = builder.Configuration.GetSection("Redis");
if (!string.IsNullOrEmpty(redisConfig["Host"]))
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = $"{redisConfig["Host"]}:{redisConfig["Port"]}";
        options.InstanceName = redisConfig["preName"] ?? "EasyProject:";
    });
}

var app = builder.Build();

// 配置中间件管道
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

try
{
    Log.Information("Starting EasyWechatWeb API");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
