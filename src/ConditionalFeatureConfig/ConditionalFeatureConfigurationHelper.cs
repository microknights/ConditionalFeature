using Microsoft.Extensions.Configuration;

namespace MicroKnights.ConditionalFeature.Configuration
{
    public static class ConditionalFeatureConfigurationHelper
    {
        public const string PrefixName = "ConditionalFeature";
        public const string SectionName = "ConditionalFeatures";

        public static string GetConfigurationValue(IConfiguration configuration, string conditionalFeatureName)
        {
            return configuration?.GetSection(SectionName)[conditionalFeatureName] ?? configuration?[$"{PrefixName}.{conditionalFeatureName}"];
        }
    }
}