using AirQualityPublish.BLL.Extensions;
using AirQualityPublish.BLL.Helpers;
using AirQualityPublish.Model.Entities;
using AirQualityPublish.Model.Repositories;
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
    public class StationDayMonitorAirQualitySync : SyncBase<StationDayMonitorAirQuality>
    {
        protected Station[] Stations { get; set; }

        public StationDayMonitorAirQualitySync(OpenAccessContext context) : base(context)
        {
            Logger = LogManager.GetLogger<StationDayMonitorAirQualitySync>();
            Interval = TimeSpan.FromDays(1);
            Stations = new StationRepository(context).GetList().ToArray();
        }

        protected override DateTime GetTime()
        {
            return DateTime.Today.AddDays(-1);
        }

        protected override string[] GetCodes(DateTime time)
        {
            return Stations.Select(o => o.Code).ToArray();
        }

        protected override List<StationDayMonitorAirQuality> GetSyncData(string code, DateTime time)
        {
            List<StationDayMonitorAirQuality> list = new List<StationDayMonitorAirQuality>();
            Station station = Stations.First(o => o.Code == code);
            using (CNEMCService.CNEMCServiceClient client = new CNEMCService.CNEMCServiceClient())
            {
                string key = client.Login(CNEMCServiceHelper.GetLoginState());
                CNEMCService.AQIDataPublishLive src = client.GetAQIDataPublishLive(key, station.EnvPublishCode, time);
                if (src != null)
                {
                    StationDayMonitorAirQuality data = new StationDayMonitorAirQuality()
                    {
                        Code = code,
                        Time = time,
                        SO2 = src.SO2_24h.ToNullableDouble(),
                        NO2 = src.NO2_24h.ToNullableDouble(),
                        PM10 = src.PM10_24h.ToNullableDouble(),
                        CO = src.CO.ToNullableDouble(),
                        O3 = src.O3_8h.ToNullableDouble(),
                        PM25 = src.PM2_5_24h.ToNullableDouble()
                    };
                    DayAQICalculator calculator = new DayAQICalculator();
                    calculator.CalculateAQI(data);
                    list.Add(data);
                }
            }
            return list;
        }

        protected override List<StationDayMonitorAirQuality> GetCoverData(string code, DateTime time)
        {
            List<StationDayMonitorAirQuality> list = new List<StationDayMonitorAirQuality>();
            Station station = Stations.First(o => o.Code == code);
            using (CNEMCService.CNEMCServiceClient client = new CNEMCService.CNEMCServiceClient())
            {
                string key = client.Login(CNEMCServiceHelper.GetLoginState());
                CNEMCService.AQIDataPublishHistory src = client.GetAQIDataPublishHistory(key, station.EnvPublishCode, time);
                if (src != null)
                {
                    StationDayMonitorAirQuality data = new StationDayMonitorAirQuality()
                    {
                        Code = code,
                        Time = time,
                        SO2 = src.SO2_24h.ToNullableDouble(),
                        NO2 = src.NO2_24h.ToNullableDouble(),
                        PM10 = src.PM10_24h.ToNullableDouble(),
                        CO = src.CO.ToNullableDouble(),
                        O3 = src.O3_8h.ToNullableDouble(),
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
