using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Schany.Core.Dto;
using Schany.Core.Service.SignalR;
using Schany.Framework;

namespace Schany.Api
{
    public class Startup
    {

        public IConfiguration Configuration { get; set; }

        public Startup()
        {
            //添加对配置文件的管理。
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //跨域设置
            services.AddCors(options =>
                options.AddPolicy("cors",
                builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials())
            );

            //数据库上下文配置            
            DbContextConfig.DoConfig(services, Configuration["Root:Database:ConnectionString"]);

            //配置身份服务器与内存中的存储，密钥，客户端和资源
            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddInMemoryIdentityResources(IdentityServer4Resources.GetIdentityResources())
                    .AddInMemoryApiResources(IdentityServer4Resources.GetApiResources())
                    .AddInMemoryClients(IdentityServer4Resources.GetClients())
                    .AddResourceOwnerValidator<LoginValidator>()//添加自定义验证类
                    .AddProfileService<ProfileService>();

            services.AddAuthentication("Bearer")//添加授权模式                        
                    .AddIdentityServerAuthentication(Options => {
                        Options.Authority = "http://localhost:5000";//授权服务器地址
                        Options.RequireHttpsMetadata = false;//没有证书，不启用https
                        Options.ApiName = "api";//指定ApiName与Config配置中的相同
                    });

            //添加Mvc服务到DI容器
            services.AddMvc();
            //添加跨域服务到DI容器
            services.AddCors();
            //将Options注入到DI容器
            services.AddOptions();
            services.Configure<AppsettingDataModel>(Configuration.GetSection("Root"));
            //Http请求访问器注入到DI容器
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSignalR();

            //业务逻辑注入到DI容器
            IocConfig.DoDependencyInjection(services);

            //创建Redis单例实例
            //RedisConfig.CreateInstance(Configuration["Root:Redis:Host"], Configuration["Root:Redis:PreKey"]);
        }

        /// <summary>
        /// 此方法由运行时调用。
        /// 使用此方法对Http请求管道进行配置。
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //使用异常拦截中间件
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
                        
            //使用跨域中间件（配置允许任何请求源，请求头的请求。）
            app.UseCors("cors");

            //添加StaticHttpContext到HTTP管道中
            //StaticHttpContext是自定义的中间件，用于获取当前请求的上下文（HttpCotext）
            app.UseStaticHttpContext();

            //使用静态文件输出中间件（静态文件输出中间件是将js；css；img；html等文件直接提供给客户端）
            app.UseStaticFiles();

            //添加IdentityServer到HTTP管道中（它是负责分发Token的）
            app.UseIdentityServer();

            //添加Authenticatio到HTTP管道中（它是负责验证Token的）
            app.UseAuthentication();


            app.UseSignalR(routes =>
            {
                routes.MapHub<SchanyHub>("/schanyhub");
            });

            //使用Mvc中间件并配置Mvc路由
            app.UseMvc(routeBuilder =>
            {
                routeBuilder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
            });            

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
