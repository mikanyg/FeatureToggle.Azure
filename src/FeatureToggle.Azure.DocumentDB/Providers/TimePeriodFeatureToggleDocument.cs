using System;

namespace FeatureToggle.Azure.Providers
{
    public class TimePeriodFeatureToggleDocument : FeatureToggleDocument
    {
        public TimePeriodFeatureToggleDocument() : base() { }

        public TimePeriodFeatureToggleDocument(string toggleName) : base(toggleName)
        {
            EnabledFrom = default(DateTime);
            EnabledTo = default(DateTime);
        }

        public DateTime EnabledFrom { get; set; }
        public DateTime EnabledTo { get; set; }
    }
}
