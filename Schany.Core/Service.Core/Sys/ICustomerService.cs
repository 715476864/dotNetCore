using Schany.Core.Dto.Sys;
using Schany.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Schany.Core.Service.Core.Sys
{
    public interface ICustomerService
    {
        List<CustomerListModel> GetList();

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        CustomerClaimModel Login(string userName,string password);
    }
}
