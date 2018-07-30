using FeatureToggle.Azure.Providers;

namespace FeatureToggle.Azure.DocumentDB.Test.Toggles
{
    public class ComingSoonTestFeatureToggle : EnabledOnOrAfterDateFeatureToggle
    {
        public ComingSoonTestFeatureToggle()
        {
            this.ToggleValueProvider = new DocumentDbProvider();
        }
    }
}
