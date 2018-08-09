using FeatureToggle.Azure.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace FeatureToggle.Azure.Toggles
{
    public abstract class EFCoreToggle : IFeatureToggle
    {
        protected EFCoreToggle()
        {
            ToggleValueProvider = new EntityFrameworkCoreProvider();
        }

        public virtual IBooleanToggleValueProvider ToggleValueProvider { get; set; }

        public bool FeatureEnabled => ToggleValueProvider.EvaluateBooleanToggleValue(this);
    }
}
