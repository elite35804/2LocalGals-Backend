using System;
using System.ServiceProcess;
using Nexus;

namespace TwoLocalGalsWinService
{
    static class Program
    {
        static void Main()
        {
            try
            {
#if DEBUG
                Service.ServiceThread();
#else
                ServiceBase[] ServicesToRun;
                ServiceBase.Run(ServicesToRun = new ServiceBase[] { new Service() });
#endif
            }
            catch (Exception ex)
            {
                Common.LogThis("Main", ex);
            }
        }
    }
}
