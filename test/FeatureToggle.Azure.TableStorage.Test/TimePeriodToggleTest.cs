using FeatureToggle.Azure.Providers;
using FeatureToggle.Azure.TableStorage.Test.Toggles;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FeatureToggle.Azure.TableStorage.Test
{
    [TestFixture]
    public class TimePeriodToggleTest : TableStorageTestFixture
    {
        [Test]
        public async Task FeatureEnabled_LimitedTimeOfferFeatureToggleSetToEnabledTomorrow_ToggleValueIsFalse()
        {
            // Arrange
            AutoCreateToggle<LimitedTimeOfferFeatureToggle>();
            await UpdateToggleEntity(new TimePeriodFeatureToggleEntity(partitionKey, nameof(LimitedTimeOfferFeatureToggle)) { EnabledFrom = DateTime.Now.AddDays(1), EnabledTo = DateTime.Now.AddDays(2) });

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
            await UpdateToggleEntity(new TimePeriodFeatureToggleEntity(partitionKey, nameof(LimitedTimeOfferFeatureToggle)) { EnabledFrom = DateTime.Now.AddDays(-2), EnabledTo = DateTime.Now.AddDays(-1) });

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
            await UpdateToggleEntity(new TimePeriodFeatureToggleEntity(partitionKey, nameof(LimitedTimeOfferFeatureToggle)) { EnabledFrom = DateTime.Now.AddDays(-1), EnabledTo = DateTime.Now.AddDays(1) });

            var toggle = new LimitedTimeOfferFeatureToggle();
            // Act
            var toggleValue = toggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeTrue();
        }
    }
}
