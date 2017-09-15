using AirQualityPublish.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.OpenAccess;

namespace AirQualityPublish.Model.Repositories
{
    public class MissingDataRecordRepository : Repository<MissingDataRecord>
    {
        public MissingDataRecordRepository(OpenAccessContext context) : base(context)
        {
        }

        public void Add(string type, string code, DateTime time, Exception exception)
        {
            Add(new MissingDataRecord()
            {
                Type = type,
                Code = code,
                Time = time,
                CreationTime = DateTime.Now,
                Exception = exception.Message
            });
        }
    }
}
