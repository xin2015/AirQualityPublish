using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirQualityPublish.BLL.Models
{
    public class ReturnStatus
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }

        public ReturnStatus(bool status, string message)
        {
            Status = status;
            Message = message;
        }

        public ReturnStatus(string message, Exception exception)
        {
            Status = false;
            Message = message;
            Exception = exception;
        }

        public ReturnStatus(Exception exception)
        {
            Status = false;
            Message = exception.Message;
            Exception = exception;
        }
    }
}
