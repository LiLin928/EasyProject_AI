using Autofac;
using System;

namespace CommonManager.Utility
{
    /// <summary>
    /// Autofac IOC 容器初始化类
    /// </summary>
    public static class AutofacInit
    {
        private static IContainer _container;

        /// <summary>
        /// 初始化容器
        /// </summary>
        public static void InitContainer(IContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// 从容器获取对象
        /// 示例：AutofacInit.GetServiceFromFac<IFreeSql>()
        /// </summary>
        public static T GetServiceFromFac<T>() where T : notnull
        {
            return _container.Resolve<T>();
        }

        /// <summary>
        /// 从容器获取对象（泛型）
        /// </summary>
        public static T Resolve<T>() where T : notnull
        {
            return _container.Resolve<T>();
        }

        /// <summary>
        /// 从容器获取对象（指定类型）
        /// </summary>
        public static object Resolve(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        /// <summary>
        /// 从容器获取对象（带别名）
        /// </summary>
        public static object Resolve(Type serviceType, string aliasName)
        {
            return _container.ResolveNamed(aliasName, serviceType);
        }
    }
}
