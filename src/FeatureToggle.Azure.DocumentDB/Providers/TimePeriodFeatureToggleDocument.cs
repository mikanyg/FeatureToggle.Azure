using System;

namespace FeatureToggle.Azure.Providers
{
    public class TimePeriodFeatureToggleDocument : FeatureToggleDocument
    {
        public TimePeriodFeatureToggleDocument() : base() { }

        public TimePeriodFeatureToggleDocument(string toggleName) : base(toggleName)
        {
            Start = default(DateTime);
            End = default(DateTime);
        }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
