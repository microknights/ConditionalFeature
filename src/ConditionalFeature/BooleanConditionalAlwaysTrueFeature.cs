namespace MicroKnights.ConditionalFeature
{
    public abstract class BooleanConditionalAlwaysTrueFeature : BooleanConditionalFeature
    {
        protected override bool ResolveFeatureValue()
        {
            return true;
        }
    }
}