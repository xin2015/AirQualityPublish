﻿using AirQualityPublish.BLL.Extensions;
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
    public class StationHourMonitorAirQualitySync : SyncBase<StationHourMonitorAirQuality>
    {
        protected Station[] Stations { get; set; }

        public StationHourMonitorAirQualitySync(OpenAccessContext context) : base(context)
        {
            Logger = LogManager.GetLogger<StationHourMonitorAirQualitySync>();
            Interval = TimeSpan.FromHours(1);
            Stations = new StationRepository(context).GetList().ToArray();
        }

        protected override DateTime GetTime()
        {
            return DateTime.Today.AddHours(DateTime.Now.Hour);
        }

        protected override string[] GetCodes(DateTime time)
        {
            return Stations.Select(o => o.Code).ToArray();
        }

        protected override List<StationHourMonitorAirQuality> GetSyncData(string code, DateTime time)
        {
            List<StationHourMonitorAirQuality> list = new List<StationHourMonitorAirQuality>();
            Station station = Stations.First(o => o.Code == code);
            using (CNEMCService.CNEMCServiceClient client = new CNEMCService.CNEMCServiceClient())
            {
                string key = client.Login(CNEMCServiceHelper.GetLoginState());
                CNEMCService.AQIDataPublishLive src = client.GetAQIDataPublishLive(key, station.EnvPublishCode, time);
                if (src != null)
                {
                    StationHourMonitorAirQuality data = new StationHourMonitorAirQuality()
                    {
                        Code = code,
                        Time = time,
                        SO2 = src.SO2.ToNullableDouble(),
                        NO2 = src.NO2.ToNullableDouble(),
                        PM10 = src.PM10.ToNullableDouble(),
                        CO = src.CO.ToNullableDouble(),
                        O3 = src.O3_24h.ToNullableDouble(),
                        PM25 = src.PM2_5.ToNullableDouble()
                    };
                    HourAQICalculator calculator = new HourAQICalculator();
                    calculator.CalculateAQI(data);
                    list.Add(data);
                }
            }
            return list;
        }

        protected override List<StationHourMonitorAirQuality> GetCoverData(string code, DateTime time)
        {
            List<StationHourMonitorAirQuality> list = new List<StationHourMonitorAirQuality>();
            Station station = Stations.First(o => o.Code == code);
            using (CNEMCService.CNEMCServiceClient client = new CNEMCService.CNEMCServiceClient())
            {
                string key = client.Login(CNEMCServiceHelper.GetLoginState());
                CNEMCService.AQIDataPublishHistory src = client.GetAQIDataPublishHistory(key, station.EnvPublishCode, time);
                if (src != null)
                {
                    StationHourMonitorAirQuality data = new StationHourMonitorAirQuality()
                    {
                        Code = code,
                        Time = time,
                        SO2 = src.SO2.ToNullableDouble(),
                        NO2 = src.NO2.ToNullableDouble(),
                        PM10 = src.PM10.ToNullableDouble(),
                        CO = src.CO.ToNullableDouble(),
                        O3 = src.O3_24h.ToNullableDouble(),
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
