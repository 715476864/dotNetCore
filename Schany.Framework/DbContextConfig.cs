using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Schany.Data.DataContext;
using Schany.Data.Entities.Sys;
using Schany.Data.Enums;
using Schany.Infrastructure.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Schany.Framework
{
    /// <summary>
    /// 数据库上下文配置
    /// </summary>
    public class DbContextConfig
    {       

        public static void DoConfig(IServiceCollection services, string connStr)
        {
            //将Context注册到容器
            services.AddDbContext<SchanyDbContext>(options =>
            {
                options.UseSqlServer(connStr);
            });
        }

        public static void DbInit(IServiceScope scope)
        {
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<SchanyDbContext>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("Schany.Framework.DbContextConfig");
            try
            {
                Guid customerId = Guid.Parse("F4D4EF62-1111-2222-3333-6474F4D8F756");
                if (!dbContext.Customers.Any())
                {
                    //如果Customer表中没有任何数据，则向该表中插入种子数据。
                    var entity = new Customer()
                    {
                        Id = customerId,
                        UserName = "admin",
                        Password = HashHelper.GetMd5("sndd2710078"),
                        TrueName = "管理员",
                        CreateUserId = customerId,
                        LoginErrorTimes=0,
                        IsLocked=false
                    };
                    dbContext.Add(entity);
                }

                if (!dbContext.MyDictionaries.Any())
                {
                    //如果字典表中不存在任何数据，则向该表中插入种子数据。
                    List<MyDictionary> entities = new List<MyDictionary>();

                    //【民族】数据初始化
                    var nationStr = "汉族、蒙古族、回族、藏族、维吾尔族、苗族、彝族、壮族、布依族、朝鲜族、满族、侗族、瑶族、白族、土家族、哈尼族、哈萨克族、傣族、黎族、僳僳族、佤族、畲族、高山族、拉祜族、水族、东乡族、纳西族、景颇族、柯尔克孜族、土族、达斡尔族、仫佬族、羌族、布朗族、撒拉族、毛南族、仡佬族、锡伯族、阿昌族、普米族、塔吉克族、怒族、乌孜别克族、俄罗斯族、鄂温克族、德昂族、保安族、裕固族、京族、塔塔尔族、独龙族、鄂伦春族、赫哲族、门巴族、珞巴族、基诺族";
                    var nationArray = nationStr.Split('、');
                    int Code = 1;
                    foreach (var item in nationArray)
                    {
                        int _code = (int)DicType.Nation * 100 + Code;
                        entities.Add(new MyDictionary()
                        {
                            DicType = DicType.Nation,
                            Code = _code,
                            Text = item,
                            CreateUserId = customerId
                        });
                        Code++;
                    }
                    dbContext.MyDictionaries.AddRange(entities);
                }

                if (!dbContext.Modules.Any())
                {
                    //如果Modules表中不存在任何数据，则向该表中插入种子数据。
                    List<Module> entities = new List<Module>();
                    entities.Add(new Module()
                    {
                        Name = "系统模块",
                        Text = "sysmodule",
                        DisplayNo = 1,
                        AppendClass = "ion-grid",
                        HasPermission = false
                    });
                    entities.Add(new Module()
                    {
                        Name = "预置数据权限",
                        Text = "presetdatapermission",
                        DisplayNo = 1,
                        AppendClass = "ion-gear-b",
                        HasPermission = false
                    });
                    dbContext.Modules.AddRange(entities);
                }

                var res = dbContext.SaveChanges();
                logger.LogInformation("数据库初始化成功，本次共创建{0}条数据。", Convert.ToString(res));
            }
            catch (Exception ex)
            {
                //记录错误日志
                logger.LogError("数据库初始化发生异常：" + ex.Message);
            }
        }
    }
}
