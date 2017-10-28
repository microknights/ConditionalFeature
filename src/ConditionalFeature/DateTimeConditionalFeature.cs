using System;

namespace MicroKnights.ConditionalFeature
{
    public class DateTimeConditionalFeature : ConditionalFeature<DateTime>
    {
        public DateTimeConditionalFeature(DateTime defaultValue = default(DateTime)) : base(defaultValue)
        {
        }

        public DateTimeConditionalFeature(Func<DateTime> funcResolveValue, DateTime defaultValue = default(DateTime)) : base(funcResolveValue, defaultValue)
        {
        }

        public DateTimeConditionalFeature(Lazy<DateTime> lazyResolveValue, DateTime defaultValue = default(DateTime)) : base(lazyResolveValue, defaultValue)
        {
        }

        public bool IsBefore(DateTime datetime) => FeatureValue < datetime;
        public bool IsAfter(DateTime datetime) => FeatureValue > datetime;
        public bool IsBetween(DateTime start, DateTime end) => FeatureValue >= start && FeatureValue <= end;
        public bool IsOutside(DateTime start, DateTime end) => FeatureValue < start || FeatureValue > end;
    }
}