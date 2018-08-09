using FeatureToggle.Azure.Providers;
using NUnit.Framework;

namespace FeatureToggle.Azure.ServiceFabric.Test
{
    [TestFixture]
    public abstract class ServiceFabricTestFixture
    {
        [SetUp]
        public void BaseSetup()
        {
            ServiceFabricConfigProvider.Configure(new ServiceFabricConfiguration());
            ServiceFabricConfigProvider.SetCodePackageActivationContextFactory(TestConfig.CreateMockCodePackageActivationContext);
        }

        protected void SetToggleInConfig(string name, string value)
        {
            TestConfig.ToggleName = name;
            TestConfig.ToggleValue = value;
        }
    }
}
