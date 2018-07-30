using FeatureToggle.Azure.Providers;
using Microsoft.Azure.Documents.Client;
using NUnit.Framework;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace FeatureToggle.Azure.DocumentDB.Test
{
    [TestFixture]
    public abstract class DocumentDbTestFixture
    {
        private string databaseId;
        private string collectionId;
        private DocumentClient client;

        [SetUp]
        public void FixtureSetup()
        {
            client = new DocumentClient(new Uri(TestConfig.ValidEndpoint), TestConfig.ValidAuthKey);            

            databaseId = $"FeatureToggle_{Guid.NewGuid()}_Test";
            collectionId = $"Toggles_{Guid.NewGuid()}_Test";

            ResetCollectionValidationInProvider();
        }

        [TearDown]
        public async Task Cleanup()
        {
            try
            {
                await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri("FeatureToggle", "Toggles", nameof(TestFeatureToggle)));
            }
            catch { }

            try
            {
                await client.DeleteDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId));
            }
            catch { }

            try
            {
                await client.DeleteDatabaseAsync(UriFactory.CreateDatabaseUri(databaseId));
            }
            catch { }
        }

        protected void AutoCreateToggle()
        {
            ConfigureProvider(true, true);

            var toggle = new TestFeatureToggle();
            var unused = toggle.FeatureEnabled; // Auto creates the database and collection            
        }

        protected void ConfigureProvider(bool autoCreateDbAndCollection = false, bool autoCreateToggle = false)
        {
            var config = TestConfig.ValidConfig(databaseId, collectionId);
            config.AutoCreateDatabaseAndCollection = autoCreateDbAndCollection;
            config.AutoCreateFeature = autoCreateToggle;

            DocumentDbProvider.Configure(config);
        }

        protected async Task UpdateToggleDocument(object document)
        {
            await client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId), document, disableAutomaticIdGeneration: true);
        }

        protected static void ResetCollectionValidationInProvider()
        {
            // Reset docdb collection validation in provider
            var field = typeof(DocumentDbProvider).GetField("_collectionVerified", BindingFlags.Static | BindingFlags.NonPublic);
            field.SetValue(null, false);
        }
    }
}
