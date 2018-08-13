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
    public class TimePeriodToggleTest : ServiceFabricTestFixture
    {
        private const string ExpectedDateFormat = @"dd-MMM-yyyy HH:mm:ss";

        [SetUp]
        public void Setup()
        {
            ServiceFabricConfigProvider.Configure(usePrefix: false);
        }

        [Test]        
        public void FeatureEnabled_LimitedTimeOfferFeatureToggleSetToEnabledTomorrow_ToggleValueIsFalse()
        {
            // Arrange
            DateTime start = DateTime.Today.AddDays(1);
            DateTime end = DateTime.Today.AddDays(2);
            string configValue = $"{start:dd-MMM-yyyy HH:mm:ss} | {end:dd-MMM-yyyy HH:mm:ss}";
            SetToggleInConfig(nameof(LimitedTimeOfferFeatureToggle), configValue);
            var toggle = new LimitedTimeOfferFeatureToggle();
            // Act
            var toggleValue = toggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeFalse();
        }

        [Test]
        public void FeatureEnabled_LimitedTimeOfferFeatureToggleSetToExpiredYesterday_ToggleValueIsFalse()
        {
            // Arrange
            DateTime start = DateTime.Today.AddDays(-2);
            DateTime end = DateTime.Today.AddDays(-1);
            string configValue = $"{start:dd-MMM-yyyy HH:mm:ss} | {end:dd-MMM-yyyy HH:mm:ss}";
            SetToggleInConfig(nameof(LimitedTimeOfferFeatureToggle), configValue);
            var toggle = new LimitedTimeOfferFeatureToggle();
            // Act
            var toggleValue = toggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeFalse();
        }

        [Test]
        public void FeatureEnabled_LimitedTimeOfferFeatureToggleSetToEnabledYesterday_ToggleValueIsTrue()
        {
            // Arrange            
            DateTime start = DateTime.Today.AddDays(-1);
            DateTime end = DateTime.Today.AddDays(1);
            string configValue = $"{start:dd-MMM-yyyy HH:mm:ss} | {end:dd-MMM-yyyy HH:mm:ss}";
            SetToggleInConfig(nameof(LimitedTimeOfferFeatureToggle), configValue);
            var toggle = new LimitedTimeOfferFeatureToggle();
            // Act
            var toggleValue = toggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeTrue();
        }        

        [TestCase("02-Jan-2050 04:05:10 | 02-Jan-2050 04:05:09", "the start date must be less then the end date")]
        [TestCase("02-Jan-2050 04:05:09 | 02-Jan-2050 04:05:09", "the start date must be less then the end date")]
        [TestCase("01-Jan-2050 04:05:09 | 02/01/2050 04:05:44", "cannot be converted to a DateTime")]
        [TestCase("qwerty | 07-Aug-2099 06:05:04", "cannot be converted to a DateTime")]
        [TestCase("02-Jan-2050 04:05:08 ; 07-Aug-2099 06:05:04", "cannot be converted to a DateTime")]
        [TestCase("02-Jan-2050 04:05:08 | qwerty", "cannot be converted to a DateTime")]
        public void FeatureEnabled_ComingSoonFeatureToggleSetToInvalidDate_ThrowToggleConfigError(string configValue, string errorMsg)
        {
            // Arrange
            SetToggleInConfig(nameof(LimitedTimeOfferFeatureToggle), configValue);
            var toggle = new LimitedTimeOfferFeatureToggle();

            var error = Should.Throw<ToggleConfigurationError>(() =>
            {
                // Act
                var toggleValue = toggle.FeatureEnabled;
            });

            // Assert
            error.Message.ShouldContain(errorMsg);
        }
    }
}
