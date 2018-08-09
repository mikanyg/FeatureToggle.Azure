using System;

namespace FeatureToggle.Azure.Providers
{
    public class DateTimeFeatureToggleEntity : FeatureToggleEntity
    {
        public DateTimeFeatureToggleEntity() : base() { }

        public DateTimeFeatureToggleEntity(string toggleName) : base(toggleName)
        {            
        }

        public DateTime ToggleTimestamp { get; set; }
    }
}
