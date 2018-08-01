using FeatureToggle.Azure.Providers;

namespace FeatureToggle.Azure.TableStorage.Test
{
    public static class TestConfig
    {
        public const string ValidConnectionString = "UseDevelopmentStorage=true";        

        public static TableStorageConfiguration ValidConfig(string connectionString)
        {
            return new TableStorageConfiguration
            {
                ConnectionString = connectionString
            };
        }
    }
}
