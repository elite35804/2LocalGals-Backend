using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TwoLocalGalsWinService
{
    public partial class Service : ServiceBase
    {
        private static Thread serviceThread = null;
        private static ManualResetEvent exitServiceThread = new ManualResetEvent(false);
        private static ManualResetEvent exitedServiceThread = new ManualResetEvent(false);

        public Service()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Common.LogThis("Service", ex);
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                exitServiceThread.Reset();
                exitedServiceThread.Reset();
                serviceThread = new Thread(new ThreadStart(ServiceThread));
                serviceThread.Start();
            }
            catch (Exception ex)
            {
                Common.LogThis("Service.OnStart", ex);
            }
        }

        protected override void OnStop()
        {
            try
            {
                exitServiceThread.Set();
                if (!exitedServiceThread.WaitOne(5000, false))
                    serviceThread.Abort();
            }
            catch (Exception ex)
            {
                Common.LogThis("Service.OnStop", ex);
            }
        }

        public static void ServiceThread()
        {
            try
            {
                TimeSpan waitTime = TimeSpan.FromMinutes(0);
                Common.LogThis("--- TwoLocalGals Service Starting v" + Application.ProductVersion + " ---", null);
                while (!exitServiceThread.WaitOne(waitTime, false))
                {
                    Common.LogThis("Done Sleeping", null);
                    DateTime startTime = DateTime.UtcNow;
                    Recurring.RunGenAppsAndUnavailable();
                    Recurring.RunDailyTotals();
                    Recurring.RunEmailPayroll();
                    Recurring.RunEmailSchedules();
                    Recurring.RunBatches();
                    Recurring.RunMassEmails();
                    Recurring.RunUpdateInactiveCustomers();
                    Recurring.RunContractorScores();

                    waitTime = TimeSpan.FromMinutes(10) - (DateTime.UtcNow - startTime);
                    if (waitTime <= TimeSpan.Zero) waitTime = TimeSpan.FromMinutes(1);
                    Common.LogThis("Sleeping For: " + waitTime.TotalMinutes.ToString("N2") + " minutes", null);
                }
            }
            catch (Exception ex)
            {
                Common.LogThis("Service.ServiceThread", ex);
            }
            finally
            {
                exitedServiceThread.Set();
            }
        }
    }
}
