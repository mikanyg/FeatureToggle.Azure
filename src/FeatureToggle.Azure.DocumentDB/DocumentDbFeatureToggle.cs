using System;
using System.Collections.Generic;
using System.Text;
using FeatureToggle.Azure.DocumentDB.Providers;

namespace FeatureToggle.Azure.DocumentDB
{
    public abstract class DocumentDbFeatureToggle : IFeatureToggle
    {
        protected DocumentDbFeatureToggle()
        {
            ToggleValueProvider = new DocumentDbProvider();
        }

        public virtual IBooleanToggleValueProvider ToggleValueProvider { get; set; }

        public bool FeatureEnabled => ToggleValueProvider.EvaluateBooleanToggleValue(this);
    }
}
