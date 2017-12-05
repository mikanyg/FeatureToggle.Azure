using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Documents.Client;

namespace FeatureToggle.Azure.DocumentDB.Providers
{
    public class DocumentDbProvider : IBooleanToggleValueProvider
    {
        public static string AuthKey { get; set; }
        public static string ServiceEndpoint { get; set; }

        public static string DatabaseId { get; set; } = "FeatureToggle";
        public static string CollectionId { get; set; } = "Toggles";

        public bool EvaluateBooleanToggleValue(IFeatureToggle toggle)
        {
            // Get name

            // Validate connection properties

            // Get document
            var client = new DocumentClient(new Uri(ServiceEndpoint), AuthKey);
            
            return false;
        }
    }
}
