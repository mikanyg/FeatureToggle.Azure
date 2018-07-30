using FeatureToggle.Azure.Providers;

namespace FeatureToggle.Azure.DocumentDB.Test.Toggles
{
    public class ExpiringTestFeatureToggle : EnabledOnOrBeforeDateFeatureToggle
    {
        public ExpiringTestFeatureToggle()
        {
            this.ToggleValueProvider = new DocumentDbProvider();
        }
    }
}
