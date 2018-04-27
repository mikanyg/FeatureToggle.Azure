using System;

namespace FeatureToggle.Azure.Providers
{
    public class DateTimeFeatureToggleDocument : FeatureToggleDocument
    {
        public DateTimeFeatureToggleDocument() : base() { }

        public DateTimeFeatureToggleDocument(string toggleName) : base(toggleName)
        {            
            Toggle = default(DateTime);
        }
                
        public DateTime Toggle { get; set; }
    }
}
