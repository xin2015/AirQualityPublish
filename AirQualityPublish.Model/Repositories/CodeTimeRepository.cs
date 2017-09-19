using AirQualityPublish.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.OpenAccess;

namespace AirQualityPublish.Model.Repositories
{
    public class CodeTimeRepository<TEntity> : Repository<TEntity> where TEntity : ICodeTimeEntity
    {
        public CodeTimeRepository(OpenAccessContext context) : base(context)
        {
        }

        public bool Contains(string code, DateTime time)
        {
            return GetAll().Any(o => o.Code == code && o.Time == time);
        }

        public bool Contains(DateTime time)
        {
            return GetAll().Any(o => o.Time == time);
        }
    }
}
