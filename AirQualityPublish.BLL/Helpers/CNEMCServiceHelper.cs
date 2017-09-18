using Modules.Basic.CryptoTransverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirQualityPublish.BLL.Helpers
{
    public class CNEMCServiceHelper
    {
        public static string GetLoginState()
        {
            CNEMCService.LoginUser user = new CNEMCService.LoginUser()
            {
                UserName = ConfigurationManager.AppSettings["UserName"],
                Password = ConfigurationManager.AppSettings["Password"],
                LoginTime = DateTimeOffset.Now
            };
            string jsonData = JsonConvert.SerializeObject(user);
            string state = SymmetricalEncryption.Default.Encrypt(jsonData);
            return state;
        }
    }
}
