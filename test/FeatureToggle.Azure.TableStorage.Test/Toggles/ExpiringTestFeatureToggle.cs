using FeatureToggle.Azure.Providers;

namespace FeatureToggle.Azure.TableStorage.Test.Toggles
{
    public class ExpiringTestFeatureToggle : EnabledOnOrBeforeDateFeatureToggle
    {
        public ExpiringTestFeatureToggle()
        {
            this.ToggleValueProvider = new TableStorageProvider();
        }
    }
}
