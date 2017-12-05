using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace FeatureToggle.Azure.TableStorage.Providers
{
    public class FeatureToggleEntity : TableEntity
    {
        public bool Enabled { get; set; }
    }
}
