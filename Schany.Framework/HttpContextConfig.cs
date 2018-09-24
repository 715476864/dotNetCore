using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Schany.Framework
{
    public static class MyHttpContext
    {
        private static IHttpContextAccessor _httpContextAccessor;
        public static Microsoft.AspNetCore.Http.HttpContext Current => _httpContextAccessor.HttpContext;
        internal static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }

    public static class StaticHttpContextExtensions
    {
        /// <summary>
        /// Http请求访问器注入
        /// </summary>
        /// <param name="services"></param>
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
        {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            MyHttpContext.Configure(httpContextAccessor);
            return app;
        }
    }
}
