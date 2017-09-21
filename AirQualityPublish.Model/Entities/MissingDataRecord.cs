using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirQualityPublish.Model.Entities
{
    public class MissingDataRecord : IEntity<int>, ICodeTime
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public DateTime Time { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ModificationTime { get; set; }
        public bool Status { get; set; }
        public int MissTimes { get; set; }
        public string Message { get; set; }
    }
}
