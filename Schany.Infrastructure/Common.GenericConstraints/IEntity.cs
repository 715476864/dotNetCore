using System;
using System.Collections.Generic;
using System.Text;

namespace Schany.Infrastructure.Common.GenericConstraints
{
    /// <summary>
    /// 实体约束接口
    /// </summary>
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
