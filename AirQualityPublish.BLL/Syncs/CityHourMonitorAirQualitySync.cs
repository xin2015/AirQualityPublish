using AirQualityPublish.BLL.Extensions;
using AirQualityPublish.BLL.Helpers;
using AirQualityPublish.BLL.Models;
using AirQualityPublish.Model.Entities;
using Common.Logging;
using Modules.AQE.AQI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.OpenAccess;

namespace AirQualityPublish.BLL.Syncs
{
    public class CityHourMonitorAirQualitySync : SyncBase<CityHourMonitorAirQuality>
    {
        public CityHourMonitorAirQualitySync(OpenAccessContext context) : base(context)
        {
            Logger = LogManager.GetLogger<CityHourMonitorAirQualitySync>();
            Interval = TimeSpan.FromHours(1);
        }

        protected override DateTime GetTime()
        {
            return DateTime.Today.AddHours(DateTime.Now.Hour);
        }

        protected override string[] GetCodes(DateTime time)
        {
            return new string[] { SystemConfig.CityCode };
        }

        protected override List<CityHourMonitorAirQuality> GetSyncData(string code, DateTime time)
        {
            List<CityHourMonitorAirQuality> list = new List<CityHourMonitorAirQuality>();
            using (CNEMCService.CNEMCServiceClient client = new CNEMCService.CNEMCServiceClient())
            {
                string key = client.Login(CNEMCServiceHelper.GetLoginState());
                CNEMCService.CityAQIPublishLive src = client.GetCityAQIPublishLive(key, code, time);
                if (src != null)
                {
                    CityHourMonitorAirQuality data = new CityHourMonitorAirQuality()
                    {
                        Code = code,
                        Time = time,
                        SO2 = src.SO2.ToNullableDouble(),
                        NO2 = src.NO2.ToNullableDouble(),
                        PM10 = src.PM10.ToNullableDouble(),
                        CO = src.CO.ToNullableDouble(),
                        O3 = src.O3.ToNullableDouble(),
                        PM25 = src.PM2_5.ToNullableDouble()
                    };
                    HourAQICalculator calculator = new HourAQICalculator();
                    calculator.CalculateAQI(data);
                    list.Add(data);
                }
            }
            return list;
        }

        protected override List<CityHourMonitorAirQuality> GetCoverData(string code, DateTime time)
        {
            List<CityHourMonitorAirQuality> list = new List<CityHourMonitorAirQuality>();
            using (CNEMCService.CNEMCServiceClient client = new CNEMCService.CNEMCServiceClient())
            {
                string key = client.Login(CNEMCServiceHelper.GetLoginState());
                CNEMCService.CityAQIPublishHistory src = client.GetCityAQIPublishHistory(key, code, time);
                if (src != null)
                {
                    CityHourMonitorAirQuality data = new CityHourMonitorAirQuality()
                    {
                        Code = code,
                        Time = time,
                        SO2 = src.SO2.ToNullableDouble(),
                        NO2 = src.NO2.ToNullableDouble(),
                        PM10 = src.PM10.ToNullableDouble(),
                        CO = src.CO.ToNullableDouble(),
                        O3 = src.O3.ToNullableDouble(),
                        PM25 = src.PM2_5.ToNullableDouble()
                    };
                    HourAQICalculator calculator = new HourAQICalculator();
                    calculator.CalculateAQI(data);
                    list.Add(data);
                }
            }
            return list;
        }
    }
}
