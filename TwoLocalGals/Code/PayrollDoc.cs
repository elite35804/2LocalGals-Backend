using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nexus
{
    public class PayrollDoc
    {
        public static Dictionary<KeyValuePair<int, DateTime>, bool> totalAppCount = new Dictionary<KeyValuePair<int, DateTime>, bool>();
        public static decimal totalMiles = 0;
        public static decimal totalContractorHours = 0;
        public static decimal totalCustomerHours = 0;
        public static decimal totalTips = 0;
        public static decimal totalServiceFee = 0;
        public static decimal totalSuppliesFee = 0;
        public static decimal totalCarFee = 0;
        public static decimal totalSuppliesFeeNW = 0;
        public static decimal totalCarFeeNW = 0;
        public static decimal totalSubContractor = 0;
        public static decimal totalServiceSplit = 0;
        public static decimal totalScheduleFee = 0;
        public static decimal totalAdjustments = 0;
        public static decimal totalNonWaiver = 0;
        public static decimal totalNonInsurance = 0;
        public static decimal totalPayroll = 0;
        public static decimal totalLabor = 0;
        public static decimal totalRevenue = 0;
        public static decimal totalGrossRevenue = 0;
        public static decimal totalDiscounts = 0;
        public static decimal totalSalesTax = 0;

        public ContractorStruct contractor;
        public string html = "";
        public decimal appTotal = 0;
        public decimal serviceFeeTotal = 0;
        public SendEmail.SmtpEmailStruct smtp = new SendEmail.SmtpEmailStruct();

        public static string GetPayroll(int userAccess, int franchiseMask, int contractorType, List<ContractorStruct> contractors, DateTime startDate, DateTime endDate, bool isEmail, out List<PayrollDoc> payrollList)
        {
            string ret = "";
            payrollList = new List<PayrollDoc>();

            totalAppCount = new Dictionary<KeyValuePair<int, DateTime>, bool>();
            totalMiles = 0;
            totalContractorHours = 0;
            totalCustomerHours = 0;
            totalTips = 0;
            totalServiceFee = 0;
            totalSuppliesFee = 0;
            totalCarFee = 0;
            totalSuppliesFeeNW = 0;
            totalCarFeeNW = 0;
            totalSubContractor = 0;
            totalServiceSplit = 0;
            totalScheduleFee = 0;
            totalAdjustments = 0;
            totalNonWaiver = 0;
            totalNonInsurance = 0;
            totalPayroll = 0;
            totalLabor = 0;
            totalRevenue = 0;
            totalGrossRevenue = 0;
            totalDiscounts = 0;
            totalSalesTax = 0;

            try
            {
                Globals.FormatDateRange(ref startDate, ref endDate);

                Dictionary<int, List<AppStruct>> appDict = new Dictionary<int, List<AppStruct>>();
                foreach (AppStruct app in Database.GetAppsByDateRange(franchiseMask, startDate, endDate, "A.appointmentDate, A.startTime, A.endTime, CU.firstName, CU.lastName", true))
                {
                    if (app.appStatus != 0) continue;
                    if ((Globals.IDToMask(app.appType) & contractorType) == 0) continue;
                    if (!appDict.ContainsKey(app.contractorID)) appDict.Add(app.contractorID, new List<AppStruct>());
                    appDict[app.contractorID].Add(app);
                }

                Dictionary<int, FranchiseStruct> franDict = new Dictionary<int, FranchiseStruct>();
                foreach (FranchiseStruct fran in Database.GetFranchiseList())
                    franDict.Add(fran.franchiseMask, fran);

                foreach (ContractorStruct contractor in contractors)
                {
                    if (!appDict.ContainsKey(contractor.contractorID)) continue;

                    PayrollDoc doc = new PayrollDoc();
                    doc.contractor = contractor;
                    doc.smtp.to = contractor.email;

                    foreach (FranchiseStruct franchise in franDict.Values)
                    {
                        if ((contractor.franchiseMask & franchise.franchiseMask) != 0)
                        {
                            if (!string.IsNullOrEmpty(franchise.email) && !string.IsNullOrEmpty(franchise.emailSmtp))
                            {
                                doc.smtp.from = franchise.email;
                                doc.smtp.smtp = franchise.emailSmtp;
                                doc.smtp.password = franchise.emailPassword;
                                doc.smtp.port = franchise.emailPort;
                                doc.smtp.secure = franchise.emailSecure;
                                break;
                            }
                        }
                    }

                    string lastAddr = "";
                    DateTime lastDate = DateTime.MinValue;
                    decimal subTotalHours = 0;
                    decimal subTotalMiles = 0;
                    decimal subTotalTips = 0;
                    decimal subTotalServiceFee = 0;
                    decimal subTotalSuppliesFee = 0;
                    decimal subTotalCarFee = 0;
                    decimal subTotalSuppliesFeeNW = 0;
                    decimal subTotalCarFeeNW = 0;
                    decimal subTotalSubContractor = 0;
                    decimal subTotalServiceSplit = 0;
                    decimal subTotalScheduleFee = 0;
                    decimal subTotalAdjustments = 0;
                    decimal subTotalCommission = 0;
                    decimal subTotalNonWaiver = 0;
                    decimal subTotalNonInsurance = 0;
                    decimal scheduleFeeMax = 0;
                    List<string> adjustmentList = new List<string>();

                    string dateRangeString = startDate.Date == endDate.Date ? startDate.ToString("d") : startDate.ToString("d") + " to " + endDate.ToString("d");

                    if (isEmail) doc.smtp.subject = "2 Local Gals Housekeeping Payroll For: " + contractor.title + " (" + dateRangeString + ")";
                    else if (userAccess < 7) doc.smtp.subject = @"Payroll Report for: " + contractor.title + @"&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Period: " + startDate.ToString("d") + " - " + endDate.ToString("d");
                    else doc.smtp.subject = @"Payroll Report for: <a href=""Contractors.aspx?conID=" + contractor.contractorID + @""">" + contractor.title + @"</a>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Period: " + startDate.ToString("d") + " - " + endDate.ToString("d");

                    doc.html = @"
                    <table style=""width: " + (isEmail ? "800px" : "100%") + @"; border-collapse: collapse; text-align: center;"">
                        <caption style=""padding: 10px 0px 20px 0px; text-align: left; font-size: 120%; font-weight: bold;"">" + doc.smtp.subject + @"</caption>
                        <tr>
                            <th style=""width: 100px; font-weight: bold; border-bottom: 3px solid #2B2B2B;"">Date</th>
                            <th style=""width: 100px; font-weight: bold; border-bottom: 3px solid #2B2B2B;"">Start</th>
                            <th style=""width: 100px; font-weight: bold; border-bottom: 3px solid #2B2B2B;"">End</th>
                            <th style=""width: 250px; font-weight: bold; border-bottom: 3px solid #2B2B2B;"">Customer</th>
                            <th style=""width: 100px; font-weight: bold; border-bottom: 3px solid #2B2B2B;"">Miles</th>
                            <th style=""width: 100px; font-weight: bold; border-bottom: 3px solid #2B2B2B;"">Tips</th>
                            " + ((contractor.contractorType & 1) != 0 ? @"<th style=""width: 100px; font-weight: bold; border-bottom: 3px solid #2B2B2B;"">Service Fees</th>" : "") + @"
                            " + ((contractor.contractorType & 1) != 0 ? @"<th style=""width: 100px; font-weight: bold; border-bottom: 3px solid #2B2B2B;"">Hours</th>" : "") + @"
                            " + ((contractor.contractorType >> 1) != 0 ? @"<th style=""width: 100px; font-weight: bold; border-bottom: 3px solid #2B2B2B;"">Charges</th>" : "") + @"
                        </tr>";

                    foreach (AppStruct app in appDict[contractor.contractorID])
                    {
                        if (lastDate.Date != app.appointmentDate)
                        {
                            lastAddr = Globals.CleanAddr(contractor.address) + "," + Globals.CleanAddr(contractor.city) + "," + Globals.CleanAddr(contractor.state) + "," + Globals.CleanAddr(contractor.zip);
                            lastDate = app.appointmentDate;
                        }

                        string routeAddr = Globals.CleanAddr(app.customerAddress) + "," + Globals.CleanAddr(app.customerCity) + "," + Globals.CleanAddr(app.customerState) + "," + Globals.CleanAddr(app.customerZip);
                        decimal miles = GoogleMaps.GetDrivingRoute(lastAddr, routeAddr).distance;
                        lastAddr = routeAddr;

                        doc.html += @"
                        <tr>
                            <td style=""padding: 3px 0px 3px 0px; border-bottom: 1px solid #2B2B2B;"">" + (userAccess < 7 ? app.appointmentDate.ToString("d") : @"<a href=""Appointments.aspx?appID=" + app.appointmentID + @""">" + app.appointmentDate.ToString("d") + @"</a>") + @"</td>
                            <td style=""padding: 3px 0px 3px 0px; border-bottom: 1px solid #2B2B2B;"">" + app.startTime.ToString("t") + @"</td>
                            <td style=""padding: 3px 0px 3px 0px; border-bottom: 1px solid #2B2B2B;"">" + app.endTime.ToString("t") + @"</td>
                            <td style=""padding: 3px 0px 3px 0px; border-bottom: 1px solid #2B2B2B;"">" + (userAccess < 7 ? app.customerTitle : @"<a href=""Customers.aspx?custID=" + app.customerID + @""">" + app.customerTitle + @"</a>") + @"</td>
                            <td style=""padding: 3px 0px 3px 0px; border-bottom: 1px solid #2B2B2B;"">" + miles.ToString("N2") + @"</td>
                            <td style=""padding: 3px 0px 3px 0px; border-bottom: 1px solid #2B2B2B;"">" + Globals.FormatMoney(app.contractorTips) + @"</td>
                            " + ((contractor.contractorType & 1) != 0 ? @"<td style=""padding: 3px 0px 3px 0px; border-bottom: 1px solid #2B2B2B;"">" + Globals.FormatMoney(app.customerServiceFee) + @"</td>" : "") + @"
                            " + ((contractor.contractorType & 1) != 0 ? @"<td style=""padding: 3px 0px 3px 0px; border-bottom: 1px solid #2B2B2B;"">" + app.contractorHours.ToString("N2") + (app.contractorRate == contractor.hourlyRate ? "" : " @ " + Globals.FormatMoney(app.contractorRate)) + @"</td>" : "") + @"
                            " + ((contractor.contractorType >> 1) != 0 ? @"<td style=""padding: 3px 0px 3px 0px; border-bottom: 1px solid #2B2B2B;"">" + Globals.FormatMoney(app.customerSubContractor) + @"</td>" : "") + @"
                        </tr>";
                        
                        decimal feePercent = franDict.ContainsKey(app.franchiseMask) ? Globals.ParseScheduleFee(franDict[app.franchiseMask].scheduleFeeString, app.appointmentDate) : 0;
                        if (feePercent > scheduleFeeMax) scheduleFeeMax = feePercent;

                        decimal appTotalCommission = (app.contractorRate * app.contractorHours) + (app.customerSubContractor * (contractor.serviceSplit / 100.0m));
                        decimal appScheduleFee = (app.contractorRate * app.contractorHours) * (feePercent / 100.0m);

                        subTotalHours += app.contractorHours;
                        subTotalMiles += miles;
                        subTotalTips += app.contractorTips;
                        subTotalServiceFee += app.customerServiceFee;
                        subTotalSuppliesFee += app.customerServiceFee * (franDict[app.franchiseMask].suppliesPercentage / 100.0m);
                        subTotalCarFee += app.customerServiceFee * (franDict[app.franchiseMask].carPercentage / 100.0m);
                        subTotalSubContractor += app.customerSubContractor;
                        subTotalServiceSplit += app.customerSubContractor * ((100.0m - contractor.serviceSplit) / 100.0m);
                        subTotalScheduleFee += appScheduleFee;
                        subTotalAdjustments += app.contractorAdjustAmount;
                        subTotalCommission += appTotalCommission;

                        totalSalesTax += Globals.ExtractSalesTaxFromTotal(Globals.CalculateAppointmentTotal(app), app.contractorTips, app.salesTax);

                        if (!Globals.WithinContractYear(contractor.waiverDate, app.appointmentDate) && !Globals.WithinContractYear(contractor.waiverUpdateDate, app.appointmentDate))
                        {
                            subTotalNonWaiver += appTotalCommission + app.contractorTips + app.customerServiceFee + app.contractorAdjustAmount - appScheduleFee;
                            subTotalSuppliesFeeNW += app.customerServiceFee * (franDict[app.franchiseMask].suppliesPercentage / 100.0m);
                            subTotalCarFeeNW += app.customerServiceFee * (franDict[app.franchiseMask].carPercentage / 100.0m);
                        }

                        if (!Globals.WithinContractYear(contractor.insuranceDate, app.appointmentDate) && !Globals.WithinContractYear(contractor.insuranceUpdateDate, app.appointmentDate))
                        {
                            subTotalNonInsurance += appTotalCommission + app.contractorTips + app.customerServiceFee + app.contractorAdjustAmount - appScheduleFee;
                        }

                        if (app.contractorAdjustAmount != 0)
                            adjustmentList.Add((app.contractorAdjustType ?? "Unknown") + " " + Globals.FormatMoney(app.contractorAdjustAmount));

                        if (!totalAppCount.ContainsKey(new KeyValuePair<int, DateTime>(app.customerID, app.appointmentDate)))
                            totalAppCount.Add(new KeyValuePair<int, DateTime>(app.customerID, app.appointmentDate), true);

                        if (app.customerAccountStatus != "Ignored")
                        {
                            decimal revenue = app.appType <= 1 ? (app.customerRate * app.customerHours) : 0;
                            totalRevenue += revenue;
                            totalRevenue += app.customerSubContractor;
                            totalGrossRevenue += Globals.CalculateAppointmentTotal(app);
                            totalDiscounts += Globals.CalculateDiscountPercent(app.appType <= 1 ? app.customerHours : 0, app.customerRate, app.customerServiceFee, app.customerDiscountPercent + app.customerDiscountReferral, app.appointmentDate);
                            totalDiscounts += app.customerDiscountAmount;
                            totalContractorHours += app.contractorHours;
                            totalCustomerHours += app.appType <= 1 ? app.customerHours : 0;
                            totalMiles += miles;
                        }
                    }

                    doc.appTotal = subTotalCommission + subTotalTips + subTotalServiceFee + subTotalAdjustments - subTotalScheduleFee;
                    doc.serviceFeeTotal = subTotalServiceFee;
                    decimal adjustedHourly = subTotalHours == 0 ? 0 : doc.appTotal / subTotalHours; //OLD WAY: (subTotalCommission + subTotalTips + subTotalServiceFee - subTotalScheduleFee) / subTotalHours;

                    doc.html += @"
                    </table>
                    <table style=""page-break-after: always; margin-top: 20px; margin-bottom: 100px; width: 100%; border-collapse: collapse; text-align: right; " + (isEmail ? "width: 800px;" : "width: 100%;") + @""">
                        <tr style=""text-align: left;"">
                            <td colspan=""2"" style=""padding: 3px 0px 3px 0px;""><b>Total Miles: </b>" + subTotalMiles.ToString("N2") + @"</td>
                        </tr>";

                    if ((contractor.contractorType & 1) != 0)
                    {
                        doc.html += @"
                        <tr>
                            <th style=""font-weight: bold; padding: 3px 0px 3px 0px;"">Total Hours:</th>
                            <td style=""width: 200px; padding: 3px 0px 3px 0px;"">" + subTotalHours.ToString("N2") + @"</td>
                        </tr>
                        <tr>
                            <th style=""font-weight: bold; padding: 3px 0px 3px 0px;"">Hourly Rate:</th>
                            <td style=""width: 200px; padding: 3px 0px 3px 0px;"">(" + Globals.FormatMoney(adjustedHourly) + ") " + Globals.FormatMoney(contractor.hourlyRate) + @"</td>
                        </tr>";
                    }

                    if ((contractor.contractorType >> 1) != 0)
                    {
                        doc.html += @"
                        <tr>
                            <th style=""font-weight: bold; padding: 3px 0px 3px 0px;"">Charges:</th>
                            <td style=""width: 200px; padding: 3px 0px 3px 0px;"">" + Globals.FormatMoney(subTotalSubContractor) + @"</td>
                        </tr>
                        <tr>
                            <th style=""font-weight: bold; padding: 3px 0px 3px 0px;"">Charge Split (" + contractor.serviceSplit.ToString("N0") + @"%):</th>
                            <td style=""width: 200px; padding: 3px 0px 3px 0px;"">" + Globals.FormatMoney(subTotalSubContractor - subTotalServiceSplit) + @"</td>
                        </tr>";
                    } 


                    if ((contractor.contractorType & 1) != 0)
                    {
                        doc.html += @"
                        <tr>
                            <th style=""font-weight: bold; padding: 3px 0px 3px 0px;"">Amount of Commission:</th>
                            <td style=""width: 200px; padding: 3px 0px 3px 0px;""><b>" + Globals.FormatMoney(subTotalCommission) + @"</b></td>
                        </tr>
                        <tr>
                            <th style=""font-weight: bold; padding: 3px 0px 3px 0px;"">Scheduling Fee (" + scheduleFeeMax.ToString("N0") + @"%):</th>
                            <td style=""width: 200px; padding: 3px 0px 3px 0px;"">" + Globals.FormatMoney(subTotalScheduleFee) + @"</td>
                        </tr>";

                        if (subTotalSuppliesFee > 0 || subTotalCarFee > 0)
                        {
                            doc.html += @"
                            <tr>
                                <th style=""font-weight: bold; padding: 3px 0px 3px 0px;"">Cleaning Equipment and Supplies:</th>
                                <td style=""width: 200px; padding: 3px 0px 3px 0px;"">" + Globals.FormatMoney(subTotalSuppliesFee) + @"</td>
                            </tr>
                            <tr>
                                <th style=""font-weight: bold; padding: 3px 0px 3px 0px;"">Fuel and Car Maintenance:</th>
                                <td style=""width: 200px; padding: 3px 0px 3px 0px;"">" + Globals.FormatMoney(subTotalCarFee) + @"</td>
                            </tr>
                            <tr>
                                <th style=""font-weight: bold; padding: 3px 0px 3px 0px;"">Total Service Fee:</th>
                                <td style=""width: 200px; padding: 3px 0px 3px 0px;""><b>" + Globals.FormatMoney(subTotalServiceFee) + @"</b></td>
                            </tr>";
                        }
                        else
                        {
                            doc.html += @"
                            <tr>
                                <th style=""font-weight: bold; padding: 3px 0px 3px 0px;"">Service Fee:</th>
                                <td style=""width: 200px; padding: 3px 0px 3px 0px;"">" + Globals.FormatMoney(subTotalServiceFee) + @"</td>
                            </tr>";
                        }
                
                    }

                    doc.html += @"
                        <tr>
                            <th style=""font-weight: bold; padding: 3px 0px 3px 0px;"">Tips:</th>
                            <td style=""width: 200px; padding: 3px 0px 3px 0px;"">" + Globals.FormatMoney(subTotalTips) + @"</td>
                        </tr>
                        <tr>
                            <th style=""font-weight: bold; padding: 3px 0px 3px 0px;"">Other Adjustments" + (adjustmentList.Count > 0 ? @" (" + string.Join(", ", adjustmentList.ToArray()) + @")" : "") + @":</th>
                            <td style=""width: 200px; padding: 3px 0px 3px 0px;"">" + Globals.FormatMoney(subTotalAdjustments) + @"</td>
                        </tr>";
                    if (userAccess >= 7)
                    {
                        if (doc.appTotal - subTotalNonWaiver > 0)
                        {
                            doc.html += @"
                            <tr>
                                <th style=""font-weight: bold; padding: 3px 0px 3px 0px;"">(" + contractor.waiverDate.ToString("d") + (contractor.waiverUpdateDate > contractor.waiverDate ? ", Updated: " + contractor.waiverUpdateDate.ToString("d") : "") + @") Waiver Payroll Total:</th>
                                <td style=""width: 200px; padding: 3px 0px 3px 0px;"">" + Globals.FormatMoney(doc.appTotal - subTotalNonWaiver) + @"</td>
                            </tr>";
                        }
                        if (subTotalNonWaiver > 0)
                        {
                            doc.html += @"
                            <tr>
                                <th style=""font-weight: bold; padding: 3px 0px 3px 0px;"">Non-Waiver Payroll Total:</th>
                                <td style=""width: 200px; padding: 3px 0px 3px 0px;"">" + Globals.FormatMoney(subTotalNonWaiver) + @"</td>
                            </tr>";
                        }
                        if (doc.appTotal - subTotalNonInsurance > 0)
                        {
                            doc.html += @"
                        <tr>
                            <th style=""font-weight: bold; padding: 3px 0px 3px 0px;"">(" + contractor.insuranceDate.ToString("d") + (contractor.insuranceUpdateDate > contractor.insuranceDate ? ", Updated: " + contractor.insuranceUpdateDate.ToString("d") : "") + @") Insurance Payroll Total:</th>
                            <td style=""width: 200px; padding: 3px 0px 3px 0px;"">" + Globals.FormatMoney(doc.appTotal - subTotalNonInsurance) + @"</td>
                        </tr>";
                        }
                        if (subTotalNonInsurance > 0)
                        {
                            doc.html += @"
                        <tr>
                            <th style=""font-weight: bold; padding: 3px 0px 3px 0px;"">Non-Insurance Payroll Total:</th>
                            <td style=""width: 200px; padding: 3px 0px 3px 0px;"">" + Globals.FormatMoney(subTotalNonInsurance) + @"</td>
                        </tr>";
                        }
                    }
                    doc.html += @"
                        <tr>
                            <th style=""font-weight: bold; padding: 3px 0px 3px 0px;"">Total:</th>
                            <td style=""width: 200px; padding: 3px 0px 3px 0px;""><h2>" + Globals.FormatMoney(doc.appTotal) + @"</h2></td>
                        </tr>
                    </table>";

                    totalTips += subTotalTips;
                    totalServiceFee += subTotalServiceFee;
                    totalSubContractor += subTotalSubContractor;
                    totalServiceSplit += subTotalServiceSplit;
                    totalSuppliesFee += subTotalSuppliesFee;
                    totalCarFee += subTotalCarFee;
                    totalSuppliesFeeNW += subTotalSuppliesFeeNW;
                    totalCarFeeNW += subTotalCarFeeNW;
                    totalScheduleFee += subTotalScheduleFee;
                    totalAdjustments += subTotalAdjustments;
                    totalLabor += subTotalCommission;
                    totalNonWaiver += subTotalNonWaiver;
                    totalNonInsurance += subTotalNonInsurance;
                    totalPayroll += doc.appTotal;

                    doc.smtp.body = doc.html;
                    payrollList.Add(doc);
                }
            }
            catch (Exception ex)
            {
                ret += "SendEmail.SendContractorPayroll EX: " + ex.Message + "<br/>";
                Database.LogThis("SendContractorPayroll", ex);
            }
            return ret == "" ? null : ret;
        }
    }
}