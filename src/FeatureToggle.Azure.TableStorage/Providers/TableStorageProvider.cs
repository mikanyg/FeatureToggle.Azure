using System;
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
        private static bool _tableExistVerified;
        public static string ConnectionString { get; set; }

        public static string TableName { get; set; } = "FeatureToggles";

        public bool EvaluateBooleanToggleValue(IFeatureToggle toggle)
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
                throw new ToggleConfigurationError(
                    $"No Azure storage account connection string was found. Please configure {nameof(TableStorageProvider)}.{nameof(ConnectionString)} at application startup.");

            var storageAccount = CloudStorageAccount.Parse(ConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(TableName);
            
            if (!_tableExistVerified)
            {
                table.CreateIfNotExistsAsync();
                _tableExistVerified = true;
            }

            // Determine assembly of the feature toggle  (use as partitionkey)
            string component = Assembly.GetAssembly(toggle.GetType()).GetName().Name;
            // Determine featurename (use as rowkey)
            string feature = toggle.GetType().Name;

            // Fetch feature toggle
            var retrieveOperation = TableOperation.Retrieve<FeatureToggleEntity>(component, feature);
            var retrievedResult = table.ExecuteAsync(retrieveOperation).Result;

            // Return feature toggle
            if (retrievedResult.Result is FeatureToggleEntity featureToggle)
            {
                return featureToggle.Enabled;
            }
            else
            {
                throw new ToggleConfigurationError("Could not find any feature toggles");
            }
        }
    }
}
