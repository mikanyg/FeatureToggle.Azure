namespace FeatureToggle.Azure.Providers
{
    public class BooleanFeatureToggleEntity : FeatureToggleEntity
    {
        public BooleanFeatureToggleEntity() : base() { }

        public BooleanFeatureToggleEntity(string toggleName) : base(toggleName)
        {
            Enabled = false;
        }

        public bool Enabled { get; set; }
    }
}
