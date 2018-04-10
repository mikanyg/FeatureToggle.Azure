using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace FeatureToggle.Azure.Providers
{
    public class DocumentDbProvider : IBooleanToggleValueProvider
    {
        private static bool _collectionVerified;

        private static DocumentDbConfiguration Configuration { get; set; } = new DocumentDbConfiguration();

        public static void Configure(string serviceEndpoint, string authKey)
        {
            if (string.IsNullOrEmpty(serviceEndpoint))
            {
                throw new ArgumentException("value cannot be null or empty", nameof(serviceEndpoint));
            }

            if (string.IsNullOrEmpty(authKey))
            {
                throw new ArgumentException("value cannot be null or empty", nameof(authKey));
            }

            Configure(new DocumentDbConfiguration { ServiceEndpoint = serviceEndpoint, AuthKey = authKey });
        }

        public static void Configure(DocumentDbConfiguration config)
        {
            Configuration = config ?? throw new ArgumentNullException(nameof(config));
        }

        public bool EvaluateBooleanToggleValue(IFeatureToggle toggle)
        {
            var client = GetDocumentClient();

            var toggleName = toggle.GetType().Name;

            bool toggleValue;

            try
            {
                // Fetch feature toggle
                var reponse = client.ReadDocumentAsync<FeatureToggleDocument>(
                    UriFactory.CreateDocumentUri(Configuration.DatabaseId, Configuration.CollectionId, toggleName)).Result;

                toggleValue = reponse.Document.Enabled;
            }
            catch (AggregateException ae) when (ae.InnerExceptions.First() is DocumentClientException ex && ex.StatusCode == HttpStatusCode.NotFound)
            {
                Trace.TraceWarning($"Feature toggle '{toggleName}' not found.");

                toggleValue = HandleMissingToggle(toggleName, client);
            }
            
            return toggleValue;
        }

        private bool HandleMissingToggle(string toggleName, DocumentClient client)
        {
            if (!Configuration.AutoCreateFeature)
                throw new ToggleConfigurationError($"Feature toggle '{toggleName}' does not exist. Either create it manually or configure the provider to auto create it.");

            Trace.TraceInformation($"AutoCreateFeature enabled, creating feature toggle '{toggleName}'.");

            var document = new FeatureToggleDocument(toggleName);
            var response = client.CreateDocumentAsync(
                UriFactory.CreateDocumentUri(Configuration.DatabaseId, Configuration.CollectionId, toggleName), document, disableAutomaticIdGeneration:true).Result;

            return document.Enabled;
        }

        private DocumentClient GetDocumentClient()
        {
            if (string.IsNullOrWhiteSpace(Configuration.ServiceEndpoint) || string.IsNullOrWhiteSpace(Configuration.AuthKey))
                throw new ToggleConfigurationError($"DocumentDB service endpoint or auth key not set. Please configure {nameof(DocumentDbProvider)} at application startup.");

            var client = new DocumentClient(new Uri(Configuration.ServiceEndpoint), Configuration.AuthKey);

            if (_collectionVerified) return client;

            var databaseId = Configuration.DatabaseId;
            var collectionId = Configuration.CollectionId;

            if (Configuration.AutoCreateDatabaseAndCollection)
            {
                client.CreateDatabaseIfNotExistsAsync(new Database {Id = databaseId});
                client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(databaseId),
                    new DocumentCollection {Id = collectionId});
            }
            else
            {
                try
                {
                    var collection = client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId)).Result;
                }
                catch (AggregateException ae) when (ae.InnerExceptions.First() is DocumentClientException ex && ex.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new ToggleConfigurationError($"Database '{Configuration.DatabaseId}' and DocumentCollection '{Configuration.CollectionId}' could not be verified. Either create them manually or configure the provider to auto create them.");
                }
            }
            _collectionVerified = true;

            return client;
        }
    }
}
