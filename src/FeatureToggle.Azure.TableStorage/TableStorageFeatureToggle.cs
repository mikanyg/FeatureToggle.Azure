using System;
using System.Collections.Generic;
using System.Text;
using FeatureToggle.Azure.TableStorage.Providers;

namespace FeatureToggle.Azure.TableStorage
{
    public abstract class TableStorageFeatureToggle : IFeatureToggle
    {
        protected TableStorageFeatureToggle()
        {
            ToggleValueProvider = new TableStorageFeatureToggleProvider();
        }

        public virtual IBooleanToggleValueProvider ToggleValueProvider { get; set; }

        public bool FeatureEnabled => ToggleValueProvider.EvaluateBooleanToggleValue(this);
    }
}
