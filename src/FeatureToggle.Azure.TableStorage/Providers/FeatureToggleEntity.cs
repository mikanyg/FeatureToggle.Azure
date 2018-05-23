using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace FeatureToggle.Azure.Providers
{
    public abstract class FeatureToggleEntity : TableEntity
    {
        protected FeatureToggleEntity() { }

        protected FeatureToggleEntity(string componentName, string toggleName)
        {
            PartitionKey = componentName ?? throw new ArgumentNullException(nameof(componentName));
            RowKey = toggleName ?? throw new ArgumentNullException(nameof(toggleName));            
        }        
    }
}
