using System;
using System.Fabric;
using System.Linq;

namespace FeatureToggle.Azure.Providers
{
    public class ServiceFabricConfigProvider : IBooleanToggleValueProvider
    {
        private const string KeyNotFoundInSettingsMessage = "The key '{0}' was not found in settings.xml";

        private static ServiceFabricConfiguration Configuration { get; set; } = new ServiceFabricConfiguration();

        public static void Configure(string configPackageName = "Config", string configSectionName = "Features", bool usePrefix = true)
        {
            if (string.IsNullOrEmpty(configPackageName))
            {
                throw new ArgumentException("value cannot be null or empty", nameof(configPackageName));
            }

            if (string.IsNullOrEmpty(configSectionName))
            {
                throw new ArgumentException("value cannot be null or empty", nameof(configSectionName));
            }

            Configure(new ServiceFabricConfiguration { ConfigPackageName = configPackageName, ConfigSectionName = configSectionName, UsePrefix = usePrefix });
        }

        public static void Configure(ServiceFabricConfiguration config)
        {
            Configuration = config ?? throw new ArgumentNullException(nameof(config));
        }

        public bool EvaluateBooleanToggleValue(IFeatureToggle toggle)
        {
            var key = ExpectedAppSettingsKeyFor(toggle);

            ValidateKeyExists(key);

            var configValue = GetConfigValue(key);

            return ParseBooleanConfigString(configValue, key);
        }

        private string ExpectedAppSettingsKeyFor(IFeatureToggle toggle)
        {
            var prefix = Configuration.UsePrefix ? ToggleConfigurationSettings.Prefix : string.Empty;
            return prefix + toggle.GetType().Name;
        }

        private void ValidateKeyExists(string key)
        {
            var allKeys = GetAllConfigKeys();
            if (!allKeys.Contains(key))
            {
                throw new ToggleConfigurationError(string.Format(KeyNotFoundInSettingsMessage, key));
            }
        }

        private bool ParseBooleanConfigString(string valueToParse, string configKey)
        {
            try
            {
                return bool.Parse(valueToParse);
            }
            catch (Exception ex)
            {
                throw new ToggleConfigurationError($"The value '{valueToParse}' cannot be converted to a boolean as defined in config key '{configKey}'", ex);
            }
        }

        private string GetConfigValue(string key)
        {
            var configPkg = FabricRuntime.GetActivationContext().GetConfigurationPackageObject(Configuration.ConfigPackageName);
            return configPkg.Settings.Sections[Configuration.ConfigSectionName].Parameters[key].Value;
        }

        private string[] GetAllConfigKeys()
        {
            var configPkg = FabricRuntime.GetActivationContext().GetConfigurationPackageObject(Configuration.ConfigPackageName);
            return configPkg.Settings.Sections[Configuration.ConfigSectionName].Parameters.Select(setting => setting.Name).ToArray();
        }
    }
}
