using FeatureToggle.Azure.Providers;
using FeatureToggle.Azure.TableStorage.Test.Toggles;
using NUnit.Framework;
using Shouldly;
using System;
using System.Threading.Tasks;

namespace FeatureToggle.Azure.TableStorage.Test
{

    [TestFixture]
    public class TableStorageProviderTest : TableStorageTestFixture
    {        
        private TableStorageProvider sut;

        [SetUp]
        public void Setup()
        {
            sut = new TableStorageProvider();
        }
        
        [Test]
        public void SimpleConfiguration_ValidConnectionStringToggleDoesNotExist_ThrowsToggleConfigurationError()
        {
            // Arrange
            TableStorageProvider.Configure(TestConfig.ValidConnectionString);
            
            // Act and Assert
            Should.Throw<ToggleConfigurationError>(() =>
            {
                var toggleValue = sut.EvaluateBooleanToggleValue(new TestFeatureToggle());
            });
        }

        [Test]
        public void SimpleConfiguration_ConnectionStringIsNull_ThrowsArgumentException()
        {   
            // Act and Assert
            Should.Throw<ArgumentException>(() =>
            {
                TableStorageProvider.Configure((string) null);
            });
        }        

        [Test]
        public void FullConfiguration_ConfigIsNull_ThrowsAgurmentNullException()
        {
            // Act and Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                TableStorageProvider.Configure((TableStorageConfiguration) null);
            });
        }

        [Test]
        public void EvaluateBooleanToggleValue_AutoCreateTableToggleDoesNotExist_ThrowsToggleConfigurationError()
        {
            // Arrange
            ConfigureProvider(autoCreateTable: true);

            // Act and Assert
            Should.Throw<ToggleConfigurationError>(() =>
            {
                var toggleValue = sut.EvaluateBooleanToggleValue(new TestFeatureToggle());
            });
        }

        [Test]
        public void EvaluateBooleanToggleValue_AutoCreateToggleTableDoesNotExist_ThrowsToggleConfigurationError()
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
        public void EvaluateBooleanToggleValue_AutoCreateTableAndAutoCreateToggle_ToggleValueIsFalse()
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

            var entity = new BooleanFeatureToggleEntity(partitionKey, nameof(TestFeatureToggle)) { Enabled = true };
            await UpdateToggleEntity(entity);

            // Act
            var toggleValue = sut.EvaluateBooleanToggleValue(new TestFeatureToggle());
            // Assert
            toggleValue.ShouldBeTrue();
        }        

        [Test]
        public async Task EvaluateDateTimeToggleValue_ToggleExists_ToggleValueIsUtcNow()
        {
            // Arrange
            AutoCreateToggle<TestFeatureToggle>();
            var utcNow = DateTime.UtcNow;
            var entity = new DateTimeFeatureToggleEntity(partitionKey, nameof(TestFeatureToggle)) { ToggleTimestamp = utcNow };
            await UpdateToggleEntity(entity);

            // Act
            var toggleValue = sut.EvaluateDateTimeToggleValue(new TestFeatureToggle());
            // Assert
            toggleValue.ShouldBe(utcNow);
        }        
    }
}
