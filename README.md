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
5. Add a json document in DocumentDB, either using the Data Explorer in the Azure Portal or the standalone Azure Storage Explorer (ASE) or through the client API of DocumentDB in order to turn on/off the feature. The document looks like this:
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
- `DatabaseId` Database id (name) where toggles are stored. Defaults to **FeatureToggle**.
- `CollectionId` Document Collection id (name) where toggles are stored. Defaults to **Toggles**.
- `AutoCreateDatabaseAndCollection` Enable auto creation of database and collection for storing toggles. When set to true, the AuthKey must have permission to create database and collections. Defaults to **false**.
- `AutoCreateFeature` Enable auto creation of toggles as a json documents. When set to true, the AuthKey must have write permissions to the document collection. Defaults to **false**.

**NOTE: The id property the json document representing the feature toggle must match the name of the class representing the feature toggle.**

## FeatureToggle.Azure.TableStorage
FeatureToggle provider for storing feature toggles as table entities in Azure Table storage (part of Azure Storage Accounts). The nuget package can be found on [nuget.org](https://www.nuget.org/packages/FeatureToggle.Azure.TableStorage/).
### Getting started
1. Install `FeatureToggle.Azure.TableStorage` from nuget into your project.
2. Configure `TableStorageProvider` at application startup, such as in Global.asax or Startup.cs by specifying the connection string for the Azure Storage Account that will be used to store and retreive features toggles in Table storage. Global.asax example:
```c#
protected void Application_Start()
{
    // other startup configuration
    TableStorageProvider.Configure("[azurestorage_connectionstring]");
}
```
3. Add a class to your project representing the feature that needs to be controlled, e.g. `PrintFeature` and inherit from `TableStorageToggle`
```c#
public class PrintFeature : TableStorageToggle
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
5. Add a table entity in Table storage, either using the Cloud Explorer in the Visual Studio or the standalone Azure Storage Explorer (ASE) or through the client API of Table storage in order to turn on/off the feature. The table entity must have the following properties:
- *PartitionKey:* Value must be the assembly name containing the feature toggle, unless overriden in configuration, see below.
- *RowKey:* Value must be the class name of the feature toggle.
- *Enabled:* true/false boolean value.
### Provider Configuration 
In order to control how the `TableStorageProvider` fetches feature toggles from Table Storage, the provider can be configured through an instance of `TableStorageConfiguration` passed to the overloaded static method `TableStorageProvider.Configure(tableStorageconfig)`.

The following options can be controlled through the configuration instance:
- `ConnectionString` The connection string to the storage account for Azure Table Storage.
- `TableName` The table name where toggles are stored. Defaults to **FeatureToggles**.
- `AutoCreateTable` Enable auto creation of the table storing toggles. When set to true, the connection string SAS token must have permission to create tables. Defaults to **false**.
- `AutoCreateFeature` Enable auto creation of toggles as table entities. When set to true, the connection string SAS token must have write permissions to the table. Defaults to **false**.
- `PartitionKeyResolver` Provides the option to define the table partition key through a function. Defaults to the assembly name  containing the feature toggle.

**NOTE: The RowKey of the table entity representing the feature toggle must match the name of the class representing the feature toggle.**

## FeatureToggle.Azure.ServiceFabric
FeatureToggle provider for storing feature toggles in Service Fabric configuration packages (Settings.xml files). The nuget package can be found on [nuget.org](https://www.nuget.org/packages/FeatureToggle.Azure.ServiceFabric/).

### Getting started
1. Install `FeatureToggle.Azure.ServiceFabric` from nuget into your project.
2. Add a class to your project representing the feature that needs to be controlled, e.g. `CoolNewFeatureToggle` and inherit from `ServiceFabricToggle`
```c#
public class CoolNewFeatureToggle : ServiceFabricToggle
{
}
```
3. Use the new feature toggle in your code to isolate features.
```c#
public IActionResult Index()
{
    ViewData["Message"] = Is<CoolNewFeatureToggle>.Enabled ? "Cool Feature enabled" :-D" : "No cool feature for U :-["; 
    return View();
}
```
4. Add a configuration section and parameter to the Settings.xml file in PackageRoot\Config folder in order to turn on/off the feature. Like so: 
```xml
<Section Name="Features">
    <Parameter Name="FeatureToggle.CoolNewFeatureToggle" Value="true" />
</Section>
```
### Provider Configuration 
If needed it is possible to control how and where the `ServiceFabricConfigProvider` fetches features toggles from SF configuration packages. The provider can be configured using the static method `Configure` which must be done when the Service Fabric service starts. The following parameters can be set:
- `configPackageName` The name of the Service Fabric configuration package. Defaults to **Config**.
- `configSectionName` The configuration section name in Settings.xml that hold feature toggles. Defaults to **Features**.
- `usePrefix` Controls whether features toggles must be prefixed with **FeatureToggle.** Defaults to **true**.

**NOTE: The parameter name attribute in Settings.xml representing the feature toggle must match the name of the class representing the feature toggle, and unless disabled (see above) it must be prefixed with FeatureToggle.**

## Samples
[walkthrough of sample to be added]
