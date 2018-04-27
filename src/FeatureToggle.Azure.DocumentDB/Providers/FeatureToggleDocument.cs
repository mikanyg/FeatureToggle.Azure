using Newtonsoft.Json;
using System;

namespace FeatureToggle.Azure.Providers
{
    public abstract class FeatureToggleDocument
    {
        protected FeatureToggleDocument() { }

        protected FeatureToggleDocument(string toggleName)
        {
            Id = toggleName ?? throw new ArgumentNullException(nameof(toggleName));            
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }        
    }
}
