using System;

namespace FeatureToggle.Azure.Providers
{
    public class DateTimeFeatureToggleEntity : FeatureToggleEntity
    {
        public DateTimeFeatureToggleEntity() : base() { }

        public DateTimeFeatureToggleEntity(string componentName, string toggleName) : base(componentName, toggleName)
        {
            Toggle = default(DateTime);
        }

        public DateTime Toggle { get; set; }
    }
}
