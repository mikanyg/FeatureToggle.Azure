﻿using FeatureToggle.Azure.Providers;

namespace FeatureToggle.Azure.ServiceFabric.Test.Toggles
{
    public class ExpiringTestFeatureToggle : EnabledOnOrBeforeDateFeatureToggle
    {
        public ExpiringTestFeatureToggle()
        {
            this.ToggleValueProvider = new ServiceFabricConfigProvider();
        }
    }
}
