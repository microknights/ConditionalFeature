using System;
using Microsoft.Extensions.Configuration;

namespace MicroKnights.ConditionalFeature.Configuation
{
    public abstract class EnumConditionalConfigurationFeature<TEnum> : EnumConditionalFeature<TEnum> where TEnum : struct
    {
        private readonly IConfiguration _configuration;
        private readonly bool _defaultSet;

        protected EnumConditionalConfigurationFeature(IConfiguration configuration, TEnum? defaultValue = null) 
            : base(defaultValue ?? default(TEnum))
        {
            _configuration = configuration;
            _defaultSet = defaultValue.HasValue;
        }

        protected override TEnum ResolveFeatureValue()
        {
            var stringValue = ConditionalFeatureConfigurationHelper.GetConfigurationValue(_configuration, GetType().Name);
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                if (_defaultSet) return DefaultValue;
                throw new FormatException($"Empty value set for {GetType().Name}");
            }
            if (Enum.TryParse<TEnum>(stringValue, true, out var value))
            {
                return value;
            }
            if (_defaultSet) return DefaultValue;
            throw new FormatException($"Wrong value \"{stringValue}\" set for {GetType().Name}");
        }

    }
}