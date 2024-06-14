using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.Threading;
using System.IO;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace Nexus
{
    public class SendEmail
    {
        #region SendContractorSchedules
        public static string SendContractorSchedules(List<ContractorStruct> contractors, DateTime startDate, DateTime endDate, bool updateDB)
        {
            string ret = "";
            try
            {
                if (contractors.Count == 0) return null;
                Globals.FormatDateRange(ref startDate, ref endDate);

                Dictionary<int, ContractorStruct> conDict = new Dictionary<int, ContractorStruct>();
                foreach (ContractorStruct contractor in contractors)
                {
                    if (!conDict.ContainsKey(contractor.contractorID))
                        conDict.Add(contractor.contractorID, contractor);
                }

                SortedList<int, SortedList<DateTime, List<ConAppStruct>>> list = new SortedList<int, SortedList<DateTime, List<ConAppStruct>>>();
                Dictionary<DateTime, Dictionary<int, List<ConAppStruct>>> partnerDict = new Dictionary<DateTime, Dictionary<int, List<ConAppStruct>>>();
                foreach (ConAppStruct conApp in Database.GetCleaningProjects(-1, startDate, endDate))
                {
                    if (conDict.ContainsKey(conApp.contractorID))
                    {
                        if (!list.ContainsKey(conApp.contractorID)) list.Add(conApp.contractorID, new SortedList<DateTime, List<ConAppStruct>>());
                        if (!list[conApp.contractorID].ContainsKey(conApp.appointmentDate)) list[conApp.contractorID].Add(conApp.appointmentDate, new List<ConAppStruct>());
                        list[conApp.contractorID][conApp.appointmentDate].Add(conApp);
                    }

                    if (!partnerDict.ContainsKey(conApp.appointmentDate)) partnerDict.Add(conApp.appointmentDate, new Dictionary<int, List<ConAppStruct>>());
                    if (!partnerDict[conApp.appointmentDate].ContainsKey(conApp.customerID)) partnerDict[conApp.appointmentDate].Add(conApp.customerID, new List<ConAppStruct>());
                    partnerDict[conApp.appointmentDate][conApp.customerID].Add(conApp);
                }

                List<FranchiseStruct> franchiseList = Database.GetFranchiseList();

                foreach (int contractorID in list.Keys)
                {
                    try
                    {
                        decimal totalHours = 0;
                        ContractorStruct contractor = conDict[contractorID];
                        if (string.IsNullOrEmpty(contractor.email)) continue;

                        SmtpEmailStruct smtp = new SmtpEmailStruct();
                        smtp.to = contractor.email;

                        foreach (FranchiseStruct franchise in franchiseList)
                        {
                            if ((contractor.franchiseMask & franchise.franchiseMask) != 0)
                            {
                                if (!string.IsNullOrEmpty(franchise.email) && !string.IsNullOrEmpty(franchise.emailSmtp))
                                {
                                    smtp.from = franchise.email;
                                    smtp.smtp = franchise.emailSmtp;
                                    smtp.password = franchise.emailPassword;
                                    smtp.port = franchise.emailPort;
                                    smtp.secure = franchise.emailSecure;
                                    break;
                                }
                            }
                        }

                        string dateRangeString = startDate.Date == endDate.Date ? startDate.ToString("d") : startDate.ToString("d") + " to " + endDate.ToString("d");
                        smtp.subject = "2 Local Gals Schedule For: " + contractor.title + " (" + dateRangeString + ")";

                        smtp.body = @"
                        <head>
                            <style type=""text/css"">

                            table
                            {
                                margin-bottom: 10px;
                                padding: 2px 2px 2px 2px;
                                border-collapse: collapse;
                                width: 100%;
                            }

                            table th
                            {
                                font-weight: bold;
                                border: 1px solid #2B2B2B;
                            }

                            </style>
                        </head>";

                        smtp.body = @"<h1>" + smtp.subject + @"</h1>";

                        foreach (DateTime date in list[contractorID].Keys)
                        {
                            string lastAddr = null;
                            smtp.body += @"<h2>Scheduled Projects: " + date.ToString("dddd") + " " + date.ToString("d") + @"</h2>";
                            foreach (ConAppStruct conApp in list[contractorID][date])
                            {
                                if (lastAddr == null) lastAddr = Globals.CleanAddr(conApp.contractorAddress) + "," + Globals.CleanAddr(conApp.contractorCity) + "," + Globals.CleanAddr(conApp.contractorState) + "," + Globals.CleanAddr(conApp.contractorZip);
                                string conAppBody = @"
                                <table style=""margin-bottom: 10px; padding: 2px 2px 2px 2px; border-collapse: collapse; width: 100%;"">
                                    <tr>
                                        <th style=""font-weight: bold; border: 1px solid #2B2B2B;"" width=""120"">Time</th>
                                        <th style=""font-weight: bold; border: 1px solid #2B2B2B;"" width=""50"">Hours</th>
                                        <th style=""font-weight: bold; border: 1px solid #2B2B2B;"" width=""150"">Customer</th>
                                        <th style=""font-weight: bold; border: 1px solid #2B2B2B;"">Address</th>
                                        <th style=""font-weight: bold; border: 1px solid #2B2B2B;"" width=""100"">City</th>
                                    </tr>
                                    <tr>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                    </tr>
                                    <tr>
                                        <td colspan=""5""><b>Appointment Type: </b>{17}</td>
                                    </tr>
                                    <tr>
                                        <td colspan=""5""><b>Partners: </b>{5}</td>
                                    </tr>
                                    <tr>
                                        <td colspan=""5""><b>Building Info: </b>{6}</td>
                                    </tr>
                                    <tr>
                                        <td colspan=""5""><b>Instructions: </b>{7}</td>
                                    </tr>
                                    <tr>
                                        <td colspan=""5""><b>Details: </b>{8}</td>
                                    </tr>
                                    <tr>
                                        <td colspan=""5"">
                                            <b>Account Status: </b>{9}
                                            {10}
                                            <b style=""font-size: 1.3em; margin-left: 20px;"">{12}</b>
                                            {13}
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan=""5"">
                                            <b>Phone: </b>{14}
                                            <b style=""margin-left: 20px;"">Alternate: </b>{15}
                                            <b style=""margin-left: 20px;"">Alternate: </b>{16}
                                        </td>
                                    </tr>
                                </table>";

                                TimeSpan span = conApp.endTime - conApp.startTime;
                                decimal hours = (decimal)span.TotalMinutes / 60;
                                totalHours += hours;

                                object[] values = new object[18];
                                values[0] = conApp.startTime.ToString("t") + " to " + conApp.endTime.ToString("t");
                                values[1] = hours.ToString("N2");
                                values[2] = conApp.customerTitle;
                                string newAddr = Globals.CleanAddr(conApp.address) + "," + Globals.CleanAddr(conApp.city) + "," + Globals.CleanAddr(conApp.state) + "," + Globals.CleanAddr(conApp.zip);
                                values[3] = @"<a href=""" + GoogleMaps.GetDrivingLink(new string[] { "Current Location", newAddr }) + @""">" + conApp.address + @"</a>";
                                lastAddr = newAddr;

                                values[4] = conApp.city;

                                List<string> partners = new List<string>();
                                foreach (var partner in partnerDict[conApp.appointmentDate][conApp.customerID])
                                {
                                    if (partner.contractorID != conApp.contractorID)
                                    {
                                        partners.Add(partner.contractorTitle + (string.IsNullOrWhiteSpace(partner.contractorPhone) ? "" : " " + partner.contractorPhone));
                                    }
                                }

                                if (partners.Count == 0) partners.Add("None");
                                values[5] = string.Join(", ", partners.ToArray());

                                values[6] = conApp.general;
                                values[7] = conApp.instructions;
                                values[8] = conApp.details;
                                values[9] = conApp.customerAccountStatus;
                                values[10] = conApp.appType == 1 ? @"<b style=""margin-left: 20px;"">Service Fee: </b>" + Globals.FormatMoney(conApp.customerServiceFee) : "";
                                values[11] = Globals.FormatMoney(conApp.customerRate);

                                if (conApp.appType != 4)
                                {
                                    if ((conApp.paymentType.ToLower().Contains("ch") || conApp.paymentType.ToLower().Contains("cash")) && !conApp.paymentType.ToLower().Contains("mail"))
                                        values[12] = @"Payment Type: " + conApp.paymentType + @" (COLLECT " + Globals.FormatMoney(conApp.customerCollect) + @")";
                                    else
                                        values[12] = @"Payment Type: " + conApp.paymentType;
                                }

                                values[13] = (conApp.keys && conApp.appType == 1) ? @"<b style=""font-size: 1.3em; border: 2px solid black; margin-left: 20px;"">TAKE KEYS</b>" : "";
                                values[14] = Globals.FormatPhone(conApp.bestPhone);
                                values[15] = Globals.FormatPhone(conApp.alternatePhoneOne);
                                values[16] = Globals.FormatPhone(conApp.alternatePhoneTwo);
                                values[17] = Globals.FormatPhone(Globals.IndexToServiceType(conApp.appType));

                                smtp.body += string.Format(conAppBody, values);
                            }

                            smtp.body += @"<h3>" + totalHours.ToString("N2") + @" Total Hours</h3><br/><br/><br/>";
                        }


                        string error = SendSmtpEmail(smtp);
                        if (error == null)
                        {
                            if (updateDB)
                                Database.SetContractorScheduleSent(contractorID);
                        }
                        else
                        {
                            ret += "Send Failure (ContractorID=" + contractor.contractorID + ") EX: " + error + "<br/>";
                            Database.LogThis("SendContractorSchedules.Send(ContractorID=" + contractor.contractorID + ") EX: " + error, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        ret += "Format Failure (ContractorID=" + contractorID + ") EX: " + ex.Message + "<br/>";
                        Database.LogThis("SendContractorSchedules.FormatFailure(ContractorID=" + contractorID + ")", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                ret += "SendEmail.SendContractorSchedules EX: " + ex.Message + "<br/>";
                Database.LogThis("SendContractorSchedules", ex);
            }
            return ret == "" ? null : ret;
        }
        #endregion

        #region SendContractorPayroll
        public static string SendContractorPayroll(List<ContractorStruct> contractors, DateTime startDate, DateTime endDate)
        {
            string ret = null;

            try
            {
                List<PayrollDoc> payrollList = new List<PayrollDoc>();
                ret = PayrollDoc.GetPayroll(2, -1, -1, contractors, startDate, endDate, true, out payrollList);

                if (ret == null)
                {
                    foreach (PayrollDoc payrollDoc in payrollList)
                    {
                        string error = SendSmtpEmail(payrollDoc.smtp);
                        if (error == null)
                        {
                            Database.SetContractorPayrollSent(payrollDoc.contractor.contractorID);
                        }
                        else
                        {
                            ret += "Send Failure (ContractorID=" + payrollDoc.contractor.contractorID + ") EX: " + error + "<br/>";
                            Database.LogThis("SendContractorPayroll.Send(ContractorID=" + payrollDoc.contractor.contractorID + ") EX: " + error, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ret += "SendEmail.SendContractorPayroll EX: " + ex.Message + "<br/>";
                Database.LogThis("SendContractorPayroll", ex);
            }

            return ret == "" ? null : ret;
        }
        #endregion

        #region SendWelcomeLetter
        public static string SendWelcomeLetter(string customerEmail, string customerName, string repName, FranchiseStruct franchise)
        {
            try
            {
                if (string.IsNullOrEmpty(customerEmail))
                    return "Customer Email is Blank";
                if (string.IsNullOrEmpty(customerName))
                    return "Customer Name is Blank";
                if (string.IsNullOrEmpty(repName))
                    return "Customer Representative is Blank";

                SmtpEmailStruct smtp = new SmtpEmailStruct();
                smtp.to = customerEmail;
                smtp.from = franchise.email;
                smtp.smtp = franchise.emailSmtp;
                smtp.password = franchise.emailPassword;
                smtp.port = franchise.emailPort;
                smtp.secure = franchise.emailSecure;

                smtp.subject = "Welcome to 2 Local Gals Housekeeping";

                smtp.body = @"
                <head>
                    <style type=""text/css"">
                    p
                    {
                        font-size: 1.3em;
                    }
                    </style>
                </head>";

                string htmlBody = @"
                
                <img src=""" + Globals.baseUrl + @"2LG_Letterhead.png""></img>
                <br/>
                <br/>
                <p>
                Hello {0},
                </p>

                <h1>Welcome to 2 Local Gals</h1>

                <p>
                    We love new customers and we are glad you picked us.  Our business started in May of 1999 with 2 single moms that 
                    worked hard because they had a dream of being more. As our business grew we hired more gals with the same dream. Today 
                    we provide work for many women, cleaning homes and offices across the state of Utah and Texas. Our gals take pride in their work 
                    and always do the best job that they can.
                    <br/><br/>
                    We have a reputation of being reliable, honest, and professional. We take pride in the fact that people can come home 
                    to a clean home and spend their time doing what is important to them.
                </p>

                <h2>24 Hour Guarantee</h2>
                
                <p>
                    Our goal is for you to be happy with the quality of our service.  If for any reason you feel that we did not meet your 
                    expectations of quality, please call us and let us know immediately.  We will make every effort to ensure you are happy 
                    with us.
                </p>
  
                <h2>Liability</h2>
                
                <p>
                    Although we are licensed, bonded and insured it is suggested that you look to your homeowner's policy to protect items in your 
                    home against damage or loss.  It is recommended that you remove delicate fixtures or expensive items. We do not clean fragile
                    or expensive items.
                </p>

                <h2>Non-Solicitation</h2>

                <p>
                    As a service provider, we are entering into a verbal agreement to provide service to you.  As a term of that verbal agreement, 
                    we ask that you do not solicit our personnel for employment outside of 2 Local Gals.  The personnel provide services to you under 
                    a non-compete agreement, which prohibits them from accepting any offers to service you outside of the company.
                </p>

                <h2>Referrals</h2>

                <p>
                    We have joined forces with a number of different outstanding companies from movers to carpet and window cleaners. We have found that 
                    people that need house or office cleaning often times are in need of additional help as well. Check out our referrals for businesses 
                    you can trust before looking elsewhere.
                    <br/><br/><a href=""" + Globals.baseUrlSecure + @"Partners.aspx?franMask={1}"">Referral List</a>
                </p>

                <h2>We appreciate you as a customer and for the opportunity to serve you.</h2>

                <p>
                    If you have any questions, please don't hesitate to call.<br/>
                    <br/>
                    Thank you,<br/>
                    <br/>
                    2 Local Gals<br/>
                    <br/>
                    {2}<br/>
                    {3}<br/>
                    <a href=""{4}"">{4}</a><br/>
                </p>";

                smtp.body += string.Format(htmlBody, customerName, franchise.franchiseMask, franchise.phone, franchise.email, franchise.webLink);

                string error = SendSmtpEmail(smtp);
                if (error == null) return null;
                else return "SendEmail.SendWelcomeLetter(SEND) EX: " + error;
            }
            catch (Exception ex)
            {
                return "SendEmail.SendWelcomeLetter EX: " + ex.Message;
            }
        }
        #endregion

        #region SendConfimation
        public static string SendConfimation(int appID, bool oneWeek)
        {
            try
            {
                AppStruct app;
                string error = Database.GetApointmentpByID(Globals.GetFranchiseMask(), appID, out app);
                if (error != null)
                {
                    return "Error GetApointmentpByID: " + error;
                }
                else
                {
                    CustomerStruct cust;
                    Database.GetCustomerByID(Globals.GetFranchiseMask(), app.customerID, out cust);
                    if (error != null)
                    {
                        return "Error GetCustomerByID: " + error;
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(cust.email))
                            return null;

                        FranchiseStruct fran = Globals.GetFranchiseByMask(cust.franchiseMask);

                        SmtpEmailStruct smtp = new SmtpEmailStruct();
                        smtp.to = cust.email;
                        smtp.from = fran.email;
                        smtp.smtp = fran.emailSmtp;
                        smtp.password = fran.emailPassword;
                        smtp.port = fran.emailPort;
                        smtp.secure = fran.emailSecure;

                        smtp.subject = "2 Local Gals Appointment Reminder";

                        smtp.body = @"
                        <head>
                            <style type=""text/css"">
                            p
                            {
                                font-size: 1.3em;
                            }
                            </style>
                        </head>";

                        
                        string htmlBody = @"
                        <img src=""" + Globals.baseUrl + @"2LG_Letterhead.png""></img>
                        <br/>
                        <br/>
                        <h1>Appointment Reminder</h1>
                        <p>
                            Hi {0},
                        </p>
                        <p>
                            This is a friendly reminder of your appointment around <b>{1}</b> on <b>{2}</b>. 
                        </p>
                        <p>
                            at <b>{3}</b>.
                        </p>

                        <p>
                            If you have any questions or concerns, please call us immediately. Otherwise, we will see you then!<br/>
                            <br/>
                            Thank you,<br/>
                            <br/>
                            2 Local Gals<br/>
                            <br/>
                            {4}<br/>
                            {5}<br/>
                            <a href=""{6}"">{6}</a><br/>
                        </p>";


                        if (oneWeek)
                        {
                            htmlBody = @"
                            <img src=""" + Globals.baseUrl + @"2LG_Letterhead.png""></img>
                            <br/>
                            <br/>
                            <h1>Appointment Reminder</h1>
                            <p>
                                Hi {0},
                            </p>
                            <p>
                                This is a friendly reminder of your appointment on <b>{2}</b>. 
                            </p>
                            <p>
                                at <b>{3}</b>. We will contact you the day before with an exact time.
                            </p>

                            <p>
                                If you have any questions or concerns, please call us immediately. Otherwise, we will see you then!<br/>
                                <br/>
                                Thank you,<br/>
                                <br/>
                                2 Local Gals<br/>
                                <br/>
                                {4}<br/>
                                {5}<br/>
                                <a href=""{6}"">{6}</a><br/>
                            </p>";
                        }

                        smtp.body += string.Format(htmlBody, cust.firstName, app.startTime.ToString("t"), app.appointmentDate.ToString("dddd, MMMM d"), cust.locationAddress + ", " + cust.locationCity + ", " + cust.locationState + " " + cust.locationZip, fran.phone, fran.email, fran.webLink);

                        error = SendSmtpEmail(smtp);
                        if (error == null) return null;
                        else return "SendEmail.SendConfimation(SEND) EX: " + error;
                    }
                }
            }
            catch (Exception ex)
            {
                return "SendEmail.SendConfimation EX: " + ex.Message;
            }
        }
        #endregion

        #region SendHomeGuardActivation
        public static string SendHomeGuardActivation(CustomerStruct customer)
        {
            try
            {
                if (!Globals.ValidEmail(customer.email))
                    return "Customer Email is Invalid";

                FranchiseStruct fran = Globals.GetFranchiseByMask(customer.franchiseMask);

                SmtpEmailStruct smtp = new SmtpEmailStruct();
                smtp.to = Globals.SplitEmail(customer.email)[0];
                smtp.from = fran.email;
                smtp.smtp = fran.emailSmtp;
                smtp.password = fran.emailPassword;
                smtp.port = fran.emailPort;
                smtp.secure = fran.emailSecure;


                string oneClickActivation = Globals.BuildQueryString(Globals.baseUrlSecure + @"HomeGuardContract.aspx", "Sign", Globals.Encrypt(customer.customerID.ToString()));

                smtp.subject = "2 Local Gals Home Guard Activation";
                string htmlBody = @"
                <img src=""" + Globals.baseUrl + @"2LG_Letterhead.png""></img>
                <br/>
                <br/>
                <p>
                Hi {0},
                </p>
                <p>
                     Welcome to 2 Local Gals Home Guard. This email is to confirm that we have all the correct information and that you agree to the terms of the Home Guard Agreement.
                </p>
                <br/>
                <p>
                    <a href=""" + oneClickActivation + @"""><img src=""" + Globals.baseUrl + @"2LG_ClickContinue.png"" /></a>
                </p>
                <br/>
                <p>We appreciate you as a customer and for the opportunity to serve you.</p>
                <p>
                    If you have any questions, please don't hesitate to call.<br/>
                    <br/>
                    Thank you,<br/>
                    <br/>
                    2 Local Gals<br/>
                    <br/>
                    {3}<br/>
                    {4}<br/>
                    <a href=""{5}"">{5}</a><br/>
                </p>";

                smtp.body = string.Format(htmlBody, customer.firstName, smtp.to, Globals.CustomerIDToPassphrase(customer.customerID), fran.phone, fran.email, fran.webLink);

                string error = SendSmtpEmail(smtp);
                if (error == null) return null;
                else return "SendEmail.SendCustomerLoginInfo(SEND) EX: " + error;
            }
            catch (Exception ex)
            {
                return "SendEmail.SendCustomerLoginInfo EX: " + ex.Message;
            }
        }
        #endregion

        #region SendCustomerLoginInfo
        public static string SendCustomerLoginInfo(CustomerStruct customer)
        {
            try
            {
                if (!Globals.ValidEmail(customer.email))
                    return "Customer Email is Invalid";

                FranchiseStruct fran = Globals.GetFranchiseByMask(customer.franchiseMask);

                SmtpEmailStruct smtp = new SmtpEmailStruct();
                smtp.to = Globals.SplitEmail(customer.email)[0];
                smtp.from = fran.email;
                smtp.smtp = fran.emailSmtp;
                smtp.password = fran.emailPassword;
                smtp.port = fran.emailPort;
                smtp.secure = fran.emailSecure;

                string oneClickLogin = Globals.BuildQueryString(Globals.BuildQueryString(Globals.baseUrlSecure + @"Login.aspx", "A", Globals.Encrypt(smtp.to)), "B", Globals.Encrypt(Globals.CustomerIDToPassphrase(customer.customerID)));

                smtp.subject = "2 Local Gals Login Information";
                string htmlBody = @"
                <img src=""" + Globals.baseUrl + @"2LG_Letterhead.png""></img>
                <br/>
                <br/>
                <p>
                Hi {0},
                </p>
                <p>
                    2 Local Gals has created a dedicated place just for you! Go check it out and see all the useful things you can do:
                </p>
                <br />
                    <ul>
                        <li>View appointments</li>
                        <li>Request appointments</li>
                        <li>View your Customer Rewards Points</li>
                        <li>Update your account information</li>
                        <li>Buy virtual gift cards and send them to friends and family</li>
                        <li>Redeem virtual gift cards</li>
                        <li>Leave feedback reviews on your services</li>
                        <li>Leave tips for your Gals that cleaned for you</li>
                        <li>Ability to refer family and friends and save up to 30% off each cleaning</li>
                        <li>And much more!</li>
                    </ul>
                <br/>
                <p>
                    Customer Portal Login: <a href=""" + oneClickLogin + @""">Click Here</a><br/>
                    Username: <b>{1}</b><br/>
                    Password: <b>{2}</b>
                </p>
                <br/>
                <p>We appreciate you as a customer and for the opportunity to serve you.</p>
                <p>
                    If you have any questions, please don't hesitate to call.<br/>
                    <br/>
                    Thank you,<br/>
                    <br/>
                    2 Local Gals<br/>
                    <br/>
                    {3}<br/>
                    {4}<br/>
                    <a href=""{5}"">{5}</a><br/>
                </p>";

                smtp.body = string.Format(htmlBody, customer.firstName, smtp.to, Globals.CustomerIDToPassphrase(customer.customerID), fran.phone, fran.email, fran.webLink);

                string error = SendSmtpEmail(smtp);
                if (error == null) return null;
                else return "SendEmail.SendCustomerLoginInfo(SEND) EX: " + error;
            }
            catch (Exception ex)
            {
                return "SendEmail.SendCustomerLoginInfo EX: " + ex.Message;
            }
        }
        #endregion

        #region SendCustomerRewardsEnabled
        public static string SendCustomerRewardsEnabled(CustomerStruct customer)
        {
            try
            {
                if (!Globals.ValidEmail(customer.email))
                    return "Customer Email is Invalid";

                FranchiseStruct fran = Globals.GetFranchiseByMask(customer.franchiseMask);

                SmtpEmailStruct smtp = new SmtpEmailStruct();
                smtp.to = customer.email;
                smtp.from = fran.email;
                smtp.smtp = fran.emailSmtp;
                smtp.password = fran.emailPassword;
                smtp.port = fran.emailPort;
                smtp.secure = fran.emailSecure;

                string oneClickLogin = Globals.BuildQueryString(Globals.BuildQueryString(Globals.baseUrlSecure + @"Login.aspx", "A", Globals.Encrypt(smtp.to)), "B", Globals.Encrypt(Globals.CustomerIDToPassphrase(customer.customerID)));

                smtp.subject = "2 Local Gals Customer Rewards";
                string htmlBody = @"
                <img src=""" + Globals.baseUrl + @"2LG_Letterhead.png""></img>
                <br/>
                <br/>
                <p>
                Hi {0},
                </p>
                <br/>
                <p>
                    Welcome to the 2 Local Gals Customer Rewards program. The Rewards program will earn you points for every dollar you spend with us. You can then use these points to pay for future appointments, or gift cards for family and friends.
                </p>
                <br/>
                <p>
                    Go to the 2 Local Gals Customer Portal to view your Rewards <a href=""" + oneClickLogin + @""">Click Here</a>
                </p>
                <br/>
                <p>We appreciate you as a customer and for the opportunity to serve you.</p>
                <p>
                    If you have any questions, please don't hesitate to call.<br/>
                    <br/>
                    Thank you,<br/>
                    2 Local Gals<br/>
                    <br/>
                    {1}<br/>
                    {2}<br/>
                    <a href=""{3}"">{3}</a><br/>
                </p>";

                smtp.body = string.Format(htmlBody, customer.firstName.Trim(), fran.phone, fran.email, fran.webLink);

                string error = SendSmtpEmail(smtp);
                if (error == null) return null;
                else return "SendEmail.SendCustomerRewardsEnabled(SEND) EX: " + error;
            }
            catch (Exception ex)
            {
                return "SendEmail.SendCustomerRewardsEnabled EX: " + ex.Message;
            }
        }
        #endregion

        #region SendReferral
        public static string SendReferral(CustomerStruct customer, string name, string toEmail)
        {
            try
            {
                if (!Globals.ValidEmail(customer.email))
                    return "Customer Email is Invalid";

                FranchiseStruct fran = Globals.GetFranchiseByMask(customer.franchiseMask);

                SmtpEmailStruct smtp = new SmtpEmailStruct();
                smtp.to = toEmail;
                smtp.from = fran.email;
                smtp.smtp = fran.emailSmtp;
                smtp.password = fran.emailPassword;
                smtp.port = fran.emailPort;
                smtp.secure = fran.emailSecure;

                smtp.subject = "You Were Referred to 2 Local Gals Housekeeping";
                string htmlBody = @"
                <img src=""" + Globals.baseUrl + @"2LG_Letterhead.png""></img>
                <br/>
                <br/>
                <p>
                Hi {0},
                </p>
                <br/>
                <p>
                    You have been referred to us by {1}. They thought you might be interested in some cleaning services. 2 Local Gals Housekeeping has been in business since 1999. We pride ourselves in being one of the best cleaning services around. We are experts in residential and office cleaning,  as well as deep cleans,  and move in/out cleans. Go to our website,  <a href=""{4}"">{4}</a>, and see everything we have to offer and get a free online quote today.
                </p>
                <br/>
                <p>We appreciate you as a future customer and for the opportunity to have the chance to serve you.</p>
                <br/>
                <p>
                    If you have any questions, please don't hesitate to call.<br/>
                    <br/>
                    Thank you,<br/>
                    2 Local Gals<br/>
                    <br/>
                    {2}<br/>
                    {3}<br/>
                    <a href=""{4}"">{4}</a><br/>
                </p>";

                smtp.body = string.Format(htmlBody, name, Globals.FormatFullName(customer.firstName, customer.lastName, "Unknown"), fran.phone, fran.email, fran.webLink);

                string error = SendSmtpEmail(smtp);
                if (error == null) return null;
                else return "SendEmail.SendReferral(SEND) EX: " + error;
            }
            catch (Exception ex)
            {
                return "SendEmail.SendReferral EX: " + ex.Message;
            }
        }
        #endregion

        #region SendWebQuoteReply
        public static string SendWebQuoteReply(string body, string customerEmail, FranchiseStruct franchise)
        {
            try
            {
                SmtpEmailStruct smtp = new SmtpEmailStruct();
                smtp.to = customerEmail;
                smtp.from = franchise.email;
                smtp.smtp = franchise.emailSmtp;
                smtp.password = franchise.emailPassword;
                smtp.port = franchise.emailPort;
                smtp.secure = franchise.emailSecure;

                smtp.subject = "2 Local Gals Web Quote";
                body = body.Replace("\r", "<br/>");
                body += @"<br/><img src=""" + Globals.baseUrl + @"2LG_EmailFooter.jpg""></img>";
                smtp.body = body;


                string error = SendSmtpEmail(smtp);
                if (error == null) return null;
                else return "SendEmail.SendWebQuoteReply(SEND) EX: " + error;
            }
            catch (Exception ex)
            {
                return "SendEmail.SendWebQuoteReply EX: " + ex.Message;
            }
        }
        #endregion

        #region SendTransaction
        public static string SendTransaction(string email, int transID)
        {
            try
            {
                if (transID > 5000)
                {
                    int franchiseMask = Globals.GetFranchiseMask();
                    TransactionStruct trans = Database.GetTransactionByID(franchiseMask, transID);
                    if (!string.IsNullOrWhiteSpace(email)) trans.email = email;
                    return SendEmail.SendTransDoc(TransDoc.GetTransactionDoc(franchiseMask, trans));
                }
                else return "EmailTransaction Failed: Invalid Transaction ID";
            }
            catch (Exception ex)
            {
                return "SendEmail.EmailTransaction EX: " + ex.Message;
            }
        }
        #endregion

        #region SendTransDoc
        public static string SendTransDoc(TransDoc transDoc)
        {
            try
            {
                if (transDoc.customer.customerID <= 0)
                {
                    return "Error Customer Lookup";
                }
                else
                {
                    if (!Globals.ValidEmail(transDoc.emailAddress))
                    {
                        return "Invalid Email Address (" + (transDoc.emailAddress ?? "NULL") + ")";
                    }
                    else
                    {
                        string html = transDoc.GetHTML();

                        SmtpEmailStruct smtp = new SmtpEmailStruct();
                        smtp.to = transDoc.emailAddress;
                        smtp.from = transDoc.franchise.email;
                        smtp.smtp = transDoc.franchise.emailSmtp;
                        smtp.password = transDoc.franchise.emailPassword;
                        smtp.port = transDoc.franchise.emailPort;
                        smtp.secure = transDoc.franchise.emailSecure;

                        smtp.subject = transDoc.emailSubject;
                        smtp.body = @"<div style=""width: 8.5in;"">" + html + @"</div>";

                        string error = SendSmtpEmail(smtp, html, smtp.subject + ".pdf");
                        if (error == null) return error;
                        else return "SendEmail.SendTransDoc(SEND) EX: " + error;
                    }
                }
            }
            catch (Exception ex)
            {
                return "SendEmail.SendTransDoc EX: " + ex.Message;
            }
        }
        #endregion

        #region SendGiftCard
        public static string SendGiftCard(int franchiseMask, int giftCardID)
        {
            try
            {
                GiftCardStruct giftCard = Database.GetGiftCardByID(franchiseMask, giftCardID);
                CustomerStruct cust;
                Database.GetCustomerByID(franchiseMask, giftCard.customerID, out cust);
                FranchiseStruct fran = Globals.GetFranchiseByMask(cust.franchiseMask);

                SmtpEmailStruct smtp = new SmtpEmailStruct();
                smtp.to = giftCard.recipientEmail;
                smtp.from = fran.email;
                smtp.smtp = fran.emailSmtp;
                smtp.password = fran.emailPassword;
                smtp.port = fran.emailPort;
                smtp.secure = fran.emailSecure;

                smtp.subject = "2 Local Gals E-Gift Card Purchase from " + giftCard.giverName;
                string htmlBody = @"
                <img src=""" + Globals.baseUrl + @"2LG_Letterhead.png""></img>
                <br/>
                <br/>
                <p>
                Hi {0},
                </p>
                <br/>
                <p>
                    {1} has purchased you a 2 Local Gals E-Gift Card to use for future housekeeping.
                    If you are an existing customer you may redeem your gift card in your customer portal (<a href=""" + Globals.baseUrlSecure + @"Protected/PortalGiftCards.aspx"">here</a>), or by calling us.
                </p>
                <br>
                <p>
                     If you are a new customer please call us at {2} to use your E-Gift Card.
                </p>
                <br/>
                <p>
                    <h1><b>E-Gift Card Amount: {4}</b></h1>
                    <h1><b>Note: {3}</b></h1>
                    <br/>
                    <h1><b>Redemption Code: {5}</b></h1>
                </p>
                <br/>
                <p>
                    If you have any questions, please don't hesitate to call.<br/>
                    <br/>
                    Thank you,<br/>
                    <br/>
                    2 Local Gals<br/>
                    <br/>
                    {6}<br/>
                    {7}<br/>
                    <a href=""{8}"">{8}</a><br/>
                </p>";

                smtp.body = string.Format(htmlBody, giftCard.recipientName, giftCard.giverName, fran.phone, giftCard.message, Globals.FormatMoney(giftCard.amount), Globals.EncodeZBase32((uint)giftCard.giftCardID), fran.phone, fran.email, fran.webLink);

                string error = SendSmtpEmail(smtp);
                if (error == null) return null;
                else return "SendEmail.SendGiftCard(SEND) EX: " + error;
            }
            catch (Exception ex)
            {
                return "SendEmail.SendGiftCard EX: " + ex.Message;
            }
        }
        #endregion

        #region SendFollowUp
        public static string SendFollowUp(int appID)
        {
            try
            {
                AppStruct app;
                Database.GetApointmentpByID(-1, appID, out app);
                FranchiseStruct franchise = Globals.GetFranchiseByMask(app.franchiseMask);
                ContractorStruct contractor = Database.GetContractorByID(-1, app.contractorID);
                FollowUpStruct followUp = Database.GetFollowUpByID(appID);

                if (followUp.schedulingSatisfaction == 0 && followUp.timeManagement == 0 && followUp.professionalism == 0 && followUp.cleaningQuality == 0)
                    return null;

                List<string> sameList = new List<string>();
                foreach (AppStruct same in Database.GetSameApointments(app))
                {
                    if (app.appType == same.appType)
                        sameList.Add(same.contractorTitle);
                }
                sameList.Insert(0, app.contractorTitle);

                if (!Globals.ValidEmail(contractor.email))
                    return "Contractor Email is Invalid";

                if (!Globals.ValidEmail(franchise.email))
                    return "Franchise Email is Invalid";

                SmtpEmailStruct smtp = new SmtpEmailStruct();
                smtp.to = contractor.email;
                smtp.from = franchise.email;
                smtp.smtp = franchise.emailSmtp;
                smtp.password = franchise.emailPassword;
                smtp.port = franchise.emailPort;
                smtp.secure = franchise.emailSecure;

                smtp.subject = "2 Local Gals Follow Up Report for " + app.customerTitle + " (" + app.appointmentDate.ToString("d") + ")";
                smtp.body = @"
                <head>
                    <style type=""text/css"">
                    p
                    {
                        font-size: 1.3em;
                    }
                    </style>
                </head>
                <h1>
                    Follow Up Report for " + app.customerTitle + @" (" + app.appointmentDate.ToString("d") + @")
                </h1>
                <p>
                    Assigned Contractors: <b>" + string.Join(", ", sameList.ToArray()) + @"</b>
                </p>
                <p>
                    Contractor Time Management: <b>" + followUp.timeManagement + @" Stars</b>
                </p>
                <p>
                    Contractor Professionalism: <b>" + followUp.professionalism + @" Stars</b>
                </p>
                <p>
                    Contractor Quality of Work: <b>" + followUp.cleaningQuality + @" Stars</b>
                </p>
                <p>
                    Job Score: <b>" + Globals.FormatHours((decimal)(followUp.timeManagement + followUp.professionalism + followUp.cleaningQuality) / 3.0m) + @" Stars</b>
                </p>
                <br/>
                <p>
                    Is there anything we could have done better for you? <b>" + followUp.notes + @"</b>
                </p>";

                string error = SendSmtpEmail(smtp);
                if (error == null) return null;
                else return "SendEmail.SendFollowUp(SEND) EX: " + error;
            }
            catch (Exception ex)
            {
                return "SendEmail.SendFollowUp EX: " + ex.Message;
            }
        }
        #endregion

        #region SendPromotion
        public static string SendPromotion(DBRow massEmail)
        {
            try
            {
                CustomerStruct cust = new CustomerStruct();
                ContractorStruct contractor = new ContractorStruct();
                FranchiseStruct fran = new FranchiseStruct();

                string firstName = null;
                string lastName = null;
                string emailAddress = null;

                if (massEmail.GetInt("customerID") > 0)
                {
                    Database.GetCustomerByID(-1, massEmail.GetInt("customerID"), out cust);
                    firstName = cust.firstName;
                    lastName = cust.lastName;
                    emailAddress = cust.email;
                    fran = Globals.GetFranchiseByMask(cust.franchiseMask);

                    if (!cust.sendPromotions)
                    {
                        return "Customer Opt-Out";
                    }

                    if (!Globals.ValidEmail(cust.email))
                    {
                        return "Invalid Email Address";
                    }
                }
                else
                {
                    contractor = Database.GetContractorByID(-1, massEmail.GetInt("contractorID"));
                    firstName = contractor.firstName;
                    lastName = contractor.lastName;
                    emailAddress = contractor.email;
                    fran = Globals.GetFranchiseByMask(contractor.franchiseMask);

                    if (!Globals.ValidEmail(contractor.email))
                    {
                        return "Invalid Email Address";
                    }
                }

                SmtpEmailStruct smtp = new SmtpEmailStruct();
                smtp.to = emailAddress;
                smtp.from = fran.email;
                smtp.smtp = fran.emailSmtp;
                smtp.password = fran.emailPassword;
                smtp.port = fran.emailPort;
                smtp.secure = fran.emailSecure;

                smtp.subject = Globals.ReplaceEmailDefinedStrings(Globals.Base64Decode(massEmail.GetString("subject")), fran, firstName, lastName);
                smtp.body = Globals.ReplaceEmailDefinedStrings(Globals.Base64Decode(massEmail.GetString("body")), fran, firstName, lastName);
                if (cust.customerID > 0) smtp.body += @"<br/><br/><a href=""" + Globals.BuildQueryString(Globals.baseUrlSecure + @"Unsubscribe.aspx", "A", Globals.Encrypt(cust.customerID.ToString())) + @""">Unsubscribe</a>";
                
                string error = SendSmtpEmail(smtp);
                if (error == null) return null;
                else return "SendEmail.SendPromotion(SEND) EX: " + error;
            }
            catch (Exception ex)
            {
                return "SendEmail.SendPromotion EX: " + ex.Message;
            }
        }
        #endregion

        public static string SendReviewUsEmail(int customerID)
        {
            CustomerStruct customer;
            string error = Database.GetCustomerByID(Globals.GetFranchiseMask(), customerID, out customer);
            if (error != null)
            {
                return "Error GetCustomerByID: " + error;
            }
            else
            {
                FranchiseStruct franchise = Globals.GetFranchiseByMask(customer.franchiseMask);
                if (!Globals.ValidEmail(customer.email))
                {
                    return "Customer Email is Invalid";
                }
                else
                {
                    SmtpEmailStruct smtp = new SmtpEmailStruct();
                    smtp.to = Globals.SplitEmail(customer.email)[0];
                    smtp.from = franchise.email;
                    smtp.smtp = franchise.emailSmtp;
                    smtp.password = franchise.emailPassword;
                    smtp.port = franchise.emailPort;
                    smtp.secure = franchise.emailSecure;

                    smtp.subject = "Thank You For Choosing Us";
                    string htmlBody = @"
                        <img src=""" + Globals.baseUrl + @"2LG_Letterhead.png""></img>
                        <br/>
                        <br/>
                        <p>
                        Hey {0},
                        </p>
                        <p>
                            This is 2 Local Gals thanking you for choosing us for your Housekeeping needs. We're reaching out because we'd love your continued support in knowing how we've personally made a difference for you - Please let us know about your experience <a href=""{4}"">{4}</a>.
                        </p>
                        <br/>
                        <p>
                            We appreciate you as a customer and for the opportunity to serve you.<br/>
                            <br/>
                            Thank you,<br/>
                            <br/>
                            2 Local Gals<br/>
                            <br/>
                            {1}<br/>
                            {2}<br/>
                            <a href=""{3}"">{3}</a><br/>
                        </p>";

                    smtp.body = string.Format(htmlBody, customer.firstName, franchise.phone, franchise.email, franchise.webLink, franchise.reviewUsLink);

                    error = SendSmtpEmail(smtp);
                    if (error == null) return null;
                    else return "SendEmail.SendReviewUsEmail EX: " + error;
                }
            }
        }

        #region SendSmtpEmail
        public struct SmtpEmailStruct
        {
            public string to;
            public string from;
            public string subject;
            public string body;
            public string smtp;
            public string password;
            public int port;
            public bool secure;
        }
        public static string SendSmtpEmail(SmtpEmailStruct email, string pdfHtml = null, string pdfName = null)
        {
            string ret = null;
            foreach (string toEmail in Globals.SplitEmail(email.to))
            {
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        if (i < 0) Thread.Sleep(1000);
                        SmtpClient client = new SmtpClient(email.smtp, email.port)
                        {
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(email.from, email.password),
                            EnableSsl = email.secure
                        };

                        MailMessage message = new MailMessage(new MailAddress(email.from), new MailAddress(toEmail))
                        {
                            Subject = email.subject,
                            Body = email.body,
                            IsBodyHtml = true,
                        };

                        message.Bcc.Add(new MailAddress(@"sent@2localgals.com"));

                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            if (pdfName != null)
                            {
                                var pdf = PdfGenerator.GeneratePdf(pdfHtml, PdfSharp.PageSize.A4);
                                pdf.Save(memoryStream);
                                memoryStream.Position = 0;
                                Attachment attachement = new Attachment(memoryStream, new ContentType(MediaTypeNames.Application.Pdf));
                                attachement.ContentDisposition.FileName = pdfName;
                                message.Attachments.Add(attachement);
                            }
                            client.Send(message);
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        ret = ex.Message;
                    }
                }
            }
            return ret;
        }
        #endregion

        #region TestSMTP
        public static void TestSMTP()
        {
            try
            {
                SmtpEmailStruct email = new SmtpEmailStruct();

                /*//GO DADDY
                email.port = 25;
                email.secure = false;
                email.smtp = @"smtpout.secureserver.net";
                email.from = @"housekeeping@2-local-gals.com";
                email.password = @"2localgals*";*/

                /*//GMAIL
                email.port = 587;
                email.secure = true;
                email.smtp = @"smtp.gmail.com";
                email.from = @"2localgals.sg@gmail.com";
                email.password = @"2lgsg*123";*/

                //hMailServer
                email.port = 25;
                email.secure = false;
                email.smtp = @"172.16.145.71";
                email.from = @"dustin@2localgals.com";
                email.password = @"1##7sinx";

                email.to = "dustinmarks07@gmail.com";
                email.subject = "TEST SUBJECT";
                email.body = "TEST BODY";

                string error = SendSmtpEmail(email);

                if (error == null)
                {
                    Database.LogThis("Test Mail Success (" + email.from + ")", null);
                }
                else
                {
                    Database.LogThis("Test Mail Error (" + email.from + ") EX: " + error, null);
                }
            }
            catch (Exception ex)
            {
                Database.LogThis("TestSMTP: " + ex.Message, ex);
            }
        }
        #endregion
    }
}