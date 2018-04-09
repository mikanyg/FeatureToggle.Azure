using System;

namespace FeatureToggle.Providers
{
    public class TableStorageConfiguration
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; } = "FeatureToggles";
        public bool AutoCreateTable { get; set; } = false;
        public bool AutoCreateFeature { get; set; } = false;
        public Func<IFeatureToggle, string> PartitionKeyResolver { get; set; } = toggle => toggle.GetType().Assembly.GetName().Name;
    }
}