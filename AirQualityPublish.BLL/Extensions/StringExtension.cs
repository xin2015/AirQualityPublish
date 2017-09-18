using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirQualityPublish.BLL.Extensions
{
    /// <summary>
    /// 字符串扩展类
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 将字符串转换为可空Double类型
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns></returns>
        public static double? ToNullableDouble(this string text)
        {
            double value;
            if (double.TryParse(text, out value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }
    }
}
