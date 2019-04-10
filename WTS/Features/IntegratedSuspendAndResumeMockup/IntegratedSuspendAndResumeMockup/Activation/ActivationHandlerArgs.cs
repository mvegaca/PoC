using IntegratedSuspendAndResumeMockup.Services;

namespace IntegratedSuspendAndResumeMockup.Activation
{
    public class ActivationHandlerArgs
    {
        public object NavigationParameter { get; set; }

        public SuspensionState SuspensionState { get; set; }

        public ActivationHandlerArgs()
        {
        }
    }
}
