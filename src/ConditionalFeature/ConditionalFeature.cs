using System;

namespace MicroKnights.ConditionalFeature
{
    public abstract class ConditionalFeature<TValue>
    {
        private readonly Lazy<TValue> _lazyValueResolver;
        private readonly Func<TValue> _funcValueResolver;

        protected TValue DefaultValue { get; set; }

        public virtual string FeatureName => GetType().Name;

        protected ConditionalFeature(TValue defaultValue)
        {
            DefaultValue = defaultValue;
            _lazyValueResolver = null;
            _funcValueResolver = null;
        }

        /// <summary>
        /// Using func to get value, called every time the conditionalfeature is called
        /// </summary>
        /// <param name="funcResolveValue">Func to call to get value</param>
        /// <param name="defaultValue"></param>
        protected ConditionalFeature(Func<TValue> funcResolveValue, TValue defaultValue = default(TValue))
            : this(defaultValue)
        {
            _funcValueResolver = funcResolveValue;
        }

        protected ConditionalFeature(Lazy<TValue> lazyResolveValue, TValue defaultValue = default(TValue))
            : this(defaultValue)
        {
            _lazyValueResolver = lazyResolveValue;
        }

        public virtual TValue FeatureValue
        {
            get
            {
                if (_lazyValueResolver != null)
                    return _lazyValueResolver.Value;
                if (_funcValueResolver != null)
                    return _funcValueResolver.Invoke();
                return ResolveFeatureValue();
            }
        }

        protected virtual TValue ResolveFeatureValue()
        {
            return DefaultValue;
        }
    }
}