using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.Extensions.Logging;
using Schany.Core.Dto.Sys;
using Schany.Core.Service.Core.Sys;
using Schany.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace Schany.Framework
{
    /// <summary>
    /// IdentityServer4资源
    /// </summary>
    public class IdentityServer4Resources
    {
        /// <summary>
        /// 添加对OpenId Connect的支持
        /// 定义系统的标示资源
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(), //必须要添加，否则报无效的scope错误                 
                new IdentityResources.Profile()
            };
        }


        /// <summary>
        /// 定义系统中受保护的API资源
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource> {
                new ApiResource("api", "identity_api")
            };
        }

        /// <summary>
        /// 定义允许访问系统API资源的客户端
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                //自定义客户端             
                new Client
                {
                    //客户端名称                    
                    ClientId = "browser_client",
                    //客户端访问方式：密码验证                  
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    //Token过去时间（默认1小时）
                    AccessTokenLifetime=3600,
                    //用于认证的密码加密方式                   
                    ClientSecrets = {
                        new Secret("secret".Sha256())
                    },
                    //客户端有权访问的范围         
                    AllowedScopes = { "api",
                        IdentityServerConstants.StandardScopes.OpenId,
                        //必须要添加，否则报403 forbidden错误                
                        IdentityServerConstants.StandardScopes.Profile,
                        //如果想带有RefreshToken，那么必须设置：StandardScopes.OfflineAccess
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    },
                    AllowOfflineAccess = true,
                    AbsoluteRefreshTokenLifetime = 3600*2,        //RefreshToken的最大过期时间
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,   //默认状态，RefreshToken只能使用一次，使用一次之后旧的就不能使用了，只能使用新的RefreshToken
                    //RefreshTokenUsage = TokenUsage.ReUse,       //可重复使用RefreshToken，RefreshToken，当然过期了就不能使用了
                }
            };
        }


    }

    /// <summary>
    /// IdentityServer4自定义登陆校验
    /// </summary>
    public class LoginValidator : IResourceOwnerPasswordValidator
    {
        private readonly ILogger logger;       
        private readonly ICustomerService customerServie;
        public LoginValidator(ICustomerService _customerServie, ILogger<ProfileService> _logger)
        {
            customerServie = _customerServie;
            logger = _logger;
        }

        /// <summary>
        /// 登陆校验
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            await Task.Run(() =>
            {
                //根据context.UserName和context.Password与数据库的数据做校验，判断是否合法
                string userName = context.UserName;
                string password = context.Password;
                CustomerClaimModel claimModel = customerServie.Login(userName, password);
                if (claimModel.Notice.NotifyType == NotifyType.Success)
                {
                    //用户名密码验证成功
                    context.Result = new GrantValidationResult(
                       subject: context.UserName,
                       authenticationMethod: "custom",
                       claims: GetUserClaims(claimModel)
                    );
                }
                else
                {
                    //验证失败               
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, claimModel.Notice.Message);
                }
            });
        }

        /// <summary>      
        /// 创建证件单元信息      
        /// </summary>      
        /// <returns></returns>      
        private Claim[] GetUserClaims(CustomerClaimModel claimModel)
        {
            return new Claim[]
            {
                new Claim("CustomerId", claimModel.CustomerId.ToString()),
                new Claim("UserName", claimModel.UserName.ToString()),
                new Claim("TrueName", claimModel.TrueName.ToString()),
                new Claim("Pic", string.IsNullOrEmpty(claimModel.Pic)?string.Empty:claimModel.Pic.ToString())
            };
        }
    }

    public class ProfileService : IProfileService
    {
        private readonly ILogger _logger;
        public ProfileService(ILogger<ProfileService> logger)
        {
            _logger = logger;
        }

        /// <summary>         
        /// 获取用户信息        
        /// </summary>         
        /// <param name="context"></param>         
        /// <returns></returns>         
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            await Task.Run(() =>
            {
                try
                {
                    var dd = _logger;
                    //用户信息                 
                    var claims = context.Subject.Claims.ToList();
                    //获取用户信息                 
                    context.IssuedClaims = claims.ToList();
                    //_logger.LogInformation("获取登陆用户信息成功");
                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format("获取登陆用户信息发生异常：{0}", ex.Message));
                }
            });
        }
        /// <summary>       
        /// 获取或设置一个值，该值指示主题是否处于活动状态并且可以接收令牌。
        /// </summary>         
        /// <param name="context"></param>      
        /// <returns></returns>  

        public async Task IsActiveAsync(IsActiveContext context)
        {
            await Task.Run(() =>
            {
                context.IsActive = true;
            });
        }
    }

}
