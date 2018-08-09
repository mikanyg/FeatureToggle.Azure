using FeatureToggle.Azure.Providers;
using FeatureToggle.Azure.ServiceFabric.Test.Toggles;
using NUnit.Framework;
using Shouldly;

namespace FeatureToggle.Azure.ServiceFabric.Test
{
    [TestFixture]
    public class ServiceFabricFeatureToggleTest : ServiceFabricTestFixture
    {
        [Test]
        public void FeatureEnabled_ToggleExists_ToggleValueIsFalse()
        {
            // Arrange
            SetToggleInConfig($"FeatureToggle.{nameof(TestFeatureToggle)}", bool.FalseString);
            var toggle = new TestFeatureToggle();
            // Act
            var toggleValue = toggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeFalse();
        }

        [Test]
        public void FeatureEnabled_ToggleExists_ToggleValueIsTrue()
        {
            // Arrange
            SetToggleInConfig($"FeatureToggle.{nameof(TestFeatureToggle)}", bool.TrueString);            
            var toggle = new TestFeatureToggle();
            // Act
            var toggleValue = toggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeTrue();
        }

        [Test, Order(1)]
        public void FeatureEnabled_ToggleNameIsWithoutPrefixAndNotConfigured_ThrowsToggleConfigurationError()
        {
            // Arrange            
            SetToggleInConfig(nameof(TestFeatureToggle), bool.TrueString);
            var toggle = new TestFeatureToggle();
            // Assert            
            var error = Should.Throw<ToggleConfigurationError>(() =>
            {
                // Act
                var toggleValue = toggle.FeatureEnabled;
            });

            error.Message.ShouldBe("The key 'FeatureToggle.TestFeatureToggle' was not found in settings.xml");
        }

        [Test, Order(2)]
        public void FeatureEnabled_ToggleNameIsWithoutPrefixAndConfigured_ToggleValueIsTrue()
        {
            // Arrange            
            ServiceFabricConfigProvider.Configure(usePrefix: false);
            SetToggleInConfig(nameof(TestFeatureToggle), bool.TrueString);
            var toggle = new TestFeatureToggle();
            // Act
            var toggleValue = toggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeTrue();
        }

        [Test]
        public void FeatureEnabled_FeatureToggleSetToInvalidValue_ThrowsToggleConfigurationError()
        {
            // Arrange            
            SetToggleInConfig($"FeatureToggle.{nameof(TestFeatureToggle)}", "not a bool value");
            var toggle = new TestFeatureToggle();
            // Assert            
            var error = Should.Throw<ToggleConfigurationError>(() =>
            {
                // Act
                var toggleValue = toggle.FeatureEnabled;
            });

            error.Message.ShouldContain("cannot be converted to a boolean as defined in config key");
        }
    }
}
