using System.Threading.Tasks;

namespace Opw.PineBlog.FeatureManagement
{
    /// <summary>
    /// Used to evaluate whether a feature is enabled or disabled.
    /// </summary>
    public interface IFeatureManager
    {
        FeatureState IsEnabled(FeatureFlag featureFlag);

        Task<FeatureState> IsEnabledAsync(FeatureFlag featureFlag);
    }
}
