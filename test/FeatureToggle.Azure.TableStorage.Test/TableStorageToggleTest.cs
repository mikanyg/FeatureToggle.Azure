using FeatureToggle.Azure.Providers;
using FeatureToggle.Azure.TableStorage.Test.Toggles;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FeatureToggle.Azure.TableStorage.Test
{
    [TestFixture]
    public class TableStorageToggleTest : TableStorageTestFixture
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
            await UpdateToggleEntity(new BooleanFeatureToggleEntity(partitionKey, nameof(TestFeatureToggle)) { Enabled = true });

            var toggle = new TestFeatureToggle();
            // Act
            var toggleValue = toggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeTrue();
        }        
    }
}
