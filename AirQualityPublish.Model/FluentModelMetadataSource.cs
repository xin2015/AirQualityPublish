using AirQualityPublish.Model.Entities;
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
            mappingConfigurations.Add(GetMissingDataRecordMappingConfiguration());
            mappingConfigurations.Add(GetStationMappingConfiguration());
            mappingConfigurations.Add(GetStationHourMonitorAirQualityMappingConfiguration());
            mappingConfigurations.Add(GetStationDayMonitorAirQualityMappingConfiguration());
            mappingConfigurations.Add(GetCityHourMonitorAirQualityMappingConfiguration());
            mappingConfigurations.Add(GetCityDayMonitorAirQualityMappingConfiguration());
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

        private MappingConfiguration<MissingDataRecord> GetMissingDataRecordMappingConfiguration()
        {
            MappingConfiguration<MissingDataRecord> configuration = new MappingConfiguration<MissingDataRecord>();

            configuration.MapType().ToTable(typeof(MissingDataRecord).Name);

            configuration.HasProperty(x => x.Id).IsIdentity(KeyGenerator.Autoinc);
            configuration.HasProperty(x => x.Type).IsNotNullable().HasColumnType("nvarchar").HasLength(256);
            configuration.HasProperty(x => x.Code).IsNotNullable().HasColumnType("nvarchar").HasLength(64);
            configuration.HasProperty(x => x.Message).HasColumnType("nvarchar(MAX)");
            configuration.HasProperty(x => x.Others).HasColumnType("nvarchar(MAX)");

            return configuration;
        }

        private MappingConfiguration<Station> GetStationMappingConfiguration()
        {
            MappingConfiguration<Station> configuration = new MappingConfiguration<Station>();

            configuration.MapType().ToTable(typeof(Station).Name);

            configuration.HasProperty(x => x.Id).IsIdentity(KeyGenerator.Autoinc);
            configuration.HasProperty(x => x.Code).IsNotNullable().HasColumnType("nvarchar").HasLength(64);
            configuration.HasProperty(x => x.Name).IsNotNullable().HasColumnType("nvarchar").HasLength(128);
            configuration.HasProperty(x => x.EnvPublishCode).IsNotNullable().HasColumnType("nvarchar").HasLength(8);

            return configuration;
        }

        private MappingConfiguration<StationHourMonitorAirQuality> GetStationHourMonitorAirQualityMappingConfiguration()
        {
            MappingConfiguration<StationHourMonitorAirQuality> configuration = new MappingConfiguration<StationHourMonitorAirQuality>();

            configuration.MapType().ToTable(typeof(StationHourMonitorAirQuality).Name);

            configuration.HasProperty(x => x.Code).IsIdentity().IsNotNullable().HasColumnType("nvarchar").HasLength(64);
            configuration.HasProperty(x => x.Time).IsIdentity();
            configuration.HasProperty(x => x.PrimaryPollutant).HasColumnType("nvarchar").HasLength(128);
            configuration.HasProperty(x => x.Type).HasColumnType("nvarchar").HasLength(8);

            return configuration;
        }

        private MappingConfiguration<StationDayMonitorAirQuality> GetStationDayMonitorAirQualityMappingConfiguration()
        {
            MappingConfiguration<StationDayMonitorAirQuality> configuration = new MappingConfiguration<StationDayMonitorAirQuality>();

            configuration.MapType().ToTable(typeof(StationDayMonitorAirQuality).Name);

            configuration.HasProperty(x => x.Code).IsIdentity().IsNotNullable().HasColumnType("nvarchar").HasLength(64);
            configuration.HasProperty(x => x.Time).IsIdentity();
            configuration.HasProperty(x => x.PrimaryPollutant).HasColumnType("nvarchar").HasLength(128);
            configuration.HasProperty(x => x.Type).HasColumnType("nvarchar").HasLength(8);

            return configuration;
        }

        private MappingConfiguration<CityHourMonitorAirQuality> GetCityHourMonitorAirQualityMappingConfiguration()
        {
            MappingConfiguration<CityHourMonitorAirQuality> configuration = new MappingConfiguration<CityHourMonitorAirQuality>();

            configuration.MapType().ToTable(typeof(CityHourMonitorAirQuality).Name);

            configuration.HasProperty(x => x.Code).IsIdentity().IsNotNullable().HasColumnType("nvarchar").HasLength(64);
            configuration.HasProperty(x => x.Time).IsIdentity();
            configuration.HasProperty(x => x.PrimaryPollutant).HasColumnType("nvarchar").HasLength(128);
            configuration.HasProperty(x => x.Type).HasColumnType("nvarchar").HasLength(8);

            return configuration;
        }

        private MappingConfiguration<CityDayMonitorAirQuality> GetCityDayMonitorAirQualityMappingConfiguration()
        {
            MappingConfiguration<CityDayMonitorAirQuality> configuration = new MappingConfiguration<CityDayMonitorAirQuality>();

            configuration.MapType().ToTable(typeof(CityDayMonitorAirQuality).Name);

            configuration.HasProperty(x => x.Code).IsIdentity().IsNotNullable().HasColumnType("nvarchar").HasLength(64);
            configuration.HasProperty(x => x.Time).IsIdentity();
            configuration.HasProperty(x => x.PrimaryPollutant).HasColumnType("nvarchar").HasLength(128);
            configuration.HasProperty(x => x.Type).HasColumnType("nvarchar").HasLength(8);

            return configuration;
        }
    }
}
