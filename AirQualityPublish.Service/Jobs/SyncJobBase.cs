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
    class SyncJobBase<TSync> : IJob where TSync : ISync
    {
        ILog logger;

        public SyncJobBase()
        {
            logger = LogManager.GetLogger<SyncJobBase<TSync>>();
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
                    sync.Sync();
                }
                sw.Stop();
                logger.InfoFormat("{0} Sync {1}.", typeof(TSync).Name, sw.Elapsed);
            }
            catch (Exception e)
            {
                logger.Error("Execute failed.", e);
            }
        }
    }
}
