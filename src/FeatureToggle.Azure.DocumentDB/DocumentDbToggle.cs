using FeatureToggle.Azure.Providers;

namespace FeatureToggle.Azure.Toggles
{
    public abstract class DocumentDbToggle : IFeatureToggle
    {
        protected DocumentDbToggle()
        {
            ToggleValueProvider = new DocumentDbProvider();
        }

        public virtual IBooleanToggleValueProvider ToggleValueProvider { get; set; }

        public bool FeatureEnabled => ToggleValueProvider.EvaluateBooleanToggleValue(this);
    }
}
