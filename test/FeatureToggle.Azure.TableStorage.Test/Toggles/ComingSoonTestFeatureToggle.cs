using FeatureToggle.Azure.Providers;

namespace FeatureToggle.Azure.TableStorage.Test.Toggles
{
    public class ComingSoonTestFeatureToggle : EnabledOnOrAfterDateFeatureToggle
    {
        public ComingSoonTestFeatureToggle()
        {
            this.ToggleValueProvider = new TableStorageProvider();
        }
    }
}
