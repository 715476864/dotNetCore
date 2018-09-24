
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Options;
using Schany.Core.Dto;
using Schany.Core.Dto.Sys;
using Schany.Core.Repository;
using Schany.Data.Entities.Sys;
using Schany.Infrastructure;
using Schany.Infrastructure.Common.Helpers;

namespace Schany.Core.Service.Core.Sys.Impl
{
    public class CustomerService : ICustomerService
    {
        private IRepository<Customer> customerRepository;
        private AppsettingDataModel appsetting;

        public CustomerService(IRepository<Customer> _customerRepository, IOptions<AppsettingDataModel> option)
        {
            customerRepository = _customerRepository;
            appsetting = option.Value;
        }


        public List<CustomerListModel> GetList()
        {
            
            var results = customerRepository.Entities.Where(r => !r.IsDeleted).Select(r => new CustomerListModel()
            {
                Id = r.Id,
                UserName = r.UserName,
                Password = r.Password,
                TrueName = r.TrueName
            }).ToList();
            return results;
        }

        #region 用户登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public CustomerClaimModel Login(string userName, string password)
        {
            CustomerClaimModel claimModel = new CustomerClaimModel();
            if (!customerRepository.CheckExists(r => !r.IsDeleted & r.UserName == userName))
            {
                //用户名不存在
                claimModel.Notice = new Notification(NotifyType.Error, "用户不存在。");
            }
            else
            {
                //用户名存在
                string _password = HashHelper.GetMd5(password);
                var customer = customerRepository.Entities.Where(r => !r.IsDeleted
                      & r.UserName == userName
                      & r.Password == _password).FirstOrDefault();

                if (customer == null)
                {
                    //用户名密码校验失败
                    StringBuilder errorMsg = new StringBuilder();
                    errorMsg.Append("用户名或密码错误。");
                    var _customer = customerRepository.Entities.Where(r => !r.IsDeleted & r.UserName == userName).FirstOrDefault();
                    if (_customer.LoginErrorTimes < 0)
                    {
                        _customer.LoginErrorTimes = 1;
                        errorMsg.Append("您已经登录错误1次，还有4次登录机会。");
                    }
                    else if (_customer.LoginErrorTimes >= 0 & _customer.LoginErrorTimes < 4)
                    {
                        errorMsg.Append(string.Format("你已经登录错误{0}次，还有{1}次登录机会。", Convert.ToString(_customer.LoginErrorTimes + 1), Convert.ToString(4 - _customer.LoginErrorTimes)));
                        _customer.LoginErrorTimes++;
                    }
                    else
                    {
                        _customer.LoginErrorTimes++;
                        _customer.IsLocked = true;
                        errorMsg.Append("您已经登录错误5次，账户已被锁定。");
                    }
                    _customer.LastLoginTime = DateTime.Now;
                    claimModel.Notice = new Notification(NotifyType.Error, errorMsg.ToString());

                    customerRepository.Update(_customer);
                }
                else
                {
                    //用户名密码校验成功
                    if (customer.IsLocked)
                    {
                        //用户已经被锁定
                        claimModel.Notice = new Notification(NotifyType.Error, "用户已经被锁定。");
                    }
                    else
                    {
                        //用户状态正常
                        claimModel.CustomerId = customer.Id;
                        claimModel.UserName = customer.UserName;
                        claimModel.TrueName = customer.TrueName;
                        claimModel.Pic = customer.Pic;
                        claimModel.Notice = new Notification(NotifyType.Success, "登录成功。");

                        customer.LoginErrorTimes = 0;
                    }
                    customer.LastLoginTime = DateTime.Now;
                    customerRepository.Update(customer);
                }
            }

            return claimModel;
        }
        #endregion
    }
}
