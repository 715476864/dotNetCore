using StackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Schany.Framework
{
    public class RedisConfig
    {
        /// <summary>
        /// 创建Redis单例实例
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="preKey"></param>
        public static void CreateInstance(string connStr,string preKey)
        {
            RedisHelper.CreateInstance(connStr, preKey);
        }
    }
}
