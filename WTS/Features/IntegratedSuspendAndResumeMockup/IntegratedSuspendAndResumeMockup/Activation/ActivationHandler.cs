using System;
using System.Threading.Tasks;
using IntegratedSuspendAndResumeMockup.Services;

namespace IntegratedSuspendAndResumeMockup.Activation
{
    // For more information on application activation see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/activation.md
    internal abstract class ActivationHandler
    {
        public abstract bool CanHandle(object args);

        public abstract Task HandleAsync(object args, SuspendAndResumeArgs suspendAndResumeArgs = null);
    }

    internal abstract class ActivationHandler<T> : ActivationHandler
        where T : class
    {
        protected abstract Task HandleInternalAsync(T args, SuspendAndResumeArgs suspendAndResumeArgs = null);

        public override async Task HandleAsync(object args, SuspendAndResumeArgs suspendAndResumeArgs = null)
        {
            await HandleInternalAsync(args as T, suspendAndResumeArgs);
        }

        public override bool CanHandle(object args)
        {
            return args is T && CanHandleInternal(args as T);
        }

        protected virtual bool CanHandleInternal(T args)
        {
            return true;
        }
    }
}
