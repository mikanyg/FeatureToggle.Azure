using FeatureToggle;
using FeatureToggle.Azure.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.FeatureToggles
{
    public class ComingSoonFeature : EnabledOnOrAfterDateFeatureToggle
    {
        public ComingSoonFeature()
        {
            this.ToggleValueProvider = new DocumentDbProvider();
        }
    }
}