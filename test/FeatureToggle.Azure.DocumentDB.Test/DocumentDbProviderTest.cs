using FeatureToggle.Azure.Providers;
using Microsoft.Azure.Documents.Client;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace FeatureToggle.Azure.DocumentDB.Test
{
    [TestFixture]
    public class DocumentDbProviderTest
    {
        private string databaseId;
        private string collectionId;
        private DocumentClient client;

        [SetUp]
        public void Setup()
        {
            client = new DocumentClient(new Uri(TestConfig.ValidEndpoint), TestConfig.ValidAuthKey);

            databaseId = $"FeatureToggle_{Guid.NewGuid()}_Test";
            collectionId = $"Toggles_{Guid.NewGuid()}_Test";

            // Reset docdb collection validation in provider
            var field = typeof(DocumentDbProvider).GetField("_collectionVerified", BindingFlags.Static | BindingFlags.NonPublic);            
            field.SetValue(null, false);
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

        [Test]
        public void SimpleConfiguration_ValidEndpointAndAuthKeyToggleDoesNotExist_ThrowsToggleConfigurationError()
        {
            // Arrange
            DocumentDbProvider.Configure(TestConfig.ValidEndpoint, TestConfig.ValidAuthKey);
            var toggle = new TestFeatureToggle();
            // Act and Assert
            Should.Throw<ToggleConfigurationError>(() =>
            {
                var toggleValue = toggle.FeatureEnabled;
            });
        }

        [Test]
        public void SimpleConfiguration_EndpointIsNull_ThrowsArgumentException()
        {   
            // Act and Assert
            Should.Throw<ArgumentException>(() =>
            {
                DocumentDbProvider.Configure(null, TestConfig.ValidAuthKey);
            });
        }

        [Test]
        public void SimpleConfiguration_AuthKeyIsNull_ThrowsArgumentException()
        {            
            // Act and Assert
            Should.Throw<ArgumentException>(() =>
            {
                DocumentDbProvider.Configure(TestConfig.ValidEndpoint, null);
            });
        }

        [Test]
        public void FullConfiguration_ConfigIsNull_ThrowsAgurmentNullException()
        {
            // Act and Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                DocumentDbProvider.Configure(null);
            });
        }

        [Test]
        public void FullConfiguration_AutoCreateDbAndCollectionAndToggleDoesNotExist_ThrowsToggleConfigurationError()
        {
            // Arrange
            var config = TestConfig.ValidConfig(databaseId, collectionId);
            config.AutoCreateDatabaseAndCollection = true;

            DocumentDbProvider.Configure(config);
            var toggle = new TestFeatureToggle();
            // Act and Assert
            Should.Throw<ToggleConfigurationError>(() =>
            {
                var toggleValue = toggle.FeatureEnabled;
            });
        }

        [Test]
        public void FullConfiguration_AutoCreateToggleAndDbAndCollectionDoesNotExist_ThrowsToggleConfigurationError()
        {
            // Arrange
            var config = TestConfig.ValidConfig(databaseId, collectionId);
            config.AutoCreateFeature = true;

            DocumentDbProvider.Configure(config);
            var toggle = new TestFeatureToggle();
            // Act and Assert
            Should.Throw<ToggleConfigurationError>(() =>
            {
                var toggleValue = toggle.FeatureEnabled;
            });             
        }

        [Test]
        public void FullConfiguration_AutoCreateDbAndCollectionAndAutoCreateToggle_ToggleValueIsFalse()
        {
            // Arrange
            var config = TestConfig.ValidConfig(databaseId, collectionId);
            config.AutoCreateDatabaseAndCollection = true;
            config.AutoCreateFeature = true;

            DocumentDbProvider.Configure(config);
            var toggle = new TestFeatureToggle();            
            // Act
            var toggleValue = toggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeFalse();
        }

        [Test]
        public async Task FullConfiguration_AutoCreateDbAndCollectionAndToggleIsTurnedOn_ToggleValueIsTrue()
        {
            // Arrange
            var config = TestConfig.ValidConfig(databaseId, collectionId);
            config.AutoCreateDatabaseAndCollection = true;
            config.AutoCreateFeature = true;

            DocumentDbProvider.Configure(config);
            var toggle = new TestFeatureToggle();

            var unused = toggle.FeatureEnabled; // Auto creates the database and collection
            var document = new BooleanFeatureToggleDocument(nameof(TestFeatureToggle)) { Enabled = true };
            await client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId), document, disableAutomaticIdGeneration: true);
            // Act
            var toggleValue = toggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeTrue();
        }

        // create test for the date time feature toggle
    }
}
