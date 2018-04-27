using System;

namespace FeatureToggle.Azure.Providers
{
    public class DateTimeFeatureToggleEntity : FeatureToggleEntity
    {
        public DateTimeFeatureToggleEntity() : base() { }

        public DateTimeFeatureToggleEntity(string componentName, string toggleName) : base(componentName, toggleName)
        {
            ToggleTimestamp = new DateTime(1900, 1, 1); // Table Storage does not support DateTime.Min
        }

        public DateTime ToggleTimestamp { get; set; }
    }
}
