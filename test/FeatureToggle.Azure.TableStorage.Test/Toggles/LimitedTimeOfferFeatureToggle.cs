using FeatureToggle.Azure.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace FeatureToggle.Azure.TableStorage.Test.Toggles
{
    public class LimitedTimeOfferFeatureToggle : EnabledBetweenDatesFeatureToggle
    {
        public LimitedTimeOfferFeatureToggle()
        {
            this.ToggleValueProvider = new TableStorageProvider();
        }
    }
}
