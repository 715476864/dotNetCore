using Schany.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Schany.Core.Dto.Sys
{
    /// <summary>
    /// 用户证件单元字段模型
    /// </summary>
    public class CustomerClaimModel
    {
        public Guid CustomerId { get; set; }
        public string UserName { get; set; }
        public string TrueName { get; set; }
        public string Pic { get; set; }
        public Notification Notice { get; set; }
    }
    public class CustomerListModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }        
        public string Password { get; set; }     
        public string TrueName { get; set; }       
        public string Pic { get; set; }       
        public DateTime? LastLoginTime { get; set; }
        public int? LoginErrorTimes { get; set; }        
        public bool? IsLocked { get; set; }
    }
}
