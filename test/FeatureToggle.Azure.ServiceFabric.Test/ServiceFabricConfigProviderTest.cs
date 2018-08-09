using FeatureToggle.Azure.Providers;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace FeatureToggle.Azure.ServiceFabric.Test
{
    [TestFixture]
    public class ServiceFabricConfigProviderTest : ServiceFabricTestFixture
    {
        [Test]
        public void SimpleConfiguration_ConfigPackageNameIsNull_ThrowsArgumentException()
        {            
            Should.Throw<ArgumentException>(() => ServiceFabricConfigProvider.Configure(null, "Toggles"));
        }

        [Test]
        public void SimpleConfiguration_ConfigSectionNameIsNull_ThrowsArgumentException()
        {
            Should.Throw<ArgumentException>(() => ServiceFabricConfigProvider.Configure("Testconfig", null));
        }

        [Test]
        public void SimpleConfiguration_SetCustomPackageNameAndSection_ProviderIsConfigured()
        {            
            Should.NotThrow(() => ServiceFabricConfigProvider.Configure("Testconfig", "Toggles"));            
        }

        [Test]
        public void FullConfiguration_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            Should.Throw<ArgumentNullException>(() => ServiceFabricConfigProvider.Configure(null));
        }

        [Test]
        public void FullConfiguration_SetCustomPackageNameAndSection_ProviderIsConfigured()
        {
            // Arrange
            var config = new ServiceFabricConfiguration { ConfigPackageName = "TestConfig", ConfigSectionName = "Toggles" };
            // Act and Assert
            Should.NotThrow(() => ServiceFabricConfigProvider.Configure(config));
        }
    }
}
