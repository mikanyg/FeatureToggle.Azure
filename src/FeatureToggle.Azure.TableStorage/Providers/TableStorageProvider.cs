using System;
using System.Collections.Generic;
using System.Text;

namespace FeatureToggle.Azure.TableStorage.Providers
{
    public class TableStorageProvider : IBooleanToggleValueProvider
    {
        public bool EvaluateBooleanToggleValue(IFeatureToggle toggle)
        {
            throw new NotImplementedException();
        }
    }
}
