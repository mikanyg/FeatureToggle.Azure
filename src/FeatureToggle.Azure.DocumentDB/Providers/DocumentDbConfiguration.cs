using System;
using System.Collections.Generic;
using System.Text;

namespace FeatureToggle.Azure.Providers
{
    public class DocumentDbConfiguration
    {
        public string AuthKey { get; set; }
        public string ServiceEndpoint { get; set; }
        public string DatabaseId { get; set; } = "FeatureToggle";
        public string CollectionId { get; set; } = "Toggles";
        public bool AutoCreateDatabaseAndCollection { get; set; } = false;
        public bool AutoCreateFeature { get; set; } = false;
    }
}
