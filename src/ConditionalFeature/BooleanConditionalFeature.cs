using System;

namespace MicroKnights.ConditionalFeature
{
    public class BooleanConditionalFeature : ConditionalFeature<bool>
    {
        protected BooleanConditionalFeature(bool defaultValue = default(bool)) : base(defaultValue)
        {
        }

        public BooleanConditionalFeature(Func<bool> funcResolveValue, bool defaultValue = default(bool)) : base(funcResolveValue, defaultValue)
        {
        }

        public BooleanConditionalFeature(Lazy<bool> lazyResolveValue, bool defaultValue = default(bool)) : base(lazyResolveValue, defaultValue)
        {
        }

        public bool IsEnabled => FeatureValue == true;
        public bool IsDisabled => FeatureValue == false;

        public void OnEnabled(Action enabledAction)
        {
            if (IsEnabled)
            {
                enabledAction();
            }
        }

        public void OnDisabled(Action disabledAction)
        {
            if (IsDisabled)
            {
                disabledAction();
            }
        }
    }
}