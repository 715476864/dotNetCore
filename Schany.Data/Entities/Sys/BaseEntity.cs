
using Schany.Infrastructure.Common.GenericConstraints;
using Schany.Infrastructure.Common.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace Schany.Data.Entities.Sys
{
    /// <summary>
    /// 抽象基类
    /// </summary>
    public abstract class BaseEntity: IEntity
    {
        public BaseEntity()
        {
            //构造函数初始化ID
            this.Id = CombGuidHelper.NewCombGuid();
            //构造函数初始化CreateTime
            this.CreateTime = DateTime.Now;
        }
        
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [Display(Name = "主键")]
        public Guid Id { get; set; }

        /// <summary>
        /// 是否已逻辑删除
        /// </summary>
        [Display(Name = "是否已逻辑删除")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>
        [Display(Name = "创建人Id")]
        public Guid CreateUserId { get; set; }
    }
}
