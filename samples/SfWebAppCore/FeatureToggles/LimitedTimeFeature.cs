using FeatureToggle;
using FeatureToggle.Azure.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SfWebAppCore.FeatureToggles
{
    public class LimitedTimeFeature : EnabledBetweenDatesFeatureToggle
    {
        public LimitedTimeFeature()
        {
            this.ToggleValueProvider = new ServiceFabricConfigProvider();
        }
    }
}
