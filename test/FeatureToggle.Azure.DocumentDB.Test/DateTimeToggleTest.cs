using FeatureToggle.Azure.DocumentDB.Test.Toggles;
using FeatureToggle.Azure.Providers;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FeatureToggle.Azure.DocumentDB.Test
{
    [TestFixture]
    public class DateTimeToggleTest : DocumentDbTestFixture
    {
        [Test]
        public async Task FeatureEnabled_ExpiringFeatureToggleSetToExpireYesterday_ToggleValueIsFalse()
        {
            // Arrange
            AutoCreateToggle<ExpiringTestFeatureToggle>();
            await UpdateToggleDocument(new DateTimeFeatureToggleDocument(nameof(ExpiringTestFeatureToggle)) { ToggleTimestamp = DateTime.Today.AddDays(-1) });

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
            await UpdateToggleDocument(new DateTimeFeatureToggleDocument(nameof(ExpiringTestFeatureToggle)) { ToggleTimestamp = DateTime.Today.AddDays(1) });

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
            await UpdateToggleDocument(new DateTimeFeatureToggleDocument(nameof(ComingSoonTestFeatureToggle)) { ToggleTimestamp = DateTime.Today.AddDays(-1) });

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
            await UpdateToggleDocument(new DateTimeFeatureToggleDocument(nameof(ComingSoonTestFeatureToggle)) { ToggleTimestamp = DateTime.Today.AddDays(1) });

            var toggle = new ComingSoonTestFeatureToggle();
            // Act
            var toggleValue = toggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeFalse();
        }
    }
}
