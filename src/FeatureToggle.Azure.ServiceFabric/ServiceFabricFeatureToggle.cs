using System;
using System.Collections.Generic;
using System.Text;
using FeatureToggle.Azure.ServiceFabric.Providers;

namespace FeatureToggle.Azure.ServiceFabric
{
    public abstract class ServiceFabricFeatureToggle : IFeatureToggle
    {
        protected ServiceFabricFeatureToggle()
        {
            ToggleValueProvider = new ServiceFabricConfigProvider();
        }

        public virtual IBooleanToggleValueProvider ToggleValueProvider { get; set; }

        public bool FeatureEnabled => ToggleValueProvider.EvaluateBooleanToggleValue(this);
    }
}
