using AirQualityPublish.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.OpenAccess;

namespace AirQualityPublish.Model.Repositories
{
    public class StationRepository : Repository<Station>
    {
        public StationRepository(OpenAccessContext context) : base(context)
        {
        }

        public IQueryable<Station> GetList()
        {
            return GetAll().Where(o => o.Status);
        }
    }
}
