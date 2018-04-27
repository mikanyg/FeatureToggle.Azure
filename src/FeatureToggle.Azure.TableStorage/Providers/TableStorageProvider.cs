using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Diagnostics;

namespace FeatureToggle.Azure.Providers
{
    public class TableStorageProvider : IBooleanToggleValueProvider, IDateTimeToggleValueProvider
    {
        private static bool _tableVerified;

        private static TableStorageConfiguration Configuration { get; set; } = new TableStorageConfiguration();

        public static void Configure(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("value cannot be null or empty", nameof(connectionString));
            }

            Configure(new TableStorageConfiguration { ConnectionString = connectionString });
        }

        public static void Configure(TableStorageConfiguration config)
        {
            Configuration = config ?? throw new ArgumentNullException(nameof(config));
        }

        public bool EvaluateBooleanToggleValue(IFeatureToggle toggle)
        {
            return EvaluateToggleValue(toggle, entity => entity.Enabled, (componentName, toggleName) => new BooleanFeatureToggleEntity(componentName, toggleName));
        }

        public DateTime EvaluateDateTimeToggleValue(IFeatureToggle toggle)
        {
            return EvaluateToggleValue(toggle, entity => entity.ToggleTimestamp, (componentName, toggleName) => new DateTimeFeatureToggleEntity(componentName, toggleName));
        }

        private TValue EvaluateToggleValue<TEntity, TValue>(IFeatureToggle toggle, Func<TEntity, TValue> valueSelector, Func<string, string, TEntity> entityCreator) where TEntity : FeatureToggleEntity
        {
            var table = GetCloudTable();

            var componentName = Configuration.PartitionKeyResolver(toggle); // Defaults to assembly name of the feature toggle (used as partitionkey)
            var toggleName = toggle.GetType().Name;

            // Fetch feature toggle
            var retrievedResult = table.ExecuteAsync(
                TableOperation.Retrieve<TEntity>(componentName, toggleName)).Result;

            TValue toggleValue;

            if (retrievedResult.Result is TEntity entity)
            {
                toggleValue = valueSelector(entity);
            }
            else
            {
                Trace.TraceWarning($"Feature toggle '{toggleName}' not found.");

                var newEntity = HandleMissingToggle(toggleName, () => entityCreator(componentName, toggleName), table);
                toggleValue = valueSelector(newEntity);
            }

            return toggleValue;
        }        

        private TEntity HandleMissingToggle<TEntity>(string toggleName, Func<TEntity> entityCreator, CloudTable table) where TEntity : FeatureToggleEntity
        {
            if (!Configuration.AutoCreateFeature)
                throw new ToggleConfigurationError($"Feature toggle '{toggleName}' does not exist. Either create it manually or configure the provider to auto create it.");

            Trace.TraceInformation($"AutoCreateFeature enabled, creating feature toggle '{toggleName}'.");

            var entity = entityCreator();
            var result = table.ExecuteAsync(TableOperation.Insert(entity)).Result;

            return entity;
        }

        private CloudTable GetCloudTable()
        {
            if (string.IsNullOrWhiteSpace(Configuration.ConnectionString))
                throw new ToggleConfigurationError($"Azure Storage account connection string not set. Please configure {nameof(TableStorageProvider)} at application startup.");

            var storageAccount = CloudStorageAccount.Parse(Configuration.ConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(Configuration.TableName);

            if (_tableVerified) return table;

            if (Configuration.AutoCreateTable)
            {
                table.CreateIfNotExistsAsync();
            }
            else
            {
                var exist = table.ExistsAsync().Result;

                if (!exist)
                    throw new ToggleConfigurationError($"Table '{Configuration.TableName}' does not exist. Either create it manually or configure the provider to auto create it.");
            }
            _tableVerified = true;

            return table;
        }        
    }
}
