using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Schany.Data.Enums
{
    /// <summary>
    /// 开放平台类型
    /// </summary>
    public enum OpenPlateType
    {
        [Description("微信公众平台")]
        WxOpenPlate = 1,
        [Description("微信小程序")]
        WxMiniprograme = 2,
    }
}
