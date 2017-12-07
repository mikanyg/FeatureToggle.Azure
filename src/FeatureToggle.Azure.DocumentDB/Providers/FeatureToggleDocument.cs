using Newtonsoft.Json;
using System;

namespace FeatureToggle.Azure.DocumentDB.Providers
{
    public class FeatureToggleDocument
    {
        public FeatureToggleDocument() { }

        public FeatureToggleDocument(string toggleName)
        {
            Id = toggleName ?? throw new ArgumentNullException(nameof(toggleName));
            Enabled = false;
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public bool Enabled { get; set; }
    }
}
