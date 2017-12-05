using System;
using System.Collections.Generic;
using System.Text;

namespace FeatureToggle.Azure.TableStorage.Providers
{
    public class TableStorageProvider : IBooleanToggleValueProvider
    {
        public static string ConnectionString { get; set; }

        public static string TableName { get; set; } = "FeatureToggles";

        public bool EvaluateBooleanToggleValue(IFeatureToggle toggle)
        {
            throw new NotImplementedException();
        }
    }
}
