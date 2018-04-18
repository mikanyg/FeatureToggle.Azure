# FeatureToggle.Azure
FeatureToggle.Azure is a collection of [FeatureToggle](https://github.com/jason-roberts/FeatureToggle) providers for various Azure services. The providers support centralized storage of feature toggles in Azure. For more information on how to implement and leverage features toggles, using FeatureToggle, see the [official docs for FeatureToggle](http://jason-roberts.github.io/FeatureToggle.Docs)

## List of Providers
- FeatureToggle Provider for Azure DocumentDB, ([learn more](#featuretoggleazuredocumentdb))
- FeatureToggle Provider for Azure Table storage, ([learn more](#featuretoggleazuretablestorage))
- FeatureToggle Provider for Service Fabric configuration packages, ([learn more](#featuretoggleazureservicefabric))

## FeatureToggle.Azure.DocumentDB
FeatureToggle provider for storing feature toggles as simple json documents in Azure DocumentDB (part of Azure Cosmos DB). The nuget package can be found on [nuget.org](https://www.nuget.org/packages/FeatureToggle.Azure.DocumentDB/).

### Getting started
1. Install `FeatureToggle.Azure.DocumentDB` from nuget into your project.
2. Configure `DocumentDbProvider` at application startup, such as in Global.asax or Startup.cs by specifying the DocumentDb service uri and authentication key needed to store and retreive features toggles in DocumentDB. Global.asax example:
```c#
protected void Application_Start()
{
    // other startup configuration
    DocumentDbProvider.Configure("https://[serviceuri]", "[authkey]");
}
```
3. Add a class to your project representing the feature that needs to be controlled, e.g. `PrintFeature` and inherit from `DocumentDbToggle`
```c#
public class PrintFeature : DocumentDbToggle
{
}
```
4. Use the new feature toggle in your code to isolate features.
```c#
public ActionResult Index()
{
    ViewBag.EnablePrint = Is<PrintFeature>.Enabled;
    return View();
}
```
5. Add a json document in DocumentDB, either using the Data Explorer in the Azure Portal or through the client API of DocumentDB in order to turn on/off the feature. The document looks like this:
```json
{
   "id": "PrintFeature",
   "Enabled": false
}
```

### Provider Configuration 
In order to control how the `DocumentDbProvider` fetches feature toggles from DocumentDb, the provider can be configured through an instance of `DocumentDbConfiguration` passed to the overloaded static method `DocumentDbProvider.Configure(docDbConfig)`. 

The following options can be controlled through the configuration instance:
- `AuthKey` The authorization key for the Cosmos DB account.
- `ServiceEndpoint` The service endpoint for the Cosmos DB account.
- `DatabaseId` Database id (name) where toggles are stored. Defaults to **FeatureToggle**
- `CollectionId` Document Collection id (name) where toggles are stored. Defaults to **Toggles**
- `AutoCreateDatabaseAndCollection` Enable auto creation of database and collection for storing toggles. When set to true, the AuthKey must have permission to create database and collections. Defaults to **false**
- `AutoCreateFeature` Enable auto creation of toggles as a json documents. When set to true, the AuthKey must have write permissions to the document collection. Default to **false**

## FeatureToggle.Azure.TableStorage
FeatureToggle provider for storing feature toggles in Azure Table storage, can be found on [nuget.org](https://www.nuget.org/packages/FeatureToggle.Azure.TableStorage/).
### Getting started
[getting started to be added]

### Provider Configuration 
[description of provider configuration to be added]

## FeatureToggle.Azure.ServiceFabric
FeatureToggle provider for storing feature toggles in Service Fabric configuration packages, can be found on [nuget.org](https://www.nuget.org/packages/FeatureToggle.Azure.ServiceFabric/).
### Getting started
[getting started to be added]

### Provider Configuration 
[description of provider configuration to be added]

## Samples
[walkthrough of sample to be added]
