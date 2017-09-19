using AirQualityPublish.Model;
using AirQualityPublish.Model.Entities;
using AirQualityPublish.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirQualityPublish.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Telerik.OpenAccess.ServiceHost.ServiceHostManager.StartProfilerService(15555);

            using (FluentModel db = new FluentModel())
            {
                CodeTimeRepository<StationHourMonitorAirQuality> r = new CodeTimeRepository<StationHourMonitorAirQuality>(db);
                string code = "430700051";
                DateTime time = new DateTime(2017, 1, 30);

                while (!Console.KeyAvailable)
                {
                    if (r.GetAll().Count(o => o.Code == code && o.Time == time) != 0)
                    {

                    }
                    if (r.GetAll().Any(o => o.Code == code && o.Time == time))
                    {

                    }
                    if (r.GetAll().FirstOrDefault(o => o.Code == code && o.Time == time) != null)
                    {

                    }
                    if (r.GetAll().Count(o => o.Code == code) != 0)
                    {

                    }
                    if (r.GetAll().Any(o => o.Code == code))
                    {

                    }
                    if (r.GetAll().FirstOrDefault(o => o.Code == code) != null)
                    {

                    }
                    if (r.GetAll().Count(o => o.Time == time) != 0)
                    {

                    }
                    if (r.GetAll().Any(o => o.Time == time))
                    {

                    }
                    if (r.GetAll().FirstOrDefault(o => o.Time == time) != null)
                    {

                    }
                }
            }

            Telerik.OpenAccess.ServiceHost.ServiceHostManager.StopProfilerService();
        }
    }
}
