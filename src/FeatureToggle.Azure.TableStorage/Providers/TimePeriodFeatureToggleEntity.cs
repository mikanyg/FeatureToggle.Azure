using System;

namespace FeatureToggle.Azure.Providers
{
    public class TimePeriodFeatureToggleEntity : FeatureToggleEntity
    {
        public TimePeriodFeatureToggleEntity() : base() { }

        public TimePeriodFeatureToggleEntity(string componentName, string toggleName) : base(componentName, toggleName)
        {
            // Table Storage does not support DateTime.Min
            Start = new DateTime(1900, 1, 1); 
            End = new DateTime(1900, 1, 1); 
        }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
