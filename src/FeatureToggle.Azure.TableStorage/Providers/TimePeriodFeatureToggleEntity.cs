using System;

namespace FeatureToggle.Azure.Providers
{
    public class TimePeriodFeatureToggleEntity : FeatureToggleEntity
    {
        public TimePeriodFeatureToggleEntity() : base() { }

        public TimePeriodFeatureToggleEntity(string componentName, string toggleName) : base(componentName, toggleName)
        {
            // Table Storage does not support DateTime.Min
            EnabledFrom = new DateTime(1900, 1, 1); 
            EnabledTo = new DateTime(1900, 1, 1); 
        }

        public DateTime EnabledFrom { get; set; }
        public DateTime EnabledTo { get; set; }
    }
}
