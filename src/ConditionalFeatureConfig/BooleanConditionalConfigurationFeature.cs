using System;
using Microsoft.Extensions.Configuration;

namespace MicroKnights.ConditionalFeature.Configuation
{
    public abstract class BooleanConditionalConfigurationFeature : BooleanConditionalFeature
    {
        private readonly IConfiguration _configuration;
        private readonly bool _defaultSet;

        protected BooleanConditionalConfigurationFeature(IConfiguration configuration, bool? defaultValue = null) : base(defaultValue ?? default(bool))
        {
            _configuration = configuration;
            _defaultSet = defaultValue.HasValue;
        }

        protected override bool ResolveFeatureValue()
        {
            var stringValue = ConditionalFeatureConfigurationHelper.GetConfigurationValue(_configuration, GetType().Name);
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                if (_defaultSet) return DefaultValue;
                throw new FormatException($"Empty value set for {GetType().Name}");
            }
            if (bool.TryParse(stringValue, out var value))
            {
                return value;
            }
            if (_defaultSet) return DefaultValue;
            throw new FormatException($"Wrong value \"{stringValue}\" set for {GetType().Name}");
        }
    }
}