using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FeatureToggle.Azure.DocumentDB.Providers;
using FeatureToggle.Azure.TableStorage.Providers;

namespace WebApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            TableStorageFeatureToggleProvider.Configuration.ConnectionString = "UseDevelopmentStorage=true";

            DocumentDbFeatureToggleProvider.Configuration.ServiceEndpoint = "https://localhost:8081";
            DocumentDbFeatureToggleProvider.Configuration.AuthKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
            DocumentDbFeatureToggleProvider.Configuration.AutoCreateDatabaseAndCollection = true;
            DocumentDbFeatureToggleProvider.Configuration.AutoCreateFeature = true;

        }
    }
}
