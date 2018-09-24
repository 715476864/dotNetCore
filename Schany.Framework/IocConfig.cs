using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Schany.Core.Repository;
using Schany.Core.Service.Core.Sys;
using Schany.Core.Service.Core.Sys.Impl;


namespace Schany.Framework
{
    /// <summary>
    /// 依赖注入配置
    /// </summary>
    public class IocConfig
    {
        public static void DoDependencyInjection(IServiceCollection services)
        {
            //依赖注入不同类型的生命周期
            //Transient：每一次GetService都会创建一个新的实例
            //Scoped：在同一个Scope内只初始化一个实例 ，可以理解为（ 每一个request级别只创建一个实例，同一个http request会在一个 scope内）
            //Singleton ：整个应用程序生命周期以内只创建一个实例

            //仓储类(泛型)及实现注入
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            //具体类及实现注入
            services.AddScoped<ICustomerService, CustomerService>();

            
        }
    }
}
