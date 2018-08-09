using FeatureToggle.Azure.EntityFrameworkCore.Providers;
using FeatureToggle.Azure.Toggles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace FeatureToggle.Azure.EntityFrameworkCore.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            var config = new EFCoreConfiguration();
            var featureToggle = new SomeCoolFeatureToggle();
            // Act
            var toggleValue = featureToggle.FeatureEnabled;
            // Assert
            toggleValue.ShouldBeFalse();
        }
    }

    public class SomeCoolFeatureToggle : EFCoreToggle
    {
    }
}
