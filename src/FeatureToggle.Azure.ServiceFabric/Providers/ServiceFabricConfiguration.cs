namespace FeatureToggle.Azure.Providers
{
    public class ServiceFabricConfiguration
    {
        public string ConfigPackageName { get; set; } = "Config";
        public string ConfigSectionName { get; set; } = "Features";
        public bool UsePrefix { get; set; } = true;
    }
}
