using FeatureToggle.Azure.DocumentDB.Test.Toggles;
using FeatureToggle.Azure.Providers;
using NUnit.Framework;
using Shouldly;
using System;
using System.Threading.Tasks;

namespace FeatureToggle.Azure.DocumentDB.Test
{
    [TestFixture]
    public class TimePeriodToggleTest : DocumentDbTestFixture
    {
        [Test]
        public async Task FeatureEnabled_LimitedTimeOfferFeatureToggleSetToEnabledTomorrow_ToggleValueIsFalse()
        {
            // Arrange
            AutoCreateToggle<LimitedTimeOfferFeatureToggle>();
            await UpdateToggleDocument(new TimePeriodFeatureToggleDocument(nameof(LimitedTimeOfferFeatureToggle)) { EnabledFrom = DateTime.Now.AddDays(1), EnabledTo = DateTime.Now.AddDays(2) });

            var toggle = new LimitedTimeOfferFeatureToggle();
            // Act
            var toggleValue = toggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeFalse();
        }

        [Test]
        public async Task FeatureEnabled_LimitedTimeOfferFeatureToggleSetToExpiredYesterday_ToggleValueIsFalse()
        {
            // Arrange
            AutoCreateToggle<LimitedTimeOfferFeatureToggle>();
            await UpdateToggleDocument(new TimePeriodFeatureToggleDocument(nameof(LimitedTimeOfferFeatureToggle)) { EnabledFrom = DateTime.Now.AddDays(-2), EnabledTo = DateTime.Now.AddDays(-1) });

            var toggle = new LimitedTimeOfferFeatureToggle();
            // Act
            var toggleValue = toggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeFalse();
        }

        [Test]
        public async Task FeatureEnabled_LimitedTimeOfferFeatureToggleSetToEnabledYesterday_ToggleValueIsTrue()
        {
            // Arrange
            AutoCreateToggle<LimitedTimeOfferFeatureToggle>();
            await UpdateToggleDocument(new TimePeriodFeatureToggleDocument(nameof(LimitedTimeOfferFeatureToggle)) { EnabledFrom = DateTime.Now.AddDays(-1), EnabledTo = DateTime.Now.AddDays(1) });

            var toggle = new LimitedTimeOfferFeatureToggle();
            // Act
            var toggleValue = toggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeTrue();
        }
    }
}
