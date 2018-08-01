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
    public class DateTimeToggleTest : TableStorageTestFixture
    {
        [Test]
        public async Task FeatureEnabled_ExpiringFeatureToggleSetToExpireYesterday_ToggleValueIsFalse()
        {
            // Arrange
            AutoCreateToggle<ExpiringTestFeatureToggle>();
            await UpdateToggleEntity(new DateTimeFeatureToggleEntity(partitionKey, nameof(ExpiringTestFeatureToggle)) { ToggleTimestamp = DateTime.Now.AddDays(-1) });

            var toggle = new ExpiringTestFeatureToggle();
            // Act
            var toggleValue = toggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeFalse();
        }

        [Test]
        public async Task FeatureEnabled_ExpiringFeatureToggleSetToExpireTomorrow_ToggleValueIsTrue()
        {
            // Arrange            
            AutoCreateToggle<ExpiringTestFeatureToggle>();
            await UpdateToggleEntity(new DateTimeFeatureToggleEntity(partitionKey, nameof(ExpiringTestFeatureToggle)) { ToggleTimestamp = DateTime.Now.AddDays(1) });

            var toggle = new ExpiringTestFeatureToggle();
            // Act
            var toggleValue = toggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeTrue();
        }

        [Test]
        public async Task FeatureEnabled_CominSoonFeatureToggleSetToEnabledYesterday_ToggleValueIsTrue()
        {
            // Arrange
            AutoCreateToggle<ComingSoonTestFeatureToggle>();
            await UpdateToggleEntity(new DateTimeFeatureToggleEntity(partitionKey, nameof(ComingSoonTestFeatureToggle)) { ToggleTimestamp = DateTime.Now.AddDays(-1) });

            var toggle = new ComingSoonTestFeatureToggle();
            // Act
            var toggleValue = toggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeTrue();
        }

        [Test]
        public async Task FeatureEnabled_ComingSoonFeatureToggleSetToEnabledTomorrow_ToggleValueIsFalse()
        {
            // Arrange
            AutoCreateToggle<ComingSoonTestFeatureToggle>();
            await UpdateToggleEntity(new DateTimeFeatureToggleEntity(partitionKey, nameof(ComingSoonTestFeatureToggle)) { ToggleTimestamp = DateTime.Now.AddDays(1) });

            var toggle = new ComingSoonTestFeatureToggle();
            // Act
            var toggleValue = toggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeFalse();
        }
    }
}
