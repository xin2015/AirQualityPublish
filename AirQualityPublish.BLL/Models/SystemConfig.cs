using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirQualityPublish.BLL.Models
{
    public class SystemConfig
    {
        public static string CityCode { get; set; }
        public static string CityName { get; set; }

        static SystemConfig()
        {
            CityCode = ConfigurationManager.AppSettings["CityCode"];
            CityName = ConfigurationManager.AppSettings["CityName"];
        }
    }
}
