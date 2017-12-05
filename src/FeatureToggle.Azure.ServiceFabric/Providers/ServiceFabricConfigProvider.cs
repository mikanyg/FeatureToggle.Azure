using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;

namespace FeatureToggle.Azure.ServiceFabric.Providers
{
    public class ServiceFabricConfigProvider : IBooleanToggleValueProvider
    {
        private const string KeyNotFoundInSettingsMessage = "The key '{0}' was not found in settings.xml";

        public static string ConfigPackageName { get; set; } = "Config"; 
        public static string ConfigSectionName { get; set; } = "Features";
        public static bool UsePrefix { get; set; } = true;

        public bool EvaluateBooleanToggleValue(IFeatureToggle toggle)
        {
            var key = ExpectedAppSettingsKeyFor(toggle);

            ValidateKeyExists(key);

            var configValue = GetConfigValue(key);

            return ParseBooleanConfigString(configValue, key);
        }

        private string ExpectedAppSettingsKeyFor(IFeatureToggle toggle)
        {
            var prefix = UsePrefix ? ToggleConfigurationSettings.Prefix : string.Empty;
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
            var configPkg = FabricRuntime.GetActivationContext().GetConfigurationPackageObject(ConfigPackageName);
            return configPkg.Settings.Sections[ConfigSectionName].Parameters[key].Value;
        }

        private string[] GetAllConfigKeys()
        {
            var configPkg = FabricRuntime.GetActivationContext().GetConfigurationPackageObject(ConfigPackageName);
            return configPkg.Settings.Sections[ConfigSectionName].Parameters.Select(setting => setting.Name).ToArray();
        }
    }
}
