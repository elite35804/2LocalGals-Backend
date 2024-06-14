using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using Nexus;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Diagnostics;

namespace TwoLocalGals.Protected
{
    public partial class ExportExcel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Globals.GetUserAccess(this) < 7)
                    Globals.LogoutUser(this);

                switch (Request.QueryString["Report"])
                {
                    default:
                        ExportCustomerList(); 
                        break;
                    case "19":
                        ExportWCFAudit();
                        break;
                }
            }
            catch { }
        }

        private bool ExportWCFAudit()
        {
            try
            {
                if (Globals.GetUserAccess(this) < 7)
                    Globals.LogoutUser(this);

                DateTime startDate = Globals.SafeDateParse(Request["startDate"]);
                DateTime endDate = Globals.SafeDateParse(Request["endDate"]);
                Globals.FormatDateRange(ref startDate, ref endDate);

                int franchiseMask = Globals.SafeIntParse(Request["mask"]);
                int contractorMask = Globals.SafeIntParse(Request["contractorMask"]);

                List<ContractorStruct> contractorList = Database.GetContractorList(franchiseMask, contractorMask, false, false, false, false, "paymentType, lastName, firstName");

                Dictionary<int, List<AppStruct>> appDict = new Dictionary<int, List<AppStruct>>();
                foreach (AppStruct app in Database.GetAppsByDateRange(franchiseMask, startDate, endDate, "A.appointmentID", true))
                {
                    if (app.appStatus != 0) continue;
                    if ((Globals.IDToMask(app.appType) & contractorMask) == 0) continue;
                    if (!appDict.ContainsKey(app.contractorID)) appDict.Add(app.contractorID, new List<AppStruct>());
                    appDict[app.contractorID].Add(app);
                }

                Dictionary<int, FranchiseStruct> franDict = new Dictionary<int, FranchiseStruct>();
                foreach (FranchiseStruct fran in Database.GetFranchiseList())
                    franDict.Add(fran.franchiseMask, fran);

                ExcelPackage package = new ExcelPackage();
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add("ExportWCFAudit");
                workSheet.DefaultColWidth = 20;

                workSheet.Cells[1, 1].Value = "Type";
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Column(1).Width = 20;

                workSheet.Cells[1, 2].Value = "Contractor";
                workSheet.Cells[1, 2].Style.Font.Bold = true;
                workSheet.Cells[1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Column(2).Width = 50;

                workSheet.Cells[1, 3].Value = "Insurance Date";
                workSheet.Cells[1, 3].Style.Font.Bold = true;
                workSheet.Cells[1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Column(3).Width = 20;

                workSheet.Cells[1, 4].Value = "(Liability) Insured Amt";
                workSheet.Cells[1, 4].Style.Font.Bold = true;
                workSheet.Cells[1, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Column(4).Width = 20;

                workSheet.Cells[1, 5].Value = "Uninsured Amt";
                workSheet.Cells[1, 5].Style.Font.Bold = true;
                workSheet.Cells[1, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Column(5).Width = 20;

                workSheet.Cells[1, 6].Value = "Waiver Date";
                workSheet.Cells[1, 6].Style.Font.Bold = true;
                workSheet.Cells[1, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Column(6).Width = 20;

                workSheet.Cells[1, 7].Value = "Waiver Payroll";
                workSheet.Cells[1, 7].Style.Font.Bold = true;
                workSheet.Cells[1, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Column(7).Width = 20;

                workSheet.Cells[1, 8].Value = "Non-Waiver Payroll";
                workSheet.Cells[1, 8].Style.Font.Bold = true;
                workSheet.Cells[1, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Column(8).Width = 20;

                workSheet.Cells[1, 9].Value = "Reimbursed Equip/Supplies";
                workSheet.Cells[1, 9].Style.Font.Bold = true;
                workSheet.Cells[1, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Column(9).Width = 20;

                workSheet.Cells[1, 10].Value = "Reimbursed Fuel/Car Maint.";
                workSheet.Cells[1, 10].Style.Font.Bold = true;
                workSheet.Cells[1, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Column(10).Width = 20;

                workSheet.Cells[1, 11].Value = "Total SF";
                workSheet.Cells[1, 11].Style.Font.Bold = true;
                workSheet.Cells[1, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 11].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Column(11).Width = 20;

                workSheet.Cells[1, 12].Value = "Tips";
                workSheet.Cells[1, 12].Style.Font.Bold = true;
                workSheet.Cells[1, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 12].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Column(12).Width = 20;

                workSheet.Cells[1, 13].Value = "Total Payroll";
                workSheet.Cells[1, 13].Style.Font.Bold = true;
                workSheet.Cells[1, 13].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 13].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Column(13).Width = 20;

                decimal totalNonInsurance = 0;
                decimal totalNonWaiver = 0;
                decimal totalSuppliesFee = 0;
                decimal totalCarFee = 0;
                decimal totalServiceFee = 0;
                decimal totalTips = 0;
                decimal total = 0;

                int index = 2;
                foreach (ContractorStruct contractor in contractorList)
                {
                    if (!appDict.ContainsKey(contractor.contractorID)) continue;

                    decimal subTotalTips = 0;
                    decimal subTotalServiceFee = 0;
                    decimal subTotalSuppliesFee = 0;
                    decimal subTotalCarFee = 0;
                    decimal subTotalSubContractor = 0;
                    decimal subTotalServiceSplit = 0;
                    decimal subTotalScheduleFee = 0;
                    decimal subTotalAdjustments = 0;
                    decimal subTotal = 0;
                    decimal subTotalNonWaiver = 0;
                    decimal subTotalNonInsurance = 0;

                    foreach (AppStruct app in appDict[contractor.contractorID])
                    {
                        decimal feePercent = franDict.ContainsKey(app.franchiseMask) ? Globals.ParseScheduleFee(franDict[app.franchiseMask].scheduleFeeString, app.appointmentDate) : 0;
                        decimal appTotalCommission = (app.contractorRate * app.contractorHours) + (app.customerSubContractor * (contractor.serviceSplit / 100.0m));
                        decimal appScheduleFee = (app.contractorRate * app.contractorHours) * (feePercent / 100.0m);

                        subTotalTips += app.contractorTips;
                        subTotalServiceFee += app.customerServiceFee;
                        subTotalSuppliesFee += app.customerServiceFee * (franDict[app.franchiseMask].suppliesPercentage / 100.0m);
                        subTotalCarFee += app.customerServiceFee * (franDict[app.franchiseMask].carPercentage / 100.0m);
                        subTotalSubContractor += app.customerSubContractor;
                        subTotalServiceSplit += app.customerSubContractor * ((100.0m - contractor.serviceSplit) / 100.0m);
                        subTotalScheduleFee += appScheduleFee;
                        subTotalAdjustments += app.contractorAdjustAmount;




                        if (!Globals.WithinContractYear(contractor.insuranceDate,  app.appointmentDate) && !Globals.WithinContractYear(contractor.insuranceUpdateDate, app.appointmentDate))
                        {
                            subTotalNonInsurance += appTotalCommission + app.contractorTips + app.customerServiceFee + app.contractorAdjustAmount - appScheduleFee;
                        }

                        if (!Globals.WithinContractYear(contractor.waiverDate, app.appointmentDate) && !Globals.WithinContractYear(contractor.waiverUpdateDate, app.appointmentDate))
                        {
                            subTotalNonWaiver += appTotalCommission + app.contractorTips + app.customerServiceFee + app.contractorAdjustAmount - appScheduleFee;
                        }

                        subTotal += appTotalCommission + app.contractorTips + app.customerServiceFee + app.contractorAdjustAmount - appScheduleFee;
                    }

                    totalNonInsurance += subTotalNonInsurance;
                    totalNonWaiver += subTotalNonWaiver;
                    totalSuppliesFee += subTotalSuppliesFee;
                    totalCarFee += subTotalCarFee;
                    totalServiceFee += subTotalServiceFee;
                    totalTips += subTotalTips;
                    total += subTotal;

                    Color color = Color.FromArgb(223, 237, 216);

                    workSheet.Cells[index, 1].Value = "Residential Janitorial";
                    workSheet.Cells[index, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells[index, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[index, 1].Style.Fill.BackgroundColor.SetColor(color);
                    workSheet.Cells[index, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    workSheet.Cells[index, 2].Value = contractor.title;
                    workSheet.Cells[index, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells[index, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[index, 2].Style.Fill.BackgroundColor.SetColor(color);
                    workSheet.Cells[index, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    workSheet.Cells[index, 3].Value = contractor.insuranceDate > new DateTime(1990, 1, 1) ? contractor.insuranceDate.ToString("d") : "";
                    if (contractor.insuranceUpdateDate > new DateTime(1990, 1, 1))
                        workSheet.Cells[index, 3].Value += ", " + contractor.insuranceUpdateDate.ToString("d");
                    workSheet.Cells[index, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells[index, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[index, 3].Style.Fill.BackgroundColor.SetColor(color);
                    workSheet.Cells[index, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    workSheet.Cells[index, 4].Value = Globals.FormatMoney(subTotal - subTotalNonInsurance);
                    workSheet.Cells[index, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells[index, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[index, 4].Style.Fill.BackgroundColor.SetColor(color);
                    workSheet.Cells[index, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    workSheet.Cells[index, 5].Value = Globals.FormatMoney(subTotalNonInsurance);
                    workSheet.Cells[index, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells[index, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[index, 5].Style.Fill.BackgroundColor.SetColor(color);
                    workSheet.Cells[index, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    workSheet.Cells[index, 6].Value = contractor.waiverDate > new DateTime(1990, 1, 1) ? contractor.waiverDate.ToString("d") : "";
                    if (contractor.waiverUpdateDate > new DateTime(1990, 1, 1))
                        workSheet.Cells[index, 6].Value += ", " + contractor.waiverUpdateDate.ToString("d");
                    workSheet.Cells[index, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells[index, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[index, 6].Style.Fill.BackgroundColor.SetColor(color);
                    workSheet.Cells[index, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    workSheet.Cells[index, 7].Value = Globals.FormatMoney(subTotal - subTotalNonWaiver);
                    workSheet.Cells[index, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells[index, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[index, 7].Style.Fill.BackgroundColor.SetColor(color);
                    workSheet.Cells[index, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    workSheet.Cells[index, 8].Value = Globals.FormatMoney(subTotalNonWaiver);
                    workSheet.Cells[index, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells[index, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[index, 8].Style.Fill.BackgroundColor.SetColor(color);
                    workSheet.Cells[index, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    workSheet.Cells[index, 9].Value = Globals.FormatMoney(subTotalSuppliesFee);
                    workSheet.Cells[index, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells[index, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[index, 9].Style.Fill.BackgroundColor.SetColor(color);
                    workSheet.Cells[index, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    workSheet.Cells[index, 10].Value = Globals.FormatMoney(subTotalCarFee);
                    workSheet.Cells[index, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells[index, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[index, 10].Style.Fill.BackgroundColor.SetColor(color);
                    workSheet.Cells[index, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    workSheet.Cells[index, 11].Value = Globals.FormatMoney(subTotalServiceFee);
                    workSheet.Cells[index, 11].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells[index, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[index, 11].Style.Fill.BackgroundColor.SetColor(color);
                    workSheet.Cells[index, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    workSheet.Cells[index, 12].Value = subTotalTips <= 0.0m ? "" : Globals.FormatMoney(subTotalTips);
                    workSheet.Cells[index, 12].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells[index, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[index, 12].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(222, 222, 222));
                    workSheet.Cells[index, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    workSheet.Cells[index, 13].Value = Globals.FormatMoney(subTotal);
                    workSheet.Cells[index, 13].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells[index, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[index, 13].Style.Fill.BackgroundColor.SetColor(color);
                    workSheet.Cells[index, 13].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    index++;
                }


                workSheet.Cells[index, 1].Value = "Totals";
                workSheet.Cells[index, 1].Style.Font.Bold = true;
                workSheet.Cells[index, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells[index, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[index, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(158, 158, 158));
                workSheet.Cells[index, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                workSheet.Cells[index, 2].Value = "";
                workSheet.Cells[index, 2].Style.Font.Bold = true;
                workSheet.Cells[index, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells[index, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[index, 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(158, 158, 158));
                workSheet.Cells[index, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                workSheet.Cells[index, 3].Value = "";
                workSheet.Cells[index, 3].Style.Font.Bold = true;
                workSheet.Cells[index, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells[index, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[index, 3].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(158, 158, 158));
                workSheet.Cells[index, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                workSheet.Cells[index, 4].Value = Globals.FormatMoney(total - totalNonInsurance);
                workSheet.Cells[index, 4].Style.Font.Bold = true;
                workSheet.Cells[index, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells[index, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[index, 4].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(158, 158, 158));
                workSheet.Cells[index, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                workSheet.Cells[index, 5].Value = Globals.FormatMoney(totalNonInsurance);
                workSheet.Cells[index, 5].Style.Font.Bold = true;
                workSheet.Cells[index, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells[index, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[index, 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(158, 158, 158));
                workSheet.Cells[index, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                workSheet.Cells[index, 6].Value = "";
                workSheet.Cells[index, 6].Style.Font.Bold = true;
                workSheet.Cells[index, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells[index, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[index, 6].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(158, 158, 158));
                workSheet.Cells[index, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                workSheet.Cells[index, 7].Value = Globals.FormatMoney(total - totalNonWaiver);
                workSheet.Cells[index, 7].Style.Font.Bold = true;
                workSheet.Cells[index, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells[index, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[index, 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(158, 158, 158));
                workSheet.Cells[index, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                workSheet.Cells[index, 8].Value = Globals.FormatMoney(totalNonWaiver);
                workSheet.Cells[index, 8].Style.Font.Bold = true;
                workSheet.Cells[index, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells[index, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[index, 8].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(158, 158, 158));
                workSheet.Cells[index, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                workSheet.Cells[index, 9].Value = Globals.FormatMoney(totalSuppliesFee);
                workSheet.Cells[index, 9].Style.Font.Bold = true;
                workSheet.Cells[index, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells[index, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[index, 9].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(158, 158, 158));
                workSheet.Cells[index, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                workSheet.Cells[index, 10].Value = Globals.FormatMoney(totalCarFee);
                workSheet.Cells[index, 10].Style.Font.Bold = true;
                workSheet.Cells[index, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells[index, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[index, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(158, 158, 158));
                workSheet.Cells[index, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                workSheet.Cells[index, 11].Value = Globals.FormatMoney(totalServiceFee);
                workSheet.Cells[index, 11].Style.Font.Bold = true;
                workSheet.Cells[index, 11].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells[index, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[index, 11].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(158, 158, 158));
                workSheet.Cells[index, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                workSheet.Cells[index, 12].Value = totalTips <= 0.0m ? "" : Globals.FormatMoney(totalTips);
                workSheet.Cells[index, 12].Style.Font.Bold = true;
                workSheet.Cells[index, 12].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells[index, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[index, 12].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(158, 158, 158));
                workSheet.Cells[index, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                workSheet.Cells[index, 13].Value = Globals.FormatMoney(total);
                workSheet.Cells[index, 13].Style.Font.Bold = true;
                workSheet.Cells[index, 13].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells[index, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[index, 13].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(158, 158, 158));
                workSheet.Cells[index, 13].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                package.SaveAs(Response.OutputStream);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=WCFAudit(" + DateTime.Now.ToString("MM-dd-yy") + ").xlsx");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ExportWCFAudit EX: " + ex.Message);
                return false;
            }
        }

        private bool ExportCustomerList()
        {
            try
            {
                ExcelPackage package = new ExcelPackage();
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add("Customers");
                workSheet.DefaultColWidth = 20;

                workSheet.Cells[1, 1].Value = "Franchise";
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Column(1).Width = 20;
                workSheet.Cells[1, 2].Value = "FirstName";
                workSheet.Cells[1, 2].Style.Font.Bold = true;
                workSheet.Column(2).Width = 20;
                workSheet.Cells[1, 3].Value = "Last Name";
                workSheet.Cells[1, 3].Style.Font.Bold = true;
                workSheet.Column(3).Width = 20;
                workSheet.Cells[1, 4].Value = "Business Name";
                workSheet.Cells[1, 4].Style.Font.Bold = true;
                workSheet.Column(4).Width = 20;
                workSheet.Cells[1, 5].Value = "Status";
                workSheet.Cells[1, 5].Style.Font.Bold = true;
                workSheet.Column(5).Width = 15;
                workSheet.Cells[1, 6].Value = "Advertisement";
                workSheet.Cells[1, 6].Style.Font.Bold = true;
                workSheet.Column(6).Width = 20;
                workSheet.Cells[1, 7].Value = "Payment Type";
                workSheet.Cells[1, 7].Style.Font.Bold = true;
                workSheet.Column(7).Width = 20;
                workSheet.Cells[1, 8].Value = "Address";
                workSheet.Cells[1, 8].Style.Font.Bold = true;
                workSheet.Column(8).Width = 50;
                workSheet.Cells[1, 9].Value = "City";
                workSheet.Cells[1, 9].Style.Font.Bold = true;
                workSheet.Column(9).Width = 20;
                workSheet.Cells[1, 10].Value = "State";
                workSheet.Cells[1, 10].Style.Font.Bold = true;
                workSheet.Column(10).Width = 10;
                workSheet.Cells[1, 11].Value = "Zip";
                workSheet.Cells[1, 11].Style.Font.Bold = true;
                workSheet.Column(11).Width = 10;
                workSheet.Cells[1, 12].Value = "Best Phone";
                workSheet.Cells[1, 12].Style.Font.Bold = true;
                workSheet.Column(12).Width = 20;
                workSheet.Cells[1, 13].Value = "Alt Phone";
                workSheet.Cells[1, 13].Style.Font.Bold = true;
                workSheet.Column(13).Width = 20;
                workSheet.Cells[1, 14].Value = "Alt Phone";
                workSheet.Cells[1, 14].Style.Font.Bold = true;
                workSheet.Column(14).Width = 20;
                workSheet.Cells[1, 15].Value = "Email";
                workSheet.Cells[1, 15].Style.Font.Bold = true;
                workSheet.Column(15).Width = 30;
                workSheet.Cells[1, 16].Value = "Service Type";
                workSheet.Cells[1, 16].Style.Font.Bold = true;
                workSheet.Column(16).Width = 30;
                workSheet.Cells[1, 17].Value = "Last Service";
                workSheet.Cells[1, 17].Style.Font.Bold = true;
                workSheet.Column(17).Width = 20;

                Dictionary<int, string> franDict = new Dictionary<int, string>();
                foreach (FranchiseStruct franchise in Database.GetFranchiseList())
                    franDict.Add(Globals.IDToMask(franchise.franchiseID), franchise.franchiseName);

                CustomerStruct[] customerList = Database.GetCustomersExcelReport(Globals.GetFranchiseMask());

                for (int i = 0; i < customerList.Length; i++)
                {
                    List<string> typeList = new List<string>();
                    foreach (int index in Globals.BitMaskToIndexList(customerList[i].sectionMask))
                        typeList.Add(Globals.IndexToServiceType(index));

                    if (franDict.ContainsKey(customerList[i].franchiseMask))
                        workSheet.Cells[i + 2, 1].Value = franDict[customerList[i].franchiseMask];
                    workSheet.Cells[i + 2, 2].Value = customerList[i].firstName;
                    workSheet.Cells[i + 2, 3].Value = customerList[i].lastName;
                    workSheet.Cells[i + 2, 4].Value = customerList[i].businessName;
                    workSheet.Cells[i + 2, 5].Value = customerList[i].accountStatus;
                    workSheet.Cells[i + 2, 6].Value = customerList[i].advertisement;
                    workSheet.Cells[i + 2, 7].Value = customerList[i].paymentType;
                    workSheet.Cells[i + 2, 8].Value = customerList[i].locationAddress;
                    workSheet.Cells[i + 2, 9].Value = customerList[i].locationCity;
                    workSheet.Cells[i + 2, 10].Value = customerList[i].locationState;
                    workSheet.Cells[i + 2, 11].Value = customerList[i].locationZip;
                    workSheet.Cells[i + 2, 12].Value = customerList[i].bestPhone;
                    workSheet.Cells[i + 2, 13].Value = customerList[i].alternatePhoneOne;
                    workSheet.Cells[i + 2, 14].Value = customerList[i].alternatePhoneTwo;
                    workSheet.Cells[i + 2, 15].Value = customerList[i].email;
                    workSheet.Cells[i + 2, 16].Value = string.Join(", ", typeList);
                    workSheet.Cells[i + 2, 17].Value = customerList[i].lastUpdate == DateTime.MinValue ? "" : customerList[i].lastUpdate.ToString("d");
                }

                package.SaveAs(Response.OutputStream);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=CustomerData(" + DateTime.Now.ToString("MM-dd-yy") + ").xlsx");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}