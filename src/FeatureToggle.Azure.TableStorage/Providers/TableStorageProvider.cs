using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace FeatureToggle.Azure.TableStorage.Providers
{
    public class TableStorageProvider : IBooleanToggleValueProvider
    {
        private static bool _tableVerified;
        public static TableStorageConfiguration Configuration { get; } = new TableStorageConfiguration();

        public bool EvaluateBooleanToggleValue(IFeatureToggle toggle)
        {
            if (string.IsNullOrWhiteSpace(Configuration.ConnectionString))
                throw new ToggleConfigurationError(
                    $"No Azure storage account connection string was found. Please configure {nameof(TableStorageProvider)}.{nameof(Configuration.ConnectionString)} at application startup.");

            var storageAccount = CloudStorageAccount.Parse(Configuration.ConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(Configuration.TableName);
            
            if (!_tableVerified)
            {
                if (Configuration.AutoCreateTable)
                {
                    table.CreateIfNotExistsAsync();
                }
                else
                {
                    var exist = table.ExistsAsync().Result;

                    if(!exist)
                        throw new ToggleConfigurationError($"Table: {Configuration.TableName} does not exist. Create it manually or configure the provider to auto create it.");
                }
                _tableVerified = true;
            }

            // Determine assembly of the feature toggle  (use as partitionkey)
            string componentName = Configuration.PartitionKeyResolver(toggle);
            // Determine featurename (use as rowkey)
            string toggleName = toggle.GetType().Name;

            // Fetch feature toggle
            var retrieveOperation = TableOperation.Retrieve<FeatureToggleEntity>(componentName, toggleName);
            var retrievedResult = table.ExecuteAsync(retrieveOperation).Result;

            // Return feature toggle
            if (retrievedResult.Result is FeatureToggleEntity featureToggle)
            {
                return featureToggle.Enabled;
            }
            else
            {
                if (Configuration.AutoCreateFeature)
                {
                    var entity = new FeatureToggleEntity(componentName, toggleName);

                    var insertOperation = TableOperation.Insert(entity);
                    table.ExecuteAsync(insertOperation);

                    return entity.Enabled;
                }
                else
                {
                    throw new ToggleConfigurationError("Could not find any feature toggles");
                }
            }
        }
    }
}
