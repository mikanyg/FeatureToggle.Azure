using FeatureToggle.Azure.DocumentDB.Test.Toggles;
using FeatureToggle.Azure.Providers;
using Microsoft.Azure.Documents.Client;
using NUnit.Framework;
using Shouldly;
using System;
using System.Threading.Tasks;

namespace FeatureToggle.Azure.DocumentDB.Test
{

    [TestFixture]
    public class DocumentDbProviderTest : DocumentDbTestFixture
    {        
        private DocumentDbProvider sut;

        [SetUp]
        public void Setup()
        {
            sut = new DocumentDbProvider();
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
            AutoCreateToggle<TestFeatureToggle>();

            var document = new BooleanFeatureToggleDocument(nameof(TestFeatureToggle)) { Enabled = true };
            await UpdateToggleDocument(document);

            // Act
            var toggleValue = sut.EvaluateBooleanToggleValue(new TestFeatureToggle());
            // Assert
            toggleValue.ShouldBeTrue();
        }        

        [Test]
        public async Task EvaluateDateTimeToggleValue_ToggleExists_ToggleValueIsToday()
        {
            // Arrange
            AutoCreateToggle<TestFeatureToggle>();

            var document = new DateTimeFeatureToggleDocument(nameof(TestFeatureToggle)) { ToggleTimestamp = DateTime.Today };
            await UpdateToggleDocument(document);

            // Act
            var toggleValue = sut.EvaluateDateTimeToggleValue(new TestFeatureToggle());
            // Assert
            toggleValue.ShouldBe(DateTime.Today);
        }        
    }
}
