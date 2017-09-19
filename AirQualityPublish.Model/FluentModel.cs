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
        private static string connectionStringName = @"AirQualityPublishConnection";
        private static MetadataContainer metadataContainer = new FluentModelMetadataSource().GetModel();
        private static BackendConfiguration backendConfiguration = GetBackendConfiguration();

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

        public static BackendConfiguration GetBackendConfiguration()
        {
            BackendConfiguration backend = new BackendConfiguration();
            backend.Backend = "MsSql";
            backend.ProviderName = "System.Data.SqlClient";

            backend.Logging.LogEvents = LoggingLevel.Normal;
            backend.Logging.StackTrace = true;
            backend.Logging.EventStoreCapacity = 10000;
            backend.Logging.MetricStoreCapacity = 3600;
            backend.Logging.MetricStoreSnapshotInterval = 1000;
            backend.Logging.Downloader.EventPollSeconds = 1;
            backend.Logging.Downloader.MetricPollSeconds = 1;

            return backend;
        }
    }
}
