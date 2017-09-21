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
            //Telerik.OpenAccess.ServiceHost.ServiceHostManager.StartProfilerService(15555);

            using (FluentModel db = new FluentModel())
            {
                db.UpdateSchema();
            }

            //Telerik.OpenAccess.ServiceHost.ServiceHostManager.StopProfilerService();
        }
    }
}
