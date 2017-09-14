using Modules.AQE.AQI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirQualityPublish.Model.Entities
{
    public interface IMonitorAirQuality : IEntity, ICodeTime, IAQICalculate
    {
    }
}
