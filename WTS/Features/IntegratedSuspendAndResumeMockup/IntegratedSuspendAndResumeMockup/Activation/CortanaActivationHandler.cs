using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegratedSuspendAndResumeMockup.Services;

namespace IntegratedSuspendAndResumeMockup.Activation
{
    public class CortanaActivationHandler : ActivationHandler
    {
        public override bool CanHandle(object args)
        {
            throw new NotImplementedException();
        }

        public override Task HandleAsync(object args, SuspendAndResumeArgs suspendAndResumeArgs = null)
        {
            throw new NotImplementedException();
        }
    }
}
