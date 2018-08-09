using FeatureToggle.Azure.Providers;

namespace FeatureToggle.Azure.ServiceFabric.Test.Toggles
{
    public class ComingSoonTestFeatureToggle : EnabledOnOrAfterDateFeatureToggle
    {
        public ComingSoonTestFeatureToggle()
        {
            this.ToggleValueProvider = new ServiceFabricConfigProvider();
        }
    }
}
