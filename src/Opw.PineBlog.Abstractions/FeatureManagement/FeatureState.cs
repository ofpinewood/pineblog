namespace Opw.PineBlog.FeatureManagement
{
    public struct FeatureState
    {
        public bool IsEnabled { get; }

        public string Message { get; }

        private FeatureState(bool isEnabled, string message)
        {
            IsEnabled = isEnabled;
            Message = message;
        }

        /// <summary>
        /// Creates an disabled feature state.
        /// </summary>
        /// <param name="message">A help message for the users.</param>
        public static FeatureState Disabled(string message)
        {
            return new FeatureState(false, message);
        }

        /// <summary>
        /// Creates an enabled feature state.
        /// </summary>
        public static FeatureState Enabled()
        {
            return new FeatureState(true, null);
        }

        /// <summary>
        /// Implicitly operator to check if the feature is enabled without having to access the 'IsEnabled' property
        /// </summary>
        public static implicit operator bool(FeatureState state)
        {
            return state.IsEnabled;
        }
    }
}
