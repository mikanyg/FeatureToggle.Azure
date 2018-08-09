using FeatureToggle.Azure.Providers;
using FeatureToggle.Azure.ServiceFabric.Test.Toggles;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FeatureToggle.Azure.ServiceFabric.Test
{

    [TestFixture]
    public class DateTimeToggleTest : ServiceFabricTestFixture
    {
        private const string ExpectedDateFormat = @"dd-MMM-yyyy HH:mm:ss";

        [SetUp]
        public void Setup()
        {
            ServiceFabricConfigProvider.Configure(usePrefix: false);
        }

        [Test]
        public void FeatureEnabled_ExpiringFeatureToggleSetToExpireYesterday_ToggleValueIsFalse()
        {
            // Arrange
            SetToggleInConfig(nameof(ExpiringTestFeatureToggle), DateTime.Today.AddDays(-1).ToString(ExpectedDateFormat));
            var toggle = new ExpiringTestFeatureToggle();
            // Act
            var toggleValue = toggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeFalse();
        }

        [Test]
        public void FeatureEnabled_ExpiringFeatureToggleSetToExpireTomorrow_ToggleValueIsTrue()
        {
            // Arrange            
            SetToggleInConfig(nameof(ExpiringTestFeatureToggle), DateTime.Today.AddDays(1).ToString(ExpectedDateFormat));
            var toggle = new ExpiringTestFeatureToggle();
            // Act
            var toggleValue = toggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeTrue();
        }

        [Test]
        public void FeatureEnabled_CominSoonFeatureToggleSetToEnabledYesterday_ToggleValueIsTrue()
        {
            // Arrange            
            SetToggleInConfig(nameof(ComingSoonTestFeatureToggle), DateTime.Today.AddDays(-1).ToString(ExpectedDateFormat));
            var toggle = new ComingSoonTestFeatureToggle();
            // Act
            var toggleValue = toggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeTrue();
        }

        [Test]
        public void FeatureEnabled_ComingSoonFeatureToggleSetToEnabledTomorrow_ToggleValueIsFalse()
        {
            // Arrange
            SetToggleInConfig(nameof(ComingSoonTestFeatureToggle), DateTime.Today.AddDays(1).ToString(ExpectedDateFormat));            
            var toggle = new ComingSoonTestFeatureToggle();
            // Act
            var toggleValue = toggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeFalse();
        }

        [Test]
        public void FeatureEnabled_ComingSoonFeatureToggleSetToInvalidDate_ThrowToggleConfigError()
        {
            // Arrange
            SetToggleInConfig(nameof(ComingSoonTestFeatureToggle), "invalid date string");
            var toggle = new ComingSoonTestFeatureToggle();

            var error = Should.Throw<ToggleConfigurationError>(() =>
            {
                // Act
                var toggleValue = toggle.FeatureEnabled;
            });

            // Assert
            error.Message.ShouldContain("cannot be converted to a DateTime as defined in config key");
        }
    }
}
