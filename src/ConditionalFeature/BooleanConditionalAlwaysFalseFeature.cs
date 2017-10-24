namespace MicroKnights.ConditionalFeature
{
    public abstract class BooleanConditionalAlwaysFalseFeature : BooleanConditionalFeature
    {
        protected override bool ResolveFeatureValue()
        {
            return false;
        }
    }
}