using AirQualityPublish.Model.Entities;
using AirQualityPublish.Model.Repositories;
using Common.Logging;
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
            return new List<StationHourMonitorAirQuality>();
        }
    }
}
