using System;
using System.Collections.Generic;
using System.Text;
using FeatureToggle.Azure.DocumentDB.Providers;

namespace FeatureToggle.Azure.DocumentDB
{
    public abstract class DocDbFeatureToggle : IFeatureToggle
    {
        protected DocDbFeatureToggle()
        {
            ToggleValueProvider = new DocumentDbFeatureToggleProvider();
        }

        public virtual IBooleanToggleValueProvider ToggleValueProvider { get; set; }

        public bool FeatureEnabled => ToggleValueProvider.EvaluateBooleanToggleValue(this);
    }
}
