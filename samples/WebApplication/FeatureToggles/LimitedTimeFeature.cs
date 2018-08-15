using FeatureToggle;
using FeatureToggle.Azure.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.FeatureToggles
{
    public class LimitedTimeFeature : EnabledBetweenDatesFeatureToggle
    {
        public LimitedTimeFeature()
        {
            this.ToggleValueProvider = new TableStorageProvider(); //NOTE: Could also be DocumentDbProvider
        }
    }
}