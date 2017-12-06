using System;

namespace FeatureToggle.Azure.TableStorage.Providers
{
    public class TableStorageConfiguration
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; } = "FeatureToggles";
        public bool AutoCreateTable { get; set; } = true;
        public bool AutoCreateFeature { get; set; } = true;
        public Func<IFeatureToggle, string> PartitionKeyResolver { get; set; } = toggle => toggle.GetType().Assembly.GetName().Name;
    }
}