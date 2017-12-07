using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace FeatureToggle.Azure.DocumentDB.Providers
{
    public class DocumentDbFeatureToggleProvider : IBooleanToggleValueProvider
    {
        private static bool _collectionVerified;
        public static DocumentDbConfiguration Configuration { get; internal set; } = new DocumentDbConfiguration();

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
                UriFactory.CreateDocumentUri(Configuration.DatabaseId, Configuration.CollectionId, toggleName), document).Result;

            return document.Enabled;
        }

        private DocumentClient GetDocumentClient()
        {
            if (string.IsNullOrWhiteSpace(Configuration.ServiceEndpoint) || string.IsNullOrWhiteSpace(Configuration.AuthKey))
                throw new ToggleConfigurationError($"DocumentDB service endpoint or auth key not set. Please configure {nameof(DocumentDbFeatureToggleProvider)} at application startup.");

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
                catch (Exception e)
                {
                    throw new ToggleConfigurationError($"Database '{Configuration.DatabaseId}' and DocumentCollection '{Configuration.CollectionId}' could not be verified. Either create them manually or configure the provider to auto create them.", e);
                }
            }
            _collectionVerified = true;

            return client;
        }
    }
}
