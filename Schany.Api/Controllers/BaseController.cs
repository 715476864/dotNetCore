
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Schany.Core.Dto.Sys;
using Schany.Framework;
using System;
using System.Text;

namespace Schany.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BaseController : ControllerBase
    {
        public CustomerClaimModel loginUser;
        public HttpContext httpContex;
        public string baseUri;

        public BaseController()
        {
            httpContex = MyHttpContext.Current;
            var request = httpContex.Request;
            baseUri = new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .ToString();
            if (request.Host.Port == 80)
            {
                baseUri = baseUri.Replace(":80", "");
            }

            CustomerClaimModel claimModel = new CustomerClaimModel();
            //从Token中取出登陆用户信息
            var _user = httpContex.User.Claims;
            if (_user != null)
            {
                foreach (var item in _user)
                {
                    if (item.Type == "CustomerId")
                    {
                        claimModel.CustomerId = Guid.Parse(item.Value);
                    }
                    if (item.Type == "UserName")
                    {
                        claimModel.UserName = item.Value;
                    }
                    if (item.Type == "TrueName")
                    {
                        claimModel.TrueName = item.Value;
                    }
                }
                loginUser = claimModel;
            }

        }
    }
}