using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nexus;
using System.Threading;

namespace TwoLocalGalsWinService
{
    class Recurring
    {
        #region RunGenAppsAndUnavailable
        private static DateTime nextGenAppsAndUnavailable = DateTime.Now.AddHours(1);
        public static void RunGenAppsAndUnavailable()
        {
            try
            {
                if (nextGenAppsAndUnavailable < DateTime.Now)
                {
                    Common.LogThis("Running RunGenAppsAndUnavailable", null);
                    Database.GenerateRecurringApps();
                    Database.GenerateRecurringUnavailable();
                    nextGenAppsAndUnavailable = DateTime.Now.AddHours(6);
                }
                else
                {
                    Common.LogThis("Next RunGenAppsAndUnavailable: " + nextGenAppsAndUnavailable, null);
                }
            }
            catch (Exception ex)
            {
                Common.LogThis("Recurring.RunGenAppsAndUnavailable", ex);
            }
        }
        #endregion

        #region RunDailyTotals
        public static void RunDailyTotals()
        {
            try
            {
                Common.LogThis("Running RunDailyTotals", null);
                foreach (FranchiseStruct franchise in Database.GetFranchiseList())
                    Database.RunDailyTotals(franchise.franchiseMask);
            }
            catch (Exception ex)
            {
                Common.LogThis("Recurring.RunDailyTotals", ex);
            }
        }
        #endregion

        #region RunEmailPayroll
        public static void RunEmailPayroll()
        {
            try
            {
                Common.LogThis("Running RunEmailPayroll", null);
                DateTime mst = DateTime.Now;
                List<ContractorStruct> payrollList = new List<ContractorStruct>();
                foreach (ContractorStruct contractor in Database.GetContractorList(-1, -1, false, false, false, false, "lastName, firstName"))
                {
                    if (contractor.sendPayroll && Globals.ValidEmail(contractor.email))
                    {
                        if (contractor.lastPayroll.DayOfYear != mst.DayOfYear && mst.DayOfWeek.ToString() == contractor.paymentDay)
                        {
                            Common.LogThis("Sending Payroll: " + contractor.title + ", Day: " + contractor.paymentDay + ", Last: " + contractor.lastPayroll.ToString(), null);
                            payrollList.Add(contractor);
                        }
                    }
                }
                DateTime start = Globals.StartOfWeek(mst).AddDays(-14);
                DateTime end = Globals.StartOfWeek(mst).AddDays(-8);
                SendEmail.SendContractorPayroll(payrollList, start, end);
            }
            catch (Exception ex)
            {
                Common.LogThis("Recurring.RunEmailPayroll", ex);
            }
        }
        #endregion

        #region RunEmailSchedules
        public static void RunEmailSchedules()
        {
            try
            {
                Common.LogThis("Running RunEmailSchedules", null);
                DateTime mst = DateTime.Now;
                List<ContractorStruct> scheduleList = new List<ContractorStruct>();
                List<FranchiseStruct> franchiseList = Database.GetFranchiseList();

                foreach (ContractorStruct contractor in Database.GetContractorList(-1, -1, false, true, true, false, "lastName, firstName"))
                {
                    if (contractor.sendSchedules && Globals.ValidEmail(contractor.email) && contractor.lastSchedule.DayOfYear != mst.DayOfYear)
                    {
                        foreach (FranchiseStruct franchise in franchiseList)
                        {
                            if (franchise.sendSchedules.Hour == mst.Hour && (contractor.franchiseMask & franchise.franchiseMask) != 0)
                            {
                                Common.LogThis("Sending Schedule: " + contractor.title + ", Franchise: " + franchise.franchiseName + ", Hour: " + franchise.sendSchedules.Hour, null);
                                scheduleList.Add(contractor);
                            }
                        }
                    }
                }

                if (scheduleList.Count > 0)
                    SendEmail.SendContractorSchedules(scheduleList, mst.AddDays(1), mst.AddDays(1), true);
            }
            catch (Exception ex)
            {
                Common.LogThis("Recurring.RunEmailSchedules", ex);
            }
        }
        #endregion

        #region RunBatches
        private static Dictionary<string, DateTime> closedBatches = new Dictionary<string, DateTime>();
        public static void RunBatches()
        {
            try
            {
                Common.LogThis("Running RunBatches", null);
                DateTime mst = DateTime.Now;
                foreach (FranchiseStruct fran in Database.GetFranchiseList())
                {
                    if (string.IsNullOrEmpty(fran.ePNAccount) || string.IsNullOrEmpty(fran.restrictKey))
                        continue;

                    if (fran.batchTime.Hour != mst.Hour)
                        continue;

                    if (closedBatches.ContainsKey(fran.ePNAccount))
                    {
                        if (closedBatches[fran.ePNAccount].Hour == mst.Hour)
                            continue;
                    }

                    string error = CreditCard.CloseBatch(fran.ePNAccount, fran.restrictKey);
                    if (error == null)
                    {
                        if (closedBatches.ContainsKey(fran.ePNAccount)) closedBatches[fran.ePNAccount] = mst;
                        else closedBatches.Add(fran.ePNAccount, mst);
                    }
                    else Common.LogThis("Batch Error(" + fran.franchiseName + ", " + fran.ePNAccount + "): " + error, null);
                }
            }
            catch (Exception ex)
            {
                Common.LogThis("Recurring.RunBatches", ex);
            }
        }
        #endregion

        #region RunMassEmails
        private static DateTime nextMassEmails = DateTime.Now.AddHours(1);
        public static void RunMassEmails()
        {
            try
            {
                string error = null;
                Common.LogThis("Running RunMassEmails", null);
                if (nextMassEmails < DateTime.Now)
                {
                    if (DateTime.Now.Hour == 09 || DateTime.Now.Hour == 11 || DateTime.Now.Hour == 13 || DateTime.Now.Hour == 15)
                    {
                        List<DBRow> sentList = Database.GetMassEmailsSentToday();
                        List<DBRow> toSendList = Database.GetMassEmailsToSend();

                        for (int i = 0; i < toSendList.Count && i < (250 - sentList.Count); i++)
                        {
                            DBRow massEmail = toSendList[i];
                            DBRow setRow = new DBRow();
                            int massEmailID = massEmail.GetInt("massEmailID");
                            int status = 1;

                            if (massEmail.GetInt("customerID") > 0)
                            {
                                CustomerStruct customer;
                                error = Database.GetCustomerByID(-1, massEmail.GetInt("customerID"), out customer);
                                if (error != null)
                                {
                                    Common.LogThis("Error MassEmail(" + massEmailID + "):  Customer Lookup", null);
                                }
                                else
                                {
                                    if (!customer.sendPromotions)
                                    {
                                        status = 255;
                                        setRow.SetValue("dateSent", DateTime.UtcNow);
                                    }
                                    else
                                    {
                                        error = SendEmail.SendPromotion(massEmail);
                                        if (error != null)
                                        {
                                            Common.LogThis("Error MassEmail(" + massEmailID + "): " + error, null);
                                        }
                                        else
                                        {
                                            Common.LogThis("Mass Email Sent(" + customer.customerID + "): " + customer.email, null);
                                            status = 255;
                                            setRow.SetValue("dateSent", DateTime.UtcNow);
                                        }
                                        Thread.Sleep(5000);
                                    }
                                }
                            }
                            else
                            {
                                error = SendEmail.SendPromotion(massEmail);
                                if (error != null)
                                {
                                    Common.LogThis("Error MassEmail(" + massEmailID + "): " + error, null);
                                }
                                else
                                {
                                    Common.LogThis("Mass Contractor Email Sent(" + massEmail.GetInt("contractorID") + ")", null);
                                    status = 255;
                                    setRow.SetValue("dateSent", DateTime.UtcNow);
                                }
                                Thread.Sleep(5000);
                            }

                            setRow.SetValue("status", status);
                            error = Database.DynamicSetWithKeyInt("MassEmail", "massEmailID", ref massEmailID, setRow);
                            if (error != null)
                            {
                                Common.LogThis("Error MassEmail(" + massEmailID + "):  Setting Status", null);
                            }
                        }
                    }
                    nextMassEmails = DateTime.Now.AddHours(1);
                }

            }
            catch (Exception ex)
            {
                Common.LogThis("Recurring.RunMassEmails", ex);
            }
        }
        #endregion

        #region RunUpdateInactiveCustomers
        public static void RunUpdateInactiveCustomers()
        {
            try
            {
                Common.LogThis("Running RunUpdateInactiveCustomers", null);
                string error = Database.UpdateInactiveCustomers();
                if (error != null)
                {
                    Common.LogThis("Error RunUpdateInactiveCustomers: " + error, null);
                }
            }
            catch (Exception ex)
            {
                Common.LogThis("Recurring.RunUpdateInactiveCustomers", ex);
            }
        }
        #endregion

        #region RunContractorScores
        public static void RunContractorScores()
        {
            try
            {
                Common.LogThis("Running RunContractorScores", null);
                string error = Database.UpdateContractorScores();
                if (error != null)
                {
                    Common.LogThis("Error RunContractorScores: " + error, null);
                }
            }
            catch (Exception ex)
            {
                Common.LogThis("Recurring.RunContractorScores", ex);
            }
        }
        #endregion
    }
}
