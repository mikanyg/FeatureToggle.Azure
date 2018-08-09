using FeatureToggle.Azure.Providers;
using FeatureToggle.Azure.ServiceFabric.Test.Toggles;
using ServiceFabric.Mocks;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Description;
using System.Text;
using static ServiceFabric.Mocks.MockConfigurationPackage;

namespace FeatureToggle.Azure.ServiceFabric.Test
{
    public static class TestConfig
    {
        public static string ConfigPackageName { get; set; } = new ServiceFabricConfiguration().ConfigPackageName;
        public static string ConfigSectionName { get; set; } = new ServiceFabricConfiguration().ConfigSectionName;
        public static string ToggleName { get; set; } = nameof(TestFeatureToggle);
        public static string ToggleValue { get; set; } = string.Empty;

        public static ICodePackageActivationContext CreateMockCodePackageActivationContext()
        {
            //build ConfigurationSectionCollection
            var configSections = new ConfigurationSectionCollection();

            //Build ConfigurationSettings
            var configSettings = CreateConfigurationSettings(configSections);

            //add one ConfigurationSection
            ConfigurationSection configSection = CreateConfigurationSection(ConfigSectionName);
            configSections.Add(configSection);

            //add one Parameters entry
            ConfigurationProperty parameter = CreateConfigurationSectionParameters(ToggleName, ToggleValue);
            configSection.Parameters.Add(parameter);

            //Build ConfigurationPackage
            ConfigurationPackage configPackage = CreateConfigurationPackage(configSettings, nameof(configPackage.Path));

            return new MockCodePackageActivationContext(
                "fabric:/MockApp",
                "MockAppType",
                "Code",
                "1.0.0.0",
                Guid.NewGuid().ToString(),
                @"C:\logDirectory",
                @"C:\tempDirectory",
                @"C:\workDirectory",
                "ServiceManifestName",
                "1.0.0.0")
            {
                ConfigurationPackage = configPackage,
                ConfigurationPackageNames = new List<string> { ConfigPackageName }
            };
        }
    }
}
