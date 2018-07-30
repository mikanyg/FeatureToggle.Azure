using FeatureToggle.Azure.Providers;
using Microsoft.Azure.Documents.Client;
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
        private DocumentDbProvider sut;

        [SetUp]
        public void Setup()
        {
            client = new DocumentClient(new Uri(TestConfig.ValidEndpoint), TestConfig.ValidAuthKey);
            sut = new DocumentDbProvider();

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

        [Test]
        public void SimpleConfiguration_ValidEndpointAndAuthKeyToggleDoesNotExist_ThrowsToggleConfigurationError()
        {
            // Arrange
            DocumentDbProvider.Configure(TestConfig.ValidEndpoint, TestConfig.ValidAuthKey);
            
            // Act and Assert
            Should.Throw<ToggleConfigurationError>(() =>
            {
                var toggleValue = sut.EvaluateBooleanToggleValue(new TestFeatureToggle());
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
        public void EvaluateBooleanToggleValue_AutoCreateDbAndCollectionToggleDoesNotExist_ThrowsToggleConfigurationError()
        {
            // Arrange
            ConfigureProvider(autoCreateDbAndCollection: true);

            // Act and Assert
            Should.Throw<ToggleConfigurationError>(() =>
            {
                var toggleValue = sut.EvaluateBooleanToggleValue(new TestFeatureToggle());
            });
        }

        [Test]
        public void EvaluateBooleanToggleValue_AutoCreateToggleDbAndCollectionDoesNotExist_ThrowsToggleConfigurationError()
        {
            // Arrange
            ConfigureProvider(autoCreateToggle: true);

            // Act and Assert
            Should.Throw<ToggleConfigurationError>(() =>
            {
                var toggleValue = sut.EvaluateBooleanToggleValue(new TestFeatureToggle());
            });             
        }

        [Test]
        public void EvaluateBooleanToggleValue_AutoCreateDbAndCollectionAndAutoCreateToggle_ToggleValueIsFalse()
        {
            // Arrange
            ConfigureProvider(true, true);

            // Act
            var toggleValue = sut.EvaluateBooleanToggleValue(new TestFeatureToggle());
            // Assert
            toggleValue.ShouldBeFalse();
        }

        [Test]
        public async Task EvaluateBooleanToggleValue_ToggleExists_ToggleValueIsTrue()
        {
            // Arrange
            AutoCreateToggle();

            var document = new BooleanFeatureToggleDocument(nameof(TestFeatureToggle)) { Enabled = true };
            await client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId), document, disableAutomaticIdGeneration: true);

            // Act
            var toggleValue = sut.EvaluateBooleanToggleValue(new TestFeatureToggle());
            // Assert
            toggleValue.ShouldBeTrue();
        }        
                
        [Test]
        public async Task EvaluateDateTimeToggleValue_ToggleExists_ToggleValueIsToday()
        {
            // Arrange
            AutoCreateToggle();

            var document = new DateTimeFeatureToggleDocument(nameof(TestFeatureToggle)) { ToggleTimestamp = DateTime.Today };
            await client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId), document, disableAutomaticIdGeneration: true);

            // Act
            var toggleValue = sut.EvaluateDateTimeToggleValue(new TestFeatureToggle());
            // Assert
            toggleValue.ShouldBe(DateTime.Today);
        }

        private void AutoCreateToggle()
        {
            ConfigureProvider(true, true);

            var toggle = new TestFeatureToggle();
            var unused = toggle.FeatureEnabled; // Auto creates the database and collection            
        }

        private void ConfigureProvider(bool autoCreateDbAndCollection = false, bool autoCreateToggle = false)
        {
            var config = TestConfig.ValidConfig(databaseId, collectionId);
            config.AutoCreateDatabaseAndCollection = autoCreateDbAndCollection;
            config.AutoCreateFeature = autoCreateToggle;

            DocumentDbProvider.Configure(config);
        }

        private static void ResetCollectionValidationInProvider()
        {
            // Reset docdb collection validation in provider
            var field = typeof(DocumentDbProvider).GetField("_collectionVerified", BindingFlags.Static | BindingFlags.NonPublic);
            field.SetValue(null, false);
        }
    }
}
