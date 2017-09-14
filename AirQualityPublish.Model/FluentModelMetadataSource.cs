using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.OpenAccess.Metadata;
using Telerik.OpenAccess.Metadata.Fluent;

namespace AirQualityPublish.Model
{
    public class FluentModelMetadataSource : FluentMetadataSource
    {
        protected override IList<MappingConfiguration> PrepareMapping()
        {
            List<MappingConfiguration> mappingConfigurations = new List<MappingConfiguration>();
            return mappingConfigurations;
        }

        protected override MetadataContainer CreateModel()
        {
            MetadataContainer container = base.CreateModel();
            container.NameGenerator.RemoveCamelCase = false;
            container.NameGenerator.ResolveReservedWords = false;
            container.NameGenerator.SourceStrategy = NamingSourceStrategy.Property;
            return container;
        }


    }
}
