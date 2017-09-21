using AirQualityPublish.Model.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.OpenAccess;

namespace AirQualityPublish.Model.Repositories
{
    public class MissingDataRecordRepository : Repository<MissingDataRecord>
    {
        static int maxMissTimes;

        static MissingDataRecordRepository()
        {
            if (!int.TryParse(ConfigurationManager.AppSettings["MaxMissTimes"], out maxMissTimes))
            {
                maxMissTimes = 30;
            }
        }

        public MissingDataRecordRepository(OpenAccessContext context) : base(context)
        {
        }

        public void Add(string type, string code, DateTime time, string message)
        {
            Add(new MissingDataRecord()
            {
                Type = type,
                Code = code,
                Time = time,
                CreationTime = DateTime.Now,
                Message = message
            });
        }

        public MissingDataRecord Get(string type, string code, DateTime time)
        {
            return GetAll().FirstOrDefault(o => o.Type == type && o.Code == code && o.Time == time);
        }

        public IQueryable<MissingDataRecord> GetList(string type)
        {
            return GetAll().Where(o => o.Type == type && !o.Status && o.MissTimes <= maxMissTimes);
        }

        public IQueryable<MissingDataRecord> GetList(int[] ids)
        {
            return GetAll().Where(o => ids.Contains(o.Id));
        }
    }
}
