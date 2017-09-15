﻿using AirQualityPublish.Model.Entities;
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
        static int maxMissTimes;

        static MissingDataRecordRepository()
        {
            maxMissTimes = 30;
        }

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

        public IQueryable<MissingDataRecord> GetList(string type)
        {
            return GetAll().Where(o => o.Type == type && !o.Status && o.MissTimes <= maxMissTimes);
        }
    }
}
