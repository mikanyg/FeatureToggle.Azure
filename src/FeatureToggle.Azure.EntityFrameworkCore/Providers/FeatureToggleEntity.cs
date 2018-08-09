using System;

namespace FeatureToggle.Azure.Providers
{
    public abstract class FeatureToggleEntity 
    {
        protected FeatureToggleEntity() { }

        protected FeatureToggleEntity(string toggleName)
        {
            FeatureToggle = toggleName ?? throw new ArgumentNullException(nameof(toggleName));
        }        

        public int Id { get; set; }

        public string FeatureToggle { get; private set; }
    }
}
