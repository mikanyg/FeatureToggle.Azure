using FeatureToggle.Azure.Providers;

namespace FeatureToggle.Azure.DocumentDB.Test
{
    public static class TestConfig
    {
        public const string ValidEndpoint = "https://localhost:8081";
        public const string ValidAuthKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";        

        public static DocumentDbConfiguration ValidConfig(string databaseId, string collectionId)
        {
            return new DocumentDbConfiguration
            {
                ServiceEndpoint = ValidEndpoint,
                AuthKey = ValidAuthKey,
                DatabaseId = databaseId,
                CollectionId = collectionId
            };
        }
    }
}
