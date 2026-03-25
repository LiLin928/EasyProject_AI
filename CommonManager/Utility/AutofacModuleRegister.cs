using Autofac;
using System.Reflection;

namespace CommonManager.Utility
{
    /// <summary>
    /// Autofac 模块注册
    /// 自动反射注入 BusinessManager 下面所有的方法
    /// </summary>
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // 注册 CommonManager
            var commonAssembly = Assembly.Load("CommonManager");
            builder.RegisterAssemblyTypes(commonAssembly)
                .Where(t => t.Name.EndsWith("Helper") || t.Name.EndsWith("Cache"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // 注册 BusinessManager - BasicDataManager
            var basicAssembly = Assembly.Load("BasicDataManager");
            builder.RegisterAssemblyTypes(basicAssembly)
                .Where(t => t.Name.EndsWith("Service") && !t.Name.StartsWith("Base"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // 注册 BusinessManager - FlowerMallManager
            var flowerMallAssembly = Assembly.Load("FlowerMallManager");
            builder.RegisterAssemblyTypes(flowerMallAssembly)
                .Where(t => t.Name.EndsWith("Service") && !t.Name.StartsWith("Base"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // 注册其他 BusinessManager 模块（ETL/Report/Screen/Workflow）
            var etlAssembly = Assembly.Load("EtlManager");
            builder.RegisterAssemblyTypes(etlAssembly)
                .Where(t => t.Name.EndsWith("Service") && !t.Name.StartsWith("Base"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            var reportAssembly = Assembly.Load("ReportManager");
            builder.RegisterAssemblyTypes(reportAssembly)
                .Where(t => t.Name.EndsWith("Service") && !t.Name.StartsWith("Base"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            var screenAssembly = Assembly.Load("ScreenManager");
            builder.RegisterAssemblyTypes(screenAssembly)
                .Where(t => t.Name.EndsWith("Service") && !t.Name.StartsWith("Base"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            var workflowAssembly = Assembly.Load("WorkflowManager");
            builder.RegisterAssemblyTypes(workflowAssembly)
                .Where(t => t.Name.EndsWith("Service") && !t.Name.StartsWith("Base"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
