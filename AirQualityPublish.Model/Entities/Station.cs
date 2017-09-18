using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirQualityPublish.Model.Entities
{
    public class Station : IEntity<int>
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string EnvPublishCode { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public bool Status { get; set; }
    }
}
