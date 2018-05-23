using System;

namespace FeatureToggle.Azure.Providers
{
    public class DateTimeFeatureToggleDocument : FeatureToggleDocument
    {
        public DateTimeFeatureToggleDocument() : base() { }

        public DateTimeFeatureToggleDocument(string toggleName) : base(toggleName)
        {            
            ToggleTimestamp = default(DateTime);
        }
                
        public DateTime ToggleTimestamp { get; set; }
    }
}
