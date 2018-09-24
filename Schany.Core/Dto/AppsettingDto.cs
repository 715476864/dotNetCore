using System;
using System.Collections.Generic;
using System.Text;

namespace Schany.Core.Dto
{
    /// <summary>
    /// 该数据传输对象主要用于读取appsetting.json
    /// </summary>
    public class AppsettingDataModel
    {
        public Database Database { get; set; }
        public Redis Redis { get; set; }
    }

    public class Database
    {
        public string ConnectionString { get; set; }
    }

    public class Redis
    {
        public string Host { get; set; }
        public string PreKey { get; set; }
    }
}
