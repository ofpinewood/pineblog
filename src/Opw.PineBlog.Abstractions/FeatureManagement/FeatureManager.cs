using System.Collections.Generic;
using System.Threading.Tasks;

namespace Opw.PineBlog.FeatureManagement
{
    public class FeatureManager : IFeatureManager
    {
        private readonly IDictionary<FeatureFlag, FeatureState> _features;

        public FeatureManager(IDictionary<FeatureFlag, FeatureState> features)
        {
            _features = features;
        }

        public FeatureState IsEnabled(FeatureFlag featureFlag)
        {
            _features.TryGetValue(featureFlag, out FeatureState state);
            return state;
        }

        public Task<FeatureState> IsEnabledAsync(FeatureFlag featureFlag)
        {
            return Task.FromResult(IsEnabled(featureFlag));
        }
    }
}
