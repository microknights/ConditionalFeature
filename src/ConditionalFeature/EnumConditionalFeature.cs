using System;
using System.Linq;

namespace MicroKnights.ConditionalFeature
{
    public abstract class EnumConditionalFeature<TEnum> : ConditionalFeature<TEnum> where TEnum : struct
    {
        protected EnumConditionalFeature(TEnum defaultValue = default(TEnum)) : base(defaultValue)
        {
        }

        public EnumConditionalFeature(Func<TEnum> funcResolveValue, TEnum defaultValue = default(TEnum)) : base(funcResolveValue, defaultValue)
        {
            if (Enum.IsDefined(typeof(TEnum), defaultValue) == false) throw new ArgumentOutOfRangeException(nameof(defaultValue), defaultValue, $"Not defined in {typeof(TEnum).FullName}");
        }

        public EnumConditionalFeature(Lazy<TEnum> lazyResolveValue, TEnum defaultValue = default(TEnum)) : base(lazyResolveValue, defaultValue)
        {
            if (Enum.IsDefined(typeof(TEnum), defaultValue) == false) throw new ArgumentOutOfRangeException(nameof(defaultValue), defaultValue, $"Not defined in {typeof(TEnum).FullName}");
        }

        public virtual bool Is(TEnum anum) => FeatureValue.Equals(anum);
        public virtual bool Not(TEnum anum) => FeatureValue.Equals(anum) == false;
        public virtual bool Any(params TEnum[] anum) => anum.Any( e => FeatureValue.Equals(e) );
        public virtual bool NotAny(params TEnum[] anum) => anum.All( e => FeatureValue.Equals(e) == false );
    }
}