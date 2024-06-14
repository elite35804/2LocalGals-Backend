using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TwoLocalGalsService
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Common.LogThis("--- TwoLocalGals Service Starting v" + Application.ProductVersion + " ---", null);
                while (true)
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

                    TimeSpan waitTime = TimeSpan.FromMinutes(10) - (DateTime.UtcNow - startTime);
                    Common.LogThis("Sleeping For: " + waitTime.TotalMinutes.ToString("N2") + " minutes", null);
                    if (waitTime > TimeSpan.Zero) Thread.Sleep(waitTime);
                }
            }
            catch (Exception ex)
            {
                Common.LogThis("Main", ex);
            }
        }
    }
}
