using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace FeatureToggle.Azure.TableStorage.Providers
{
    public class TableStorageFeatureToggleProvider : IBooleanToggleValueProvider
    {
        private static bool _tableVerified;
        public static TableStorageConfiguration Configuration { get; } = new TableStorageConfiguration();

        public bool EvaluateBooleanToggleValue(IFeatureToggle toggle)
        {
            var table = GetCloudTable();

            // Determine assembly of the feature toggle  (use as partitionkey)
            string componentName = Configuration.PartitionKeyResolver(toggle);
            // Determine featurename (use as rowkey)
            string toggleName = toggle.GetType().Name;

            // Fetch feature toggle
            var retrieveOperation = TableOperation.Retrieve<FeatureToggleEntity>(componentName, toggleName);
            var retrievedResult = table.ExecuteAsync(retrieveOperation).Result;

            bool toogleValue;
            
            if (retrievedResult.Result is FeatureToggleEntity featureToggle)
            {
                toogleValue = featureToggle.Enabled;
            }
            else
            {
                if (Configuration.AutoCreateFeature)
                {
                    var entity = new FeatureToggleEntity(componentName, toggleName);

                    var insertOperation = TableOperation.Insert(entity);
                    table.ExecuteAsync(insertOperation);

                    toogleValue = entity.Enabled;
                }
                else
                {
                    throw new ToggleConfigurationError($"Feature toggle: '{toggleName}' does not exist. Either create it manually or configure the provider to auto create it.");
                }
            }

            return toogleValue;
        }

        private CloudTable GetCloudTable()
        {
            if (string.IsNullOrWhiteSpace(Configuration.ConnectionString))
                throw new ToggleConfigurationError(
                    $"No Azure storage account connection string was found. Please configure {nameof(TableStorageFeatureToggleProvider)} at application startup.");

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
                    throw new ToggleConfigurationError($"Table: '{Configuration.TableName}' does not exist. Either create it manually or configure the provider to auto create it.");
            }
            _tableVerified = true;

            return table;
        }
    }
}
