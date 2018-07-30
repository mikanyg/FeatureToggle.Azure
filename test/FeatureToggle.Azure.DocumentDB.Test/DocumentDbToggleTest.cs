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
    public class DocumentDbToggleTest : DocumentDbTestFixture
    {
        [Test]
        public void FeatureEnabled_ToggleExists_ToggleValueIsFalse()
        {
            // Arrange
            AutoCreateToggle<TestFeatureToggle>();
            var toggle = new TestFeatureToggle();
            // Act
            var toggleValue = toggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeFalse();
        }

        [Test]
        public async Task FeatureEnabled_ToggleExists_ToggleValueIsTrue()
        {
            // Arrange
            AutoCreateToggle<TestFeatureToggle>();
            await UpdateToggleDocument(new BooleanFeatureToggleDocument(nameof(TestFeatureToggle)) { Enabled = true });

            var toggle = new TestFeatureToggle();
            // Act
            var toggleValue = toggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeTrue();
        }
    }
}
