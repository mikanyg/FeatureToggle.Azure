using FeatureToggle;
using FeatureToggle.Azure.Providers;

namespace WebApplication.FeatureToggles
{
    public class RetiringSoonFeature : EnabledOnOrBeforeDateFeatureToggle
    {
        public RetiringSoonFeature()
        {
            this.ToggleValueProvider = new TableStorageProvider();
        }
    }
}