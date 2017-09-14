using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata;

namespace AirQualityPublish.Model
{
    public class FluentModel : OpenAccessContext, IUnitOfWork
    {
        static string connectionStringName = @"AirQualityPublishConnection";
        static MetadataContainer metadataContainer = new FluentModelMetadataSource().GetModel();
        static BackendConfiguration backendConfiguration = new BackendConfiguration()
        {
            Backend = "MsSql",
            ProviderName = "System.Data.SqlClient"
        };

        public FluentModel() : base(connectionStringName, backendConfiguration, metadataContainer) { }

        public void UpdateSchema()
        {
            var handler = GetSchemaHandler();
            string script = null;
            try
            {
                script = handler.CreateUpdateDDLScript(null);
            }
            catch
            {
                bool throwException = false;
                try
                {
                    handler.CreateDatabase();
                    script = handler.CreateDDLScript();
                }
                catch
                {
                    throwException = true;
                }
                if (throwException)
                    throw;
            }

            if (string.IsNullOrEmpty(script) == false)
            {
                handler.ExecuteDDLScript(script);
            }
        }
    }
}
