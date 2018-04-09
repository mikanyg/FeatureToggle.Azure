using FeatureToggle.Azure.TableStorage.Providers;

namespace FeatureToggle.Azure.TableStorage
{
    public abstract class TableStorageToggle : IFeatureToggle
    {
        protected TableStorageToggle()
        {
            ToggleValueProvider = new TableStorageProvider();
        }

        public virtual IBooleanToggleValueProvider ToggleValueProvider { get; set; }

        public bool FeatureEnabled => ToggleValueProvider.EvaluateBooleanToggleValue(this);
    }
}
