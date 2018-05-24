using FeatureToggle;
using FeatureToggle.Azure.Providers;

namespace SfWebAppCore.FeatureToggles
{
    public class RetiringSoonFeature : EnabledOnOrBeforeDateFeatureToggle
    {
        public RetiringSoonFeature()
        {
            this.ToggleValueProvider = new ServiceFabricConfigProvider();
        }
    }
}