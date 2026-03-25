using System;

namespace CommonManager.Helper
{
    /// <summary>
    /// Consul 服务注册帮助类
    /// </summary>
    public static class ConsulHelper
    {
        private static string _consulAddress;
        private static string _serviceName;
        private static string _serviceIp;
        private static int _servicePort;

        /// <summary>
        /// Consul 配置
        /// </summary>
        public class ConsulOptions
        {
            public string ConsulServiceAddress { get; set; } = "http://localhost:8500";
            public string ServiceName { get; set; } = "EasyProject";
            public string ServiceIp { get; set; } = "127.0.0.1";
            public int ServicePort { get; set; } = 5000;
            public string HealthCheckUrl { get; set; } = "/api/Health/index";
            public int CheckIntervalSeconds { get; set; } = 12;
            public int TimeoutSeconds { get; set; } = 5;
        }

        /// <summary>
        /// 注册服务到 Consul
        /// </summary>
        public static void RegisterService(ConsulOptions options)
        {
            try
            {
                _consulAddress = options.ConsulServiceAddress;
                _serviceName = options.ServiceName;
                _serviceIp = options.ServiceIp;
                _servicePort = options.ServicePort;

                // TODO: 实现 Consul 服务注册逻辑
                // 需要添加 Consul NuGet 包
                LogHelper.Info($"Consul 服务注册：{_serviceIp}:{_servicePort}, 服务名：{_serviceName}, Consul 地址：{_consulAddress}");
                
                Console.WriteLine($"Consul 微服务注册：{_serviceIp}:{_servicePort}，服务名：{_serviceName}，Consul 地址：{_consulAddress}");
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "Consul 服务注册失败");
                Console.WriteLine($"Consul 微服务注册异常：{ex.Message}");
            }
        }

        /// <summary>
        /// 注销服务
        /// </summary>
        public static void DeregisterService()
        {
            try
            {
                // TODO: 实现 Consul 服务注销逻辑
                LogHelper.Info($"Consul 服务注销：{_serviceName}");
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "Consul 服务注销失败");
            }
        }

        /// <summary>
        /// 获取健康的服务实例
        /// </summary>
        public static string GetHealthyServiceInstance(string serviceName)
        {
            try
            {
                // TODO: 实现 Consul 服务发现逻辑
                return $"{_serviceIp}:{_servicePort}";
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, $"获取服务实例失败：{serviceName}");
                return null;
            }
        }
    }
}
