using FeatureToggle.Azure.DocumentDB.Providers;

namespace FeatureToggle.Azure.DocumentDB
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
