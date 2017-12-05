using System;
using System.Collections.Generic;
using System.Text;

namespace FeatureToggle.Azure.DocumentDB.Providers
{
    public class DocumentDbProvider : IBooleanToggleValueProvider
    {
        public bool EvaluateBooleanToggleValue(IFeatureToggle toggle)
        {
            throw new NotImplementedException();
        }
    }
}
