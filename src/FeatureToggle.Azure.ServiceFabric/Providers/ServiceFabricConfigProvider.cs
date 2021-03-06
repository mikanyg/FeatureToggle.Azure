﻿using System;
using System.Fabric;
using System.Linq;

namespace FeatureToggle.Azure.Providers
{
    public class ServiceFabricConfigProvider : IBooleanToggleValueProvider, IDateTimeToggleValueProvider, ITimePeriodProvider
    {
        private const string KeyNotFoundInSettingsMessage = "The key '{0}' was not found in settings.xml";

        private static ServiceFabricConfiguration Configuration { get; set; } = new ServiceFabricConfiguration();
        private static Func<ICodePackageActivationContext> CodePackageActivationContextFactory { get; set; } = () => FabricRuntime.GetActivationContext();

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

        public static void SetCodePackageActivationContextFactory(Func<ICodePackageActivationContext> factory)
        {
            CodePackageActivationContextFactory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public bool EvaluateBooleanToggleValue(IFeatureToggle toggle)
        {
            var key = ExpectedAppSettingsKeyFor(toggle);

            ValidateKeyExists(key);

            var configValue = GetConfigValue(key);

            return ParseBooleanConfigString(configValue, key);
        }

        public DateTime EvaluateDateTimeToggleValue(IFeatureToggle toggle)
        {
            var key = ExpectedAppSettingsKeyFor(toggle);

            ValidateKeyExists(key);

            var configValue = GetConfigValue(key);

            var parser = new ConfigurationDateParser();

            return parser.ParseDateTimeConfigString(configValue, key);
        }

        public Tuple<DateTime, DateTime> EvaluateTimePeriod(IFeatureToggle toggle)
        {
            var key = ExpectedAppSettingsKeyFor(toggle);

            ValidateKeyExists(key);

            var configValues = GetConfigValue(key).Split('|');

            var parser = new ConfigurationDateParser();

            var startDate = parser.ParseDateTimeConfigString(configValues[0].Trim(), key);
            var endDate = parser.ParseDateTimeConfigString(configValues[1].Trim(), key);

            var v = new ConfigurationValidator();

            v.ValidateStartAndEndDates(startDate, endDate, key);

            return new Tuple<DateTime, DateTime>(startDate, endDate);
        }

        private string ExpectedAppSettingsKeyFor(IFeatureToggle toggle)
        {
            var prefix = Configuration.UsePrefix ? ToggleConfigurationSettings.Prefix : string.Empty;
            return prefix + toggle.GetType().Name;
        }

        private void ValidateKeyExists(string key)
        {
            var configKeys = GetConfigKeysInSection();
            if (!configKeys.Contains(key))
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
            var configPkg = CodePackageActivationContextFactory().GetConfigurationPackageObject(Configuration.ConfigPackageName);
            return configPkg.Settings.Sections[Configuration.ConfigSectionName].Parameters[key].Value;
        }

        private string[] GetConfigKeysInSection()
        {
            var activationContext = CodePackageActivationContextFactory();

            if (!activationContext.GetConfigurationPackageNames().Contains(Configuration.ConfigPackageName))
                return new string[0];

            var settings = activationContext.GetConfigurationPackageObject(Configuration.ConfigPackageName).Settings;

            if(!settings.Sections.Contains(Configuration.ConfigSectionName))
                return new string[0];
            
            var section = settings.Sections[Configuration.ConfigSectionName];
            return section.Parameters.Select(p => p.Name).ToArray();
        }        
    }
}
