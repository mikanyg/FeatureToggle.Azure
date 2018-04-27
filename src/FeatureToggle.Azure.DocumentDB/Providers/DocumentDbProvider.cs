using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace FeatureToggle.Azure.Providers
{
    public class DocumentDbProvider : IBooleanToggleValueProvider, IDateTimeToggleValueProvider
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
            return EvaluateToggleValue(toggle, document => document.Enabled, toggleName => new BooleanFeatureToggleDocument(toggleName));
        }

        public DateTime EvaluateDateTimeToggleValue(IFeatureToggle toggle)
        {
            return EvaluateToggleValue(toggle, document => document.Toggle, toggleName => new DateTimeFeatureToggleDocument(toggleName));
        }

        private TToggleValue EvaluateToggleValue<TDocument, TToggleValue>(IFeatureToggle toggle, Func<TDocument, TToggleValue> valueEvaluatorFunc, Func<string, TDocument> newDocumentFunc) where TDocument : FeatureToggleDocument
        {
            var client = GetDocumentClient();

            var toggleName = toggle.GetType().Name;

            TToggleValue toggleValue;

            try
            {
                // Fetch feature toggle
                var response = client.ReadDocumentAsync<TDocument>(
                    UriFactory.CreateDocumentUri(Configuration.DatabaseId, Configuration.CollectionId, toggleName)).Result;

                var document = response.Document;

                toggleValue = valueEvaluatorFunc(document);
            }
            catch (AggregateException ae) when (ae.InnerExceptions.First() is DocumentClientException ex && ex.StatusCode == HttpStatusCode.NotFound)
            {
                Trace.TraceWarning($"Feature toggle '{toggleName}' not found.");

                var newDocument = HandleMissingToggle(toggleName, () => newDocumentFunc(toggleName), client);
                toggleValue = valueEvaluatorFunc(newDocument);
            }

            return toggleValue;
        }

        private TDocument HandleMissingToggle<TDocument>(string toggleName, Func<TDocument> newDocumentFunc, DocumentClient client) where TDocument : FeatureToggleDocument
        {
            if (!Configuration.AutoCreateFeature)
                throw new ToggleConfigurationError($"Feature toggle '{toggleName}' does not exist. Either create it manually or configure the provider to auto create it.");

            Trace.TraceInformation($"AutoCreateFeature enabled, creating feature toggle '{toggleName}'.");

            var document = newDocumentFunc();
            var response = client.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(Configuration.DatabaseId, Configuration.CollectionId), document, disableAutomaticIdGeneration:true).Result;

            return document;
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
                var dbResponse = client.CreateDatabaseIfNotExistsAsync(new Database {Id = databaseId}).Result;
                var colResponse = client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(databaseId),
                    new DocumentCollection {Id = collectionId}).Result;
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
