using System;
using System.Collections.Generic;
using System.Text;

namespace FeatureToggle.Azure.Providers
{
    public class EntityFrameworkCoreProvider : IBooleanToggleValueProvider
    {
        public bool EvaluateBooleanToggleValue(IFeatureToggle toggle)
        {
            throw new NotImplementedException();
        }
    }
}
