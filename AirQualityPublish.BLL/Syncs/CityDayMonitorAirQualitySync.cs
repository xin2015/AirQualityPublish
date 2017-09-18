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
    public class CityDayMonitorAirQualitySync : SyncBase<CityDayMonitorAirQuality>
    {
        public CityDayMonitorAirQualitySync(OpenAccessContext context) : base(context)
        {
            Logger = LogManager.GetLogger<CityDayMonitorAirQualitySync>();
            Interval = TimeSpan.FromDays(1);
        }

        protected override DateTime GetTime()
        {
            return DateTime.Today.AddDays(-1);
        }

        protected override string[] GetCodes(DateTime time)
        {
            return new string[] { SystemConfig.CityCode };
        }

        protected override List<CityDayMonitorAirQuality> GetSyncData(string code, DateTime time)
        {
            List<CityDayMonitorAirQuality> list = new List<CityDayMonitorAirQuality>();
            using (CNEMCService.CNEMCServiceClient client = new CNEMCService.CNEMCServiceClient())
            {
                string key = client.Login(CNEMCServiceHelper.GetLoginState());
                CNEMCService.CityDayAQIPublishLive src = client.GetCityDayAQIPublishLive(key, code, time);
                if (src != null)
                {
                    CityDayMonitorAirQuality data = new CityDayMonitorAirQuality()
                    {
                        Code = code,
                        Time = time,
                        SO2 = src.SO2_24h.ToNullableDouble(),
                        NO2 = src.NO2_24h.ToNullableDouble(),
                        PM10 = src.PM10_24h.ToNullableDouble(),
                        CO = src.CO_24h.ToNullableDouble(),
                        O3 = src.O3_8h_24h.ToNullableDouble(),
                        PM25 = src.PM2_5_24h.ToNullableDouble()
                    };
                    DayAQICalculator calculator = new DayAQICalculator();
                    calculator.CalculateAQI(data);
                    list.Add(data);
                }
            }
            return list;
        }

        protected override List<CityDayMonitorAirQuality> GetCoverData(string code, DateTime time)
        {
            List<CityDayMonitorAirQuality> list = new List<CityDayMonitorAirQuality>();
            using (CNEMCService.CNEMCServiceClient client = new CNEMCService.CNEMCServiceClient())
            {
                string key = client.Login(CNEMCServiceHelper.GetLoginState());
                CNEMCService.CityDayAQIPublishHistory src = client.GetCityDayAQIPublishHistory(key, code, time);
                if (src != null)
                {
                    CityDayMonitorAirQuality data = new CityDayMonitorAirQuality()
                    {
                        Code = code,
                        Time = time,
                        SO2 = src.SO2_24h.ToNullableDouble(),
                        NO2 = src.NO2_24h.ToNullableDouble(),
                        PM10 = src.PM10_24h.ToNullableDouble(),
                        CO = src.CO_24h.ToNullableDouble(),
                        O3 = src.O3_8h_24h.ToNullableDouble(),
                        PM25 = src.PM2_5_24h.ToNullableDouble()
                    };
                    DayAQICalculator calculator = new DayAQICalculator();
                    calculator.CalculateAQI(data);
                    list.Add(data);
                }
            }
            return list;
        }
    }
}
