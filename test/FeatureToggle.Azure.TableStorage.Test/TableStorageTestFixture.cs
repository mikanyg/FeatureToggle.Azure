using FeatureToggle.Azure.Providers;
using FeatureToggle.Azure.TableStorage.Test.Toggles;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using NUnit.Framework;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace FeatureToggle.Azure.TableStorage.Test
{
    [TestFixture]
    public abstract class TableStorageTestFixture
    {
        private string tableName;
        private CloudTable table;
        private CloudTableClient tableClient;
        protected string partitionKey;

        [SetUp]
        public void FixtureSetup()
        {
            tableName = $"TestFeatureToggles{Guid.NewGuid().ToString().Replace("-", string.Empty)}";
            partitionKey = typeof(TestFeatureToggle).Assembly.GetName().Name;

            var storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            tableClient = storageAccount.CreateCloudTableClient();
            table = tableClient.GetTableReference(tableName);

            ResetTableValidationInProvider();
        }

        [TearDown]
        public async Task Cleanup()
        {
            var x = await table.DeleteIfExistsAsync();

            var defaultTable = tableClient.GetTableReference(new TableStorageConfiguration().TableName);
            var z = await defaultTable.DeleteIfExistsAsync();
        }

        protected void AutoCreateToggle<T>() where T : IFeatureToggle, new()
        {
            ConfigureProvider(true, true);

            var toggle = new T();
            var unused = toggle.FeatureEnabled; // Auto creates the database and collection            
        }

        protected void ConfigureProvider(bool autoCreateTable = false, bool autoCreateToggle = false)
        {
            var config = new TableStorageConfiguration { ConnectionString = "UseDevelopmentStorage=true" };
            config.AutoCreateTable = autoCreateTable;
            config.AutoCreateFeature = autoCreateToggle;
            config.TableName = tableName;

            TableStorageProvider.Configure(config);
        }

        protected async Task UpdateToggleEntity(ITableEntity entity)
        {            
            var result = await table.ExecuteAsync(TableOperation.InsertOrMerge(entity));
        }

        protected static void ResetTableValidationInProvider()
        {
            // Reset docdb collection validation in provider
            var field = typeof(TableStorageProvider).GetField("_tableVerified", BindingFlags.Static | BindingFlags.NonPublic);
            field.SetValue(null, false);
        }
    }
}
