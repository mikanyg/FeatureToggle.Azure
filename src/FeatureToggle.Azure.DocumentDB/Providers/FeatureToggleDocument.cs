using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FeatureToggle.Azure.DocumentDB.Providers
{
    public class FeatureToggleDocument
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public bool Enabled { get; set; }
    }
}
