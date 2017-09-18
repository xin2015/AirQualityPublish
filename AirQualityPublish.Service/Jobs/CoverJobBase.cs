using AirQualityPublish.BLL.Syncs;
using AirQualityPublish.Model;
using Common.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirQualityPublish.Service.Jobs
{
    class CoverJobBase<TSync> : IJob where TSync : ISync
    {
        ILog logger;

        public CoverJobBase()
        {
            logger = LogManager.GetLogger<CoverJobBase<TSync>>();
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                using (FluentModel db = new FluentModel())
                {
                    ISync sync = (ISync)Activator.CreateInstance(typeof(TSync), db);
                    sync.Cover();
                }
                sw.Stop();
                logger.InfoFormat("{0} Cover {1}.", typeof(TSync).Name, sw.Elapsed);
            }
            catch (Exception e)
            {
                logger.Error("Execute failed.", e);
            }
        }
    }
}
