﻿using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace FeatureToggle.Azure.TableStorage.Providers
{
    public class FeatureToggleEntity : TableEntity
    {
        public FeatureToggleEntity() { }

        public FeatureToggleEntity(string componentName, string toggleName)
        {
            PartitionKey = componentName ?? throw new ArgumentNullException(nameof(componentName));
            RowKey = toggleName ?? throw new ArgumentNullException(nameof(toggleName));
            Enabled = false;
        }

        public bool Enabled { get; set; }
    }
}
