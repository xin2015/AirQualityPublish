using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirQualityPublish.Model.Entities
{
    public class CityHourMonitorAirQuality : IMonitorAirQualityEntity
    {
        public string Code { get; set; }
        public DateTime Time { get; set; }
        public double? SO2 { get; set; }
        public double? NO2 { get; set; }
        public double? PM10 { get; set; }
        public double? CO { get; set; }
        public double? O3 { get; set; }
        public double? PM25 { get; set; }
        public double? AQI { get; set; }
        public string PrimaryPollutant { get; set; }
        public string Type { get; set; }
    }
}
