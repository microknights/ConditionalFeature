using System;
using System.Globalization;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace MicroKnights.ConditionalFeature.Configuration
{
    public abstract class DateTimeConditionalConfigurationFeature : DateTimeConditionalFeature
    {
        private readonly IConfiguration _configuration;
        private readonly bool _defaultSet;

        protected IFormatProvider FormatProvider;
        protected DateTimeStyles DateTimeStyles;

        protected DateTimeConditionalConfigurationFeature(IConfiguration configuration, DateTime? defaultValue = null) : base(defaultValue ?? default(DateTime))
        {
            _configuration = configuration;
            _defaultSet = defaultValue.HasValue;

            FormatProvider = CultureInfo.CurrentCulture;
            DateTimeStyles = DateTimeStyles.None;
        }

        protected override DateTime ResolveFeatureValue()
        {
            var stringValue = ConditionalFeatureConfigurationHelper.GetConfigurationValue(_configuration, GetType().Name);
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                if (_defaultSet) return DefaultValue;
                throw new FormatException($"Empty value set for {GetType().Name}");
            }
            if (DateTime.TryParse(stringValue, FormatProvider, DateTimeStyles, out var value))
            {
                return value;
            }
            if (_defaultSet) return DefaultValue;
            throw new FormatException($"Wrong value \"{stringValue}\" set for {GetType().Name}");
        }
    }
}