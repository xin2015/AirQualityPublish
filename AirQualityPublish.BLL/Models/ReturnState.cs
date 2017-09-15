using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirQualityPublish.BLL.Models
{
    public class ReturnState
    {
        public string State { get; set; }
        public string Message { get; set; }

        public ReturnState(string state, string message)
        {
            State = state;
            Message = message;
        }
    }
}
