namespace FeatureToggle.Azure.Providers
{
    public class BooleanFeatureToggleEntity : FeatureToggleEntity
    {
        public BooleanFeatureToggleEntity() : base() { }

        public BooleanFeatureToggleEntity(string componentName, string toggleName) : base(componentName, toggleName)
        {
            Enabled = false;
        }

        public bool Enabled { get; set; }
    }
}
