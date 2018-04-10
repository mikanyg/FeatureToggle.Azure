using FeatureToggle.Azure.Providers;

namespace FeatureToggle.Azure.Toggles
{
    public abstract class ServiceFabricToggle : IFeatureToggle
    {
        protected ServiceFabricToggle()
        {
            ToggleValueProvider = new ServiceFabricConfigProvider();
        }

        public virtual IBooleanToggleValueProvider ToggleValueProvider { get; set; }

        public bool FeatureEnabled => ToggleValueProvider.EvaluateBooleanToggleValue(this);
    }
}
