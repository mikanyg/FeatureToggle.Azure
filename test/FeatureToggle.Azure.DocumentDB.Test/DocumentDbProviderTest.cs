using FeatureToggle.Azure.Providers;
using FeatureToggle.Azure.Toggles;
using Microsoft.Azure.Documents.Client;
using Moq;
using Shouldly;
using System;
using Xunit;

namespace FeatureToggle.Azure.Test
{
    public class DocumentDbProviderTest
    {
        class TestableProvider : DocumentDbProvider
        {
            protected override DocumentClient CreateDocumentClient()
            {
                return new Mock<DocumentClient>().Object;
            }
        }

        class TestToggle : DocumentDbToggle
        {
            public TestToggle()
            {
                ToggleValueProvider =  new TestableProvider();
            }
        }

        [Fact]
        public void Test1()
        {
            DocumentDbProvider.Configure("someendpont", "someauthkey");
            var toggle = new TestToggle();

            toggle.FeatureEnabled.ShouldBeTrue();
        }
    }
}
