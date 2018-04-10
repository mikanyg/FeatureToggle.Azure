using FeatureToggle.Azure.Providers;

namespace FeatureToggle.Azure.Toggles
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
