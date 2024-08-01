using Nexus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using TwoLocalGals.DTO;
using System.Security.Claims;
using Nexus.Protected;
using System.Web.UI.WebControls;
using TwoLocalGals.Protected;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Data;
using System.Web.Routing;
using System.Drawing.Imaging;
using System.Net.Http;
using System.Windows.Forms.VisualStyles;
using RingCentral;
using System.Diagnostics.Contracts;


namespace TwoLocalGals.Controllers.APIs
{
    [RoutePrefix("api/v1")]
    [Authorize]
    public class ScheduleController : ApiController
    {
        [HttpGet]
        [Route("schedule/GetEarningByDateRange/{UserID}")]
        public IHttpActionResult GetUserEarning(string UserID, DateTime start, DateTime end)
        {

            return Ok();
        }


        [HttpGet]
        [Route("jobinfo/{CustomerID:int}")]
        public IHttpActionResult GetJobInfo(int CustomerID)
        {
            JobInfo user = Database.GetJobInfoCustomerById(CustomerID);


            return Ok(user);
        }



        [HttpGet]
        [Route("GetContractorInfo/{ContractorID:int}")]
        public IHttpActionResult GetContractorInfo(int ContractorID)
        {
            ContractorStruct contractor = Database.GetContractorByID(ContractorID);
            if (!string.IsNullOrEmpty(contractor.ContractorPic))
            {
                var Request = HttpContext.Current.Request;
                var ImageUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, $"/ContratorPics/{contractor.ContractorPic}");
                contractor.ContractorPic = ImageUrl;
            }

            return Ok(contractor);

        }

        [HttpPost]
        [Route("UpdateContractorInfo/{ContractorID:int}")]
        public IHttpActionResult UpdateContractorInfo(int ContractorID)
        {

            var httpRequest = HttpContext.Current.Request;
            DTO.ContractorInfo contractor = new DTO.ContractorInfo();
            contractor.businessName = httpRequest.Form["businessName"];
            contractor.address = httpRequest.Form["address"];
            contractor.city = httpRequest.Form["city"];
            contractor.state = httpRequest.Form["state"];
            contractor.zip = httpRequest.Form["zip"];

            if (string.IsNullOrEmpty(contractor.address) || string.IsNullOrEmpty(contractor.city) || string.IsNullOrEmpty(contractor.state) || string.IsNullOrEmpty(contractor.zip))
            {
                return BadRequest("Required fields Address, City, State, and Zip");
            }

            DBRow row = new DBRow();
            row.SetValue("businessName", contractor.businessName);
            row.SetValue("address", contractor.address);
            row.SetValue("city", contractor.city);
            row.SetValue("state", contractor.state);
            row.SetValue("zip", contractor.zip);


            if (httpRequest.Files.Count > 0)
            {
                try
                {

                    var postedFile = httpRequest.Files[0];
                    var getExtension = Path.GetExtension(postedFile.FileName);
                    var fileName = Guid.NewGuid().ToString() + getExtension;
                    var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/ContratorPics/"));
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    postedFile.SaveAs(Path.Combine(filePath, fileName));

                    row.SetValue("ContractorPic", fileName);


                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);

                }
            }


            string error = Database.DynamicSetWithKeyInt("Contractors", "contractorID", ref ContractorID, row);
            if (error != null)
                return BadRequest(error);


            return Ok(new { status = "contrator info has updated successfully." });


        }

        [HttpPost]
        [Route("Contractor/{ContractorID:int}/Walkthrough/{IsChecked:bool}")]
        public IHttpActionResult SetContractorWalkthroughStatus(int ContractorID, bool IsChecked)
        {
            if (ContractorID <= 0)
            {
                return BadRequest("Invalid Contractor ID");
            }

            string error = Database.SetContractorWalkthrough(ContractorID, IsChecked);
            if(error != null)
            {
                return BadRequest(error);
            }
            return Ok(new { status = "Contrator walkthrough updated successfully." });
        }



        [HttpGet]
        [Route("Payment/GetPaymentByDateRange")]
        public IHttpActionResult GetPaymentDetails(DateTime startDate, DateTime endDate)
        {

            DateTime mst = Globals.UtcToMst(DateTime.UtcNow);
            if (startDate == DateTime.MinValue) startDate = Globals.SafeDateParse(Globals.GetCookieValue("PaymentsStartDate"));
            if (endDate == DateTime.MinValue) endDate = Globals.SafeDateParse(Globals.GetCookieValue("PaymentsEndDate"));
            if (startDate == DateTime.MinValue) startDate = mst;
            if (endDate == DateTime.MinValue) endDate = mst;

            var claim = System.Security.Claims.ClaimsPrincipal
                .Current.FindFirst("franchiseMask");
            int selectedMask = Convert.ToInt32(claim?.Value);
            int contractorID = Convert.ToInt32(ClaimsPrincipal.Current.FindFirst("contractorID")?.Value);

            List<DBRow> invoiceRangeList = Database.GetInvoiceRangeByDateRange(startDate, endDate);
            Lookup<int, DBRow> invoiceRangeLookup = (Lookup<int, DBRow>)invoiceRangeList.ToLookup(p => p.GetInt("customerID"), p => p);

            SortedList<DateTime, SortedList<int, List<AppStruct>>> dictApp = new SortedList<DateTime, SortedList<int, List<AppStruct>>>();
            List<AppStruct> appList = Database.GetAppsByContractor(selectedMask, contractorID, startDate, endDate, @"A.appointmentDate, A.startTime, A.endTime", false);
            foreach (AppStruct app in appList)
            {
                if (!dictApp.ContainsKey(app.appointmentDate)) dictApp.Add(app.appointmentDate, new SortedList<int, List<AppStruct>>());
                if (!dictApp[app.appointmentDate].ContainsKey(app.customerID)) dictApp[app.appointmentDate].Add(app.customerID, new List<AppStruct>());
                dictApp[app.appointmentDate][app.customerID].Add(app);
            }

            SortedList<DateTime, SortedList<int, List<TransactionStruct>>> dictTrans = new SortedList<DateTime, SortedList<int, List<TransactionStruct>>>();
            List<TransactionStruct> transList = Database.GetTransactions(selectedMask, 0, startDate, endDate, "T.dateCreated");
            foreach (TransactionStruct trans in transList)
            {
                if (trans.auth != 3)
                {
                    if (!dictTrans.ContainsKey(trans.dateApply)) dictTrans.Add(trans.dateApply, new SortedList<int, List<TransactionStruct>>());
                    if (!dictTrans[trans.dateApply].ContainsKey(trans.customerID)) dictTrans[trans.dateApply].Add(trans.customerID, new List<TransactionStruct>());
                    dictTrans[trans.dateApply][trans.customerID].Add(trans);
                }
            }
            List<PaymentDTO> payments = new List<PaymentDTO>();

            foreach (DateTime appDate in dictApp.Keys)
            {
                foreach (int customerID in dictApp[appDate].Keys)
                {
                    bool oldPaid = false;
                    decimal dayAppTotal = 0;
                    decimal dayAppHours = 0;
                    decimal dayAppRate = decimal.MinValue;
                    decimal dayAppServiceFee = 0;
                    decimal dayAppSubContractorCC = 0;
                    decimal dayAppSubContractorWW = 0;
                    decimal dayAppSubContractorHW = 0;
                    decimal dayAppSubContractorCL = 0;
                    decimal dayAppTips = 0;
                    decimal dayAppDiscountAmount = 0;
                    decimal dayAppDiscountPercent = decimal.MinValue;
                    decimal dayAppDiscountReferral = decimal.MinValue;
                    decimal dayAppSalesTax = decimal.MinValue;

                    List<PaymentDetail> paymentDetails = new List<PaymentDetail>();


                    foreach (AppStruct appStruct in dictApp[appDate][customerID])
                    {
                        string status = "";
                        if (appStruct.appStatus == 1) status = " (Rescheduled)";
                        if (appStruct.appStatus == 2) status = " (Canceled)";

                        PaymentDetail paymentDetail = new PaymentDetail();

                        paymentDetail.AppointmentId = appStruct.appointmentID;
                        paymentDetail.ContractorTitle = appStruct.contractorTitle;
                        paymentDetail.ServiceFee = appStruct.customerServiceFee;
                        paymentDetail.SubCon = appStruct.customerSubContractor;
                        paymentDetail.Hours = Globals.FormatHours(appStruct.appType == 1 ? appStruct.customerHours : 0);
                        paymentDetail.HourRate = appStruct.customerRate;
                        paymentDetail.Tips = appStruct.contractorTips;
                        paymentDetail.DiscountAmount = appStruct.customerDiscountAmount;
                        paymentDetail.DiscountPercentage = appStruct.customerDiscountPercent;

                        paymentDetails.Add(paymentDetail);
                        if (appStruct.appStatus == 0)
                        {
                            if (appStruct.paymentFinished) oldPaid = true;
                            if (appStruct.appType == 1) dayAppHours += appStruct.customerHours;
                            dayAppServiceFee += appStruct.customerServiceFee;
                            if (appStruct.appType == 2) dayAppSubContractorCC += appStruct.customerSubContractor;
                            if (appStruct.appType == 3) dayAppSubContractorWW += appStruct.customerSubContractor;
                            if (appStruct.appType == 4) dayAppSubContractorHW += appStruct.customerSubContractor;
                            if (appStruct.appType == 5) dayAppSubContractorCL += appStruct.customerSubContractor;
                            dayAppTips += appStruct.contractorTips;
                            dayAppDiscountAmount += appStruct.customerDiscountAmount;
                            dayAppTotal += Globals.CalculateAppointmentTotal(appStruct);

                            if (dayAppRate != decimal.MaxValue)
                            {
                                if (dayAppRate == decimal.MinValue) dayAppRate = appStruct.customerRate;
                                else if (dayAppRate != appStruct.customerRate) dayAppRate = decimal.MaxValue;
                            }

                            if (dayAppDiscountPercent != decimal.MaxValue)
                            {
                                if (dayAppDiscountPercent == decimal.MinValue) dayAppDiscountPercent = appStruct.customerDiscountPercent;
                                else if (dayAppDiscountPercent != appStruct.customerDiscountPercent) dayAppDiscountPercent = decimal.MaxValue;
                            }

                            if (dayAppDiscountReferral != decimal.MaxValue)
                            {
                                if (dayAppDiscountReferral == decimal.MinValue) dayAppDiscountReferral = appStruct.customerDiscountReferral;
                                else if (dayAppDiscountReferral != appStruct.customerDiscountReferral) dayAppDiscountReferral = decimal.MaxValue;
                            }

                            if (dayAppSalesTax != decimal.MaxValue)
                            {
                                if (dayAppSalesTax == decimal.MinValue) dayAppSalesTax = appStruct.salesTax;
                                else if (dayAppSalesTax != appStruct.salesTax) dayAppSalesTax = decimal.MaxValue;
                            }
                        }
                    }

                    //Fix Service Fee Split Issue
                    dayAppTotal -= (dayAppServiceFee - Math.Round(dayAppServiceFee, 1));
                    dayAppServiceFee = Math.Round(dayAppServiceFee, 1);

                    int dayTransCount = 0;
                    decimal dayTransTotal = 0;
                    decimal dayTransHours = 0;
                    decimal dayTransRate = decimal.MinValue;
                    decimal dayTransServiceFee = 0;
                    decimal dayTransSubContractorCC = 0;
                    decimal dayTransSubContractorWW = 0;
                    decimal dayTransSubContractorHW = 0;
                    decimal dayTransSubContractorCL = 0;
                    decimal dayTransTips = 0;
                    decimal dayTransDiscountAmount = 0;
                    decimal dayTransDiscountPercent = decimal.MinValue;
                    decimal dayTransDiscountReferral = decimal.MinValue;

                    if (dictTrans.ContainsKey(appDate) && dictTrans[appDate].ContainsKey(customerID))
                    {
                        foreach (TransactionStruct transStruct in dictTrans[appDate][customerID])
                        {
                            string text = transStruct.paymentType;
                            string itemClass = "PaymentSaleItem";
                            if (transStruct.auth == 1)
                            {
                                itemClass = "PaymentOpenAuthItem";
                                text = "Open Auth";
                            }
                            if (transStruct.transType == "Return")
                            {
                                itemClass = "PaymentReturnItem";
                            }
                            if (transStruct.transType == "Invoice")
                            {
                                itemClass = "PaymentInvoiceItem";
                                text = "Invoice";
                            }
                            if (transStruct.isVoid)
                            {
                                itemClass = "PaymentVoidItem";
                                text = "Voided";
                            }
                            text += " " + transStruct.transID + " " + transStruct.dateCreated.ToString("d");


                            PaymentDetail paymentDetail = new PaymentDetail();

                            paymentDetail.AppointmentId = transStruct.transID;
                            paymentDetail.ContractorTitle = text;
                            paymentDetail.ServiceFee = transStruct.serviceFee;
                            paymentDetail.SubCon = transStruct.subContractorCC + transStruct.subContractorWW + transStruct.subContractorHW;
                            paymentDetail.Hours = Globals.FormatHours(transStruct.hoursBilled);
                            paymentDetail.HourRate = transStruct.hourlyRate;
                            paymentDetail.Tips = transStruct.tips;
                            paymentDetail.DiscountAmount = transStruct.discountAmount;
                            paymentDetail.DiscountPercentage = transStruct.discountPercent + transStruct.discountReferral;


                            paymentDetails.Add(paymentDetail);

                            if (!transStruct.isVoid && !transStruct.IsAuth())
                            {
                                if (transStruct.transType == "Return")
                                {
                                    dayTransTotal -= transStruct.total;
                                    dayTransHours -= transStruct.hoursBilled;
                                    dayTransServiceFee -= transStruct.serviceFee;
                                    dayTransSubContractorCC -= transStruct.subContractorCC;
                                    dayTransSubContractorWW -= transStruct.subContractorWW;
                                    dayTransSubContractorHW -= transStruct.subContractorHW;
                                    dayTransSubContractorCL -= transStruct.subContractorCL;
                                    dayTransTips -= transStruct.tips;
                                    dayTransDiscountAmount -= transStruct.discountAmount;
                                }
                                if (transStruct.transType == "Sale")
                                {
                                    dayTransTotal += transStruct.total;
                                    dayTransHours += transStruct.hoursBilled;
                                    dayTransServiceFee += transStruct.serviceFee;
                                    dayTransSubContractorCC += transStruct.subContractorCC;
                                    dayTransSubContractorWW += transStruct.subContractorWW;
                                    dayTransSubContractorHW += transStruct.subContractorHW;
                                    dayTransSubContractorCL += transStruct.subContractorCL;
                                    dayTransTips += transStruct.tips;
                                    dayTransDiscountAmount += transStruct.discountAmount;
                                }
                            }

                            dayTransCount++;
                            if (dayTransRate != decimal.MaxValue)
                            {
                                if (dayTransRate == decimal.MinValue) dayTransRate = transStruct.hourlyRate;
                                else if (dayTransRate != transStruct.hourlyRate) dayTransRate = decimal.MaxValue;
                            }

                            if (dayTransDiscountPercent != decimal.MaxValue)
                            {
                                if (dayTransDiscountPercent == decimal.MinValue) dayTransDiscountPercent = transStruct.discountPercent;
                                else if (dayTransDiscountPercent != transStruct.discountPercent) dayTransDiscountPercent = decimal.MaxValue;
                            }

                            if (dayTransDiscountReferral != decimal.MaxValue)
                            {
                                if (dayTransDiscountReferral == decimal.MinValue) dayTransDiscountReferral = transStruct.discountReferral;
                                else if (dayTransDiscountReferral != transStruct.discountReferral) dayTransDiscountReferral = decimal.MaxValue;
                            }
                        }
                    }

                    foreach (var invoiceRange in invoiceRangeLookup[customerID])
                    {
                        if (appDate >= invoiceRange.GetDate("startDate") && appDate <= invoiceRange.GetDate("endDate"))
                        {
                            //itemString += @"<div class=""PaymentInvoiceItem"">Bulk Invoice " + invoiceRange.GetInt("invoiceID") + @"</div>";
                            //serviceFeeString += @"<div class=""PaymentInvoiceItem"">-</div>";
                            //subContractorString += @"<div class=""PaymentInvoiceItem"">-</div>";
                            //hoursString += @"<div class=""PaymentInvoiceItem"">-</div>";
                            //tipsString += @"<div class=""PaymentInvoiceItem"">-</div>";
                            //discountPercentString += @"<div class=""PaymentInvoiceItem"">-</div>";
                            //discountAmountString += @"<div class=""PaymentInvoiceItem"">-</div>";
                        }
                    }

                    decimal balance = decimal.Round(dayAppTotal - dayTransTotal, 2);
                    if (oldPaid) balance = 0;


                    AppStruct firstApp = dictApp[appDate][customerID][0];


                    string rate = (dayAppRate == decimal.MaxValue || dayAppRate == decimal.MinValue) ? "E" : dayAppRate.ToString();
                    if (dayTransRate != decimal.MinValue && dayAppRate != dayTransRate) rate = "E";

                    string discountPercent = (dayAppDiscountPercent == decimal.MaxValue || dayAppDiscountPercent == decimal.MinValue) ? "E" : dayAppDiscountPercent.ToString();
                    //if (dayTransDiscountPercent != decimal.MinValue && dayAppDiscountPercent != dayTransDiscountPercent) discountPercent = "E"; //THIS MAY HAVE BROKEN THINGS

                    string discountReferral = (dayAppDiscountReferral == decimal.MaxValue || dayAppDiscountReferral == decimal.MinValue) ? "E" : dayAppDiscountReferral.ToString();
                    if (dayTransDiscountReferral != decimal.MinValue && dayAppDiscountReferral != dayTransDiscountReferral) discountReferral = "E";

                    string salesTax = (dayAppSalesTax == decimal.MaxValue || dayAppSalesTax == decimal.MinValue) ? "E" : dayAppSalesTax.ToString();

                    string chargeQuery = Globals.BuildQueryString("Transaction.aspx", "custID", customerID);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "transDate", appDate.ToString("d"));
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "hoursBilled", dayAppHours - dayTransHours);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "hourlyRate", rate);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "serviceFee", dayAppServiceFee - dayTransServiceFee);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "subCC", dayAppSubContractorCC - dayTransSubContractorCC);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "subWW", dayAppSubContractorWW - dayTransSubContractorWW);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "subHW", dayAppSubContractorHW - dayTransSubContractorHW);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "subCL", dayAppSubContractorCL - dayTransSubContractorCL);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "discountAmount", dayAppDiscountAmount - dayTransDiscountAmount);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "discountPercent", discountPercent);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "discountReferral", discountReferral);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "salesTax", salesTax);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "tips", dayAppTips - dayTransTips);
                    chargeQuery = Globals.BuildQueryString(chargeQuery, "total", dayAppTotal - dayTransTotal);

                    string chargeLink = @"<a class=""paymentButton"" href=""" + Globals.BuildQueryString(chargeQuery, "transType", "Sale") + @""">Pay</a>";
                    string invoiceLink = @"<a class=""paymentButton"" href=""" + Globals.BuildQueryString(chargeQuery, "transType", "Invoice") + @""">Invoice</a>";

                    rate = (dayTransRate == decimal.MaxValue || dayTransRate == decimal.MinValue) ? "E" : dayTransRate.ToString();
                    discountPercent = (dayTransDiscountPercent == decimal.MaxValue || dayTransDiscountPercent == decimal.MinValue) ? "E" : dayTransDiscountPercent.ToString();
                    discountReferral = (dayTransDiscountReferral == decimal.MaxValue || dayTransDiscountReferral == decimal.MinValue) ? "E" : dayTransDiscountReferral.ToString();

                    string returnQuery = Globals.BuildQueryString("Transaction.aspx", "custID", customerID);
                    if (balance >= 0)
                    {
                        returnQuery = Globals.BuildQueryString(returnQuery, "transType", "Return");
                        returnQuery = Globals.BuildQueryString(returnQuery, "transDate", appDate.ToString("d"));
                        returnQuery = Globals.BuildQueryString(returnQuery, "hoursBilled", dayTransHours);
                        returnQuery = Globals.BuildQueryString(returnQuery, "hourlyRate", rate);
                        returnQuery = Globals.BuildQueryString(returnQuery, "serviceFee", dayTransServiceFee);
                        returnQuery = Globals.BuildQueryString(returnQuery, "subCC", dayTransSubContractorCC);
                        returnQuery = Globals.BuildQueryString(returnQuery, "subWW", dayTransSubContractorWW);
                        returnQuery = Globals.BuildQueryString(returnQuery, "subHW", dayTransSubContractorHW);
                        returnQuery = Globals.BuildQueryString(returnQuery, "subCL", dayTransSubContractorCL);
                        returnQuery = Globals.BuildQueryString(returnQuery, "discountAmount", dayTransDiscountAmount);
                        returnQuery = Globals.BuildQueryString(returnQuery, "discountPercent", discountPercent);
                        returnQuery = Globals.BuildQueryString(returnQuery, "discountReferral", discountReferral);
                        returnQuery = Globals.BuildQueryString(returnQuery, "salesTax", salesTax);
                        returnQuery = Globals.BuildQueryString(returnQuery, "tips", dayTransTips);
                        returnQuery = Globals.BuildQueryString(returnQuery, "total", dayTransTotal);
                    }
                    else
                    {
                        returnQuery = Globals.BuildQueryString(returnQuery, "transType", "Return");
                        returnQuery = Globals.BuildQueryString(returnQuery, "transDate", appDate.ToString("d"));
                        returnQuery = Globals.BuildQueryString(returnQuery, "hoursBilled", dayTransHours - dayAppHours);
                        returnQuery = Globals.BuildQueryString(returnQuery, "hourlyRate", rate);
                        returnQuery = Globals.BuildQueryString(returnQuery, "serviceFee", dayTransServiceFee - dayAppServiceFee);
                        returnQuery = Globals.BuildQueryString(returnQuery, "subCC", dayTransSubContractorCC - dayAppSubContractorCC);
                        returnQuery = Globals.BuildQueryString(returnQuery, "subWW", dayTransSubContractorCC - dayAppSubContractorCC);
                        returnQuery = Globals.BuildQueryString(returnQuery, "subHW", dayTransSubContractorCC - dayAppSubContractorCC);
                        returnQuery = Globals.BuildQueryString(returnQuery, "subCL", dayTransSubContractorCC - dayAppSubContractorCC);
                        returnQuery = Globals.BuildQueryString(returnQuery, "discountAmount", dayTransDiscountAmount - dayAppDiscountAmount);
                        returnQuery = Globals.BuildQueryString(returnQuery, "discountPercent", discountPercent);
                        returnQuery = Globals.BuildQueryString(returnQuery, "discountReferral", discountReferral);
                        returnQuery = Globals.BuildQueryString(returnQuery, "salesTax", salesTax);
                        returnQuery = Globals.BuildQueryString(returnQuery, "tips", dayTransTips - dayAppTips);
                        returnQuery = Globals.BuildQueryString(returnQuery, "total", dayTransTotal - dayAppTotal);
                    }

                    string returnLink = @"<a class=""paymentButton"" href=""" + returnQuery + @""">Return</a>";

                    if (balance == 0) chargeLink = "";
                    if (dayTransCount == 0) returnLink = "";
                    if (firstApp.customerPaymentType.ToLower().Contains("invoice"))
                        returnLink = "";

                    string paymentType = firstApp.customerPaymentType;
                    if (Globals.IsPaymentCreditCard(paymentType))
                    {
                        int year = Globals.SafeIntParse(firstApp.customerCardExpYear);
                        int month = Globals.SafeIntParse(firstApp.customerCardExpMonth);

                        if (year == 0 || month == 0) paymentType = "Bad Card";
                        else if (year <= mst.Year && (year != mst.Year || month < mst.Month)) paymentType = "Expired Card";
                    }
                    PaymentDTO payment = new PaymentDTO();
                    payment.Date = appDate;
                    payment.Customer = firstApp.customerTitleCustomNote;
                    payment.Details = paymentDetails;
                    payment.Total = dayAppTotal;
                    payment.Balance = balance == 0 ? "Paid" : Globals.FormatMoney(balance);
                    payment.PymentType = paymentType;
                    payment.AveragePayRate = (decimal)dayAppTotal / (decimal)paymentDetails.Sum(x => Convert.ToDecimal(x.Hours));
                    payments.Add(payment);
                }

            }
            return Ok(payments);

        }

        [HttpGet]
        [Route("Schedule/GetUnavailibility")]
        public IHttpActionResult GetUnavailablity(DateTime fromDate, DateTime ToDate)
        {
            var claim = System.Security.Claims.ClaimsPrincipal.Current;
            var franchiseMask = Convert.ToInt32(claim.FindFirst("franchiseMask")?.Value);
            var contractorID = Convert.ToInt32(claim.FindFirst("contractorID")?.Value);
            ContractorStruct contractor = Database.GetContractorByID(contractorID);

            var unavailableID = 0;
            UnavailableStruct unavailable;

            var data = Database.GetUnavailableByDateRange(franchiseMask, contractor.contractorType, contractorID, fromDate, ToDate, "dateCreated");

            return Ok(data);
        }


        [HttpPost]
        [Route("Schedule/AddUnavailibility")]
        public IHttpActionResult AddUnavailablity(UnavailableDTO unavailableDTO)
        {
            var claim = System.Security.Claims.ClaimsPrincipal.Current;
            var franchiseMask = Convert.ToInt32(claim.FindFirst("franchiseMask")?.Value);
            var contractorID = Convert.ToInt32(claim.FindFirst("contractorID")?.Value);
            ContractorStruct contractor = Database.GetContractorByID(contractorID);



            UnavailableStruct unavailable = new UnavailableStruct();
            unavailable.unavailableID = unavailableDTO.unavailableID;
            unavailable.contractorID = contractorID;
            unavailable.contractorTitle = contractor.title;
            unavailable.dateCreated = DateTime.Now;
            unavailable.dateRequested = unavailableDTO.dateRequested;
            unavailable.startTime = unavailableDTO.startTime;
            unavailable.endTime = unavailableDTO.endTime;
            unavailable.recurrenceID = unavailableDTO.recurrenceID;
            unavailable.recurrenceType = unavailableDTO.recurrenceType;

            var data = Database.SetUnavailable(unavailable);
            if (data == null)
            {
                return Ok("Recored updated successfully");

            }
            return BadRequest(data);
        }


        [HttpDelete]
        [Route("Schedule/DeleteUnavailibility/{UnAvailbeId:int}")]
        public IHttpActionResult DeleteUnavailablity(int UnAvailbeId)
        {
            var data = Database.DeleteUnavailable(UnAvailbeId);
            if (data == null)
            {
                return Ok("Record deleted successfully.");
            }

            return BadRequest(data);
        }

        [HttpGet]
        [Route("Schedule/GetSchedule")]
        public IHttpActionResult LoadSchedule(int ContractorID, DateTime startDate, DateTime endDate)
        {

            var claims = System.Security.Claims.ClaimsPrincipal.Current;
            int userAccess = Convert.ToInt32(claims.FindFirst("access")?.Value);
            int userContractorID = Convert.ToInt32(claims.FindFirst("contractorID")?.Value);
            int franchiseMask = Convert.ToInt32(claims.FindFirst("franchiseMask")?.Value);

            if (ContractorID > 0)
            {
                userContractorID = ContractorID;
            }
            ContractorStruct contractor = Database.GetContractorByID(userContractorID);


            int contractorSubType = contractor.contractorType;
            int contractorMaskType = userAccess == 2 ? -1 : 1 << (contractorSubType - 1);


            List<ContractorStruct> contractors = new List<ContractorStruct>();
            {
                List<ContractorStruct> teams = new List<ContractorStruct>();
                List<ContractorStruct> nonTeams = new List<ContractorStruct>();
                foreach (ContractorStruct cont in Database.GetContractorList(franchiseMask, contractorMaskType, false, false, false, false, "team, firstName, lastName"))
                {

                    if (string.IsNullOrEmpty(cont.team)) nonTeams.Add(cont);
                    else teams.Add(cont);
                }
                contractors.AddRange(teams);
                contractors.AddRange(nonTeams);
            }

            List<ScheduleDTO> scheduleDTOs = new List<ScheduleDTO>();
            var appointments = Database.GetAppsByContractor(franchiseMask, userContractorID, startDate, endDate, "A.appointmentDate, A.startTime, A.endTime", true);

            List<string> routeData = new List<string>();
            string lastRouteAddr = Globals.CleanAddr(contractor.address) + "," + Globals.CleanAddr(contractor.city) + "," + Globals.CleanAddr(contractor.state) + "," + Globals.CleanAddr(contractor.zip);
            routeData.Add(lastRouteAddr);


            foreach (var item in appointments)
            {

                DrivingRoute route = new DrivingRoute();
                route.travelTime = 1800;

                decimal hours = (decimal)(item.endTime - item.startTime).TotalMinutes / 60;
                if (hours < 0) hours = 0;

                string routeAddr = Globals.CleanAddr(item.customerAddress) + "," + Globals.CleanAddr(item.customerCity) + "," + Globals.CleanAddr(item.customerState) + "," + Globals.CleanAddr(item.customerZip);
                route = GoogleMaps.GetDrivingRoute(lastRouteAddr, routeAddr);
                if (route.travelTime <= 0 && lastRouteAddr != routeAddr) route.travelTime = 1800;
                string routeLink = GoogleMaps.GetDrivingLink(new string[] { lastRouteAddr, routeAddr });
                lastRouteAddr = routeAddr;
                routeData.Add(routeAddr);
                Debug.WriteLine("Distance: " + route.distance + ", Time: " + TimeSpan.FromSeconds((double)route.travelTime));


                CustomerStruct customer = new CustomerStruct();
                var customerStruct = Database.GetCustomerByID(franchiseMask, item.customerID, out customer);


                ScheduleDTO schedule = new ScheduleDTO();
                schedule.AppointmentId = item.appointmentID;
                schedule.CustomerName = item.customerTitleCustomNote;
                schedule.CustomerCity = item.customerCity;
                schedule.ScheduleDate = item.appointmentDate.Date;
                schedule.startTime = item.startTime.ToString("t");
                schedule.endTime = item.endTime.ToString("t");
                schedule.Hours = item.customerHours;
                schedule.Miles = route.distance.ToString("N1") + @" mi ";
                schedule.Minutes = Math.Round(TimeSpan.FromSeconds((double)route.travelTime).TotalMinutes);
                schedule.Keys = customer.NC_RequiresKeys;
                schedule.AproxPay = (item.customerRate * item.customerHours);
                schedule.JobCompleted = item.JobCompleted;
                schedule.alternatePhoneOne = customer.alternatePhoneOne;
                schedule.bestPhone = customer.bestPhone;
                schedule.bestPhoneCell = customer.bestPhoneCell;
                schedule.alternatePhoneTwo = customer.alternatePhoneTwo;
                schedule.alternatePhoneTwoCell = customer.alternatePhoneTwoCell;
                schedule.alternatePhoneOneCell = customer.alternatePhoneOneCell;
                schedule.locationAddress = customer.locationAddress;
                schedule.locationCity = customer.locationCity;
                schedule.locationState = customer.locationState;
                schedule.locationZip = customer.locationZip;
                schedule.CustomerId = item.customerID;
                schedule.paymentType = customer.paymentType;
                schedule.TakePic = customer.TakePic;
                schedule.Bathrooms = customer.NC_Bathrooms;
                schedule.Bedrooms = customer.NC_Bedrooms;
                schedule.SquareFootage = customer.NC_SquareFootage;
                schedule.NewlyConstructed = customer.newBuilding;
                schedule.NC_Vacuum = customer.NC_Vacuum;
                schedule.NC_DoDishes = customer.NC_DoDishes;
                schedule.NC_ChangeBed = customer.NC_ChangeBed;
                schedule.PetsCount = customer.NC_Pets;
                schedule.NC_FlooringCarpet = customer.NC_FlooringCarpet;
                schedule.NC_FlooringHardwood = customer.NC_FlooringHardwood;
                schedule.NC_FlooringTile = customer.NC_FlooringTile;
                schedule.NC_FlooringLinoleum = customer.NC_FlooringLinoleum;
                schedule.NC_FlooringSlate = customer.NC_FlooringSlate;
                schedule.NC_FlooringMarble = customer.NC_FlooringMarble;
                schedule.HowtoEnterHome = customer.NC_EnterHome;
                schedule.NC_Organize = customer.NC_Organize;
                schedule.CleanRating = customer.NC_CleanRating;
                schedule.NC_CleaningType = customer.NC_CleaningType;
                schedule.GateCode = customer.NC_GateCode;
                schedule.GarageCode = customer.NC_GarageCode;
                schedule.NC_DoorCode = customer.NC_DoorCode;
                schedule.NC_RequestEcoCleaners = customer.NC_RequestEcoCleaners;
                schedule.DC_Blinds = customer.DC_Blinds;
                schedule.DC_BlindsAmount = customer.DC_BlindsAmount;
                schedule.DC_BlindsCondition = customer.DC_BlindsCondition;
                schedule.DC_Windows = customer.DC_Windows;
                schedule.DC_WindowsAmount = customer.DC_WindowsAmount;
                schedule.DC_WindowsSills = customer.DC_WindowsSills;
                schedule.DC_Walls = customer.DC_Walls;
                schedule.DC_WallsDetail = customer.DC_WallsDetail;
                schedule.DC_Baseboards = customer.DC_Baseboards;
                schedule.DC_DoorFrames = customer.DC_DoorFrames;
                schedule.DC_LightSwitches = customer.DC_LightSwitches;
                schedule.DC_VentCovers = customer.DC_VentCovers;
                schedule.DC_InsideVents = customer.DC_InsideVents;
                schedule.DC_Pantry = customer.DC_Pantry;
                schedule.DC_LaundryRoom = customer.DC_LaundryRoom;
                schedule.DC_CeilingFans = customer.DC_CeilingFans;
                schedule.DC_CeilingFansAmount = customer.DC_CeilingFansAmount;
                schedule.DC_LightFixtures = customer.DC_LightFixtures;
                schedule.DC_KitchenCuboards = customer.DC_KitchenCuboards;
                schedule.DC_KitchenCuboardsDetail = customer.DC_KitchenCuboardsDetail;
                schedule.DC_BathroomCuboards = customer.DC_BathroomCuboards;
                schedule.DC_BathroomCuboardsDetail = customer.DC_BathroomCuboardsDetail;
                schedule.DC_Oven = customer.DC_Oven;
                schedule.DC_Refrigerator = customer.DC_Refrigerator;
                schedule.DC_OtherOne = customer.DC_OtherOne;
                schedule.DC_OtherTwo = customer.DC_OtherTwo;
                schedule.Details = customer.NC_Details;
                schedule.JobCompleted = item.JobCompleted;
				schedule.CustomerServiceFee = item.customerServiceFee;
				schedule.ContractorTips = item.contractorTips;
                schedule.customerRate = item.customerRate;
                schedule.NC_RequiresKeys = customer.NC_RequiresKeys;
                schedule.jobStartTime = item.jobStartTime;
                schedule.jobEndTime = item.jobEndTime;
                schedule.Duration = item.Duration;
                schedule.pauseTime = item.pauseTime;
                schedule.RelatedAppointments = item.RelatedAppointments;
                schedule.lastStartTime = item.lastStartTime;
                scheduleDTOs.Add(schedule);
            }


            return Ok(scheduleDTOs);
        }


        [HttpGet]
        [Route("schedule/GetAppointment/{appId:int}")]
        public IHttpActionResult GetAppointment(int appId)
        {
            var claims = System.Security.Claims.ClaimsPrincipal.Current;
            int userAccess = Convert.ToInt32(claims.FindFirst("access")?.Value);
            int userContractorID = Convert.ToInt32(claims.FindFirst("contractorID")?.Value);
            int franchiseMask = Convert.ToInt32(claims.FindFirst("franchiseMask")?.Value);

            ContractorStruct contractor = Database.GetContractorByID(userContractorID);

            int contractorSubType = contractor.contractorType;

            List<ScheduleDTO> scheduleDTOs = new List<ScheduleDTO>();
            AppStruct item;
            Database.GetApointmentpByID(franchiseMask, appId, out item);

            List<string> routeData = new List<string>();
            string lastRouteAddr = Globals.CleanAddr(contractor.address) + "," + Globals.CleanAddr(contractor.city) + "," + Globals.CleanAddr(contractor.state) + "," + Globals.CleanAddr(contractor.zip);
            routeData.Add(lastRouteAddr);


            DrivingRoute route = new DrivingRoute();
            route.travelTime = 1800;

            decimal hours = (decimal)(item.endTime - item.startTime).TotalMinutes / 60;
            if (hours < 0) hours = 0;

            string routeAddr = Globals.CleanAddr(item.customerAddress) + "," + Globals.CleanAddr(item.customerCity) + "," + Globals.CleanAddr(item.customerState) + "," + Globals.CleanAddr(item.customerZip);
            route = GoogleMaps.GetDrivingRoute(lastRouteAddr, routeAddr);
            if (route.travelTime <= 0 && lastRouteAddr != routeAddr) route.travelTime = 1800;
            string routeLink = GoogleMaps.GetDrivingLink(new string[] { lastRouteAddr, routeAddr });
            lastRouteAddr = routeAddr;
            routeData.Add(routeAddr);
            Debug.WriteLine("Distance: " + route.distance + ", Time: " + TimeSpan.FromSeconds((double)route.travelTime));

            CustomerStruct customer = new CustomerStruct();
            var customerStruct = Database.GetCustomerByID(franchiseMask, item.customerID, out customer);



            ScheduleDTO schedule = new ScheduleDTO();
            schedule.AppointmentId = item.appointmentID;
            schedule.CustomerName = item.customerTitleCustomNote;
            schedule.CustomerCity = item.customerCity;
            schedule.ScheduleDate = item.appointmentDate.Date;
            schedule.startTime = item.startTime.ToString("t");
            schedule.endTime = item.endTime.ToString("t");
            schedule.Hours = item.customerHours;
            schedule.Miles = route.distance.ToString("N1") + @" mi ";
            schedule.Minutes = Math.Round(TimeSpan.FromSeconds((double)route.travelTime).TotalMinutes);
            schedule.Keys = customer.NC_RequiresKeys;
            schedule.AproxPay = (item.customerRate * item.customerHours);
            schedule.CustomerId = item.customerID;
            schedule.alternatePhoneOne = customer.alternatePhoneOne;
            schedule.bestPhone = customer.bestPhone;
            schedule.bestPhoneCell = customer.bestPhoneCell;
            schedule.alternatePhoneTwo = customer.alternatePhoneTwo;
            schedule.alternatePhoneTwoCell = customer.alternatePhoneTwoCell;
            schedule.alternatePhoneOneCell = customer.alternatePhoneOneCell;
            schedule.locationAddress = customer.locationAddress;
            schedule.locationCity = customer.locationCity;
            schedule.locationState = customer.locationState;
            schedule.locationZip = customer.locationZip;
            schedule.paymentType = customer.paymentType;
            schedule.TakePic = customer.TakePic;
            schedule.Bathrooms = customer.NC_Bathrooms;
            schedule.Bedrooms = customer.NC_Bedrooms;
            schedule.SquareFootage = customer.NC_SquareFootage;
            schedule.NewlyConstructed = customer.newBuilding;
            schedule.NC_Vacuum = customer.NC_Vacuum;
            schedule.NC_DoDishes = customer.NC_DoDishes;
            schedule.NC_ChangeBed = customer.NC_ChangeBed;
            schedule.PetsCount = customer.NC_Pets;
            schedule.NC_FlooringCarpet = customer.NC_FlooringCarpet;
            schedule.NC_FlooringHardwood = customer.NC_FlooringHardwood;
            schedule.NC_FlooringTile = customer.NC_FlooringTile;
            schedule.NC_FlooringLinoleum = customer.NC_FlooringLinoleum;
            schedule.NC_FlooringSlate = customer.NC_FlooringSlate;
            schedule.NC_FlooringMarble = customer.NC_FlooringMarble;
            schedule.HowtoEnterHome = customer.NC_EnterHome;
            schedule.NC_Organize = customer.NC_Organize;
            schedule.CleanRating = customer.NC_CleanRating;
            schedule.NC_CleaningType = customer.NC_CleaningType;
            schedule.GateCode = customer.NC_GateCode;
            schedule.GarageCode = customer.NC_GarageCode;
            schedule.NC_DoorCode = customer.NC_DoorCode;
            schedule.NC_RequestEcoCleaners = customer.NC_RequestEcoCleaners;
            schedule.DC_Blinds = customer.DC_Blinds;
            schedule.DC_BlindsAmount = customer.DC_BlindsAmount;
            schedule.DC_BlindsCondition = customer.DC_BlindsCondition;
            schedule.DC_Windows = customer.DC_Windows;
            schedule.DC_WindowsAmount = customer.DC_WindowsAmount;
            schedule.DC_WindowsSills = customer.DC_WindowsSills;
            schedule.DC_Walls = customer.DC_Walls;
            schedule.DC_WallsDetail = customer.DC_WallsDetail;
            schedule.DC_Baseboards = customer.DC_Baseboards;
            schedule.DC_DoorFrames = customer.DC_DoorFrames;
            schedule.DC_LightSwitches = customer.DC_LightSwitches;
            schedule.DC_VentCovers = customer.DC_VentCovers;
            schedule.DC_InsideVents = customer.DC_InsideVents;
            schedule.DC_Pantry = customer.DC_Pantry;
            schedule.DC_LaundryRoom = customer.DC_LaundryRoom;
            schedule.DC_CeilingFans = customer.DC_CeilingFans;
            schedule.DC_CeilingFansAmount = customer.DC_CeilingFansAmount;
            schedule.DC_LightFixtures = customer.DC_LightFixtures;
            schedule.DC_KitchenCuboards = customer.DC_KitchenCuboards;
            schedule.DC_KitchenCuboardsDetail = customer.DC_KitchenCuboardsDetail;
            schedule.DC_BathroomCuboards = customer.DC_BathroomCuboards;
            schedule.DC_BathroomCuboardsDetail = customer.DC_BathroomCuboardsDetail;
            schedule.DC_Oven = customer.DC_Oven;
            schedule.DC_Refrigerator = customer.DC_Refrigerator;
            schedule.DC_OtherOne = customer.DC_OtherOne;
            schedule.DC_OtherTwo = customer.DC_OtherTwo;
            schedule.Details = customer.NC_Details;
            schedule.JobCompleted = item.JobCompleted;
            schedule.CustomerServiceFee = item.customerServiceFee;
            schedule.ContractorTips = item.contractorTips;
			schedule.customerRate = item.customerRate;
            schedule.NC_RequiresKeys = customer.NC_RequiresKeys;
            schedule.jobStartTime = item.jobStartTime;
            schedule.jobEndTime = item.jobEndTime;
            schedule.Duration = item.Duration;
            schedule.pauseTime = item.pauseTime;
            schedule.RelatedAppointments = item.RelatedAppointments;
            schedule.lastStartTime = item.lastStartTime;

            var rows = Database.GetPartnersByAppointmentIDs(item.RelatedAppointments);
            if (rows != null)
            {
                schedule.Partners = new List<PartnerDTO>();
                foreach (var p in rows)
                {
                    PartnerDTO partner = new PartnerDTO();
                    partner.Firstname = p.GetString("firstName");
                    partner.Lastname = p.GetString("lastName");
                    partner.BusinessName = p.GetString("businessName");
                    partner.PhoneNumber = p.GetString("bestPhone");
                    partner.AlternatePhone = p.GetString("alternatePhone");
                    partner.Email = p.GetString("email");
                    schedule.Partners.Add(partner);
                }
            }


            return Ok(schedule);

        }

        [HttpPost]
        [Route("schedule/UpdateAppointment/{appId:int}")]
        public IHttpActionResult Upload(int appId)
        {

            if (!Request.Content.IsMimeMultipartContent())
                return BadRequest("Unsupported media type");

            var httpRequest = HttpContext.Current.Request;
            var files = httpRequest.Files;
            var note = httpRequest.Form["notes"];


            var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/ContratorPics/"));
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            try
            {

                List<string> images = new List<string>();
                for (int file = 0; file < files.Count; file++)
                {

                    var postedFile = httpRequest.Files[file];
                    var getExtension = Path.GetExtension(postedFile.FileName);
                    var fileName = Guid.NewGuid().ToString() + getExtension;

                    postedFile.SaveAs(Path.Combine(filePath, fileName));

                    images.Add(fileName);
                }


                var response = Database.UpdateAppointmentFromAPI(appId, images, note);
                return Ok(new { Message = "Files uploaded successfully", Note = note });


            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost]
        [Route("schedule/StartJob/{appId:int}")]
        public IHttpActionResult StartJobByAppId(int appId)
        {
            var error = Database.StartJobByAppId(appId);
            if (error == null)
            {
                return Ok("Job Started successfully!");
            }

            return BadRequest(error);
        }
        [HttpPost]
        [Route("schedule/EndJob/{appId:int}")]
        public IHttpActionResult EndJobByAppId(int appId)
        {
            var error = Database.EndJobByAppId(appId);
            if (error == null)
            {
                return Ok("Job Ended successfully!");
            }

            return BadRequest(error);
        }


        [HttpPost]
        [Route("schedule/UpdateCoordinates/{appId:int}")]
        public IHttpActionResult EndJobByAppId(int appId, Location location)
        {
            var error = Database.JobCoordinatesByAppId(appId, location.latitude, location.longitude);
            if (error == null)
            {
                return Ok("Job location updated successfully!");
            }

            return BadRequest(error);
        }

        [HttpPost]
        [Route("Schedule/Appointment/UpdateDurationTime/{appId:int}")]
        public IHttpActionResult UpdateAppointmentDurationTime(int appId, DurationTime durationTime)
        {
            var error = Database.UpdateDurationTimeByAppId(appId, durationTime.LastStartTime, durationTime.PauseTime, durationTime.Duration);
            if (error == null)
            {
                return Ok("Appointment's Duration and Time updated successfully!");
            }

            return BadRequest(error);
        }


        [HttpGet]
        [Route("schedule/GetJobLogs")]
        public IHttpActionResult GetJobLogs(int AppointmentId, int contractorID, bool isGeneral)
        {

            var data = Database.GetJobLogs(AppointmentId, contractorID, isGeneral);
            return Ok(data);
        }
        
        [HttpPost]
        [Route("schedule/UpdateJobLog")]
        public IHttpActionResult GetJobLogs(JobLogsStruct job)
        {
            var data = Database.updateJobLog(job);
            if (data == null)
            return Ok("Record updated successfully.");

            return BadRequest(data);
        }


        public class Location
        {
            public string latitude { get; set; }
            public string longitude { get; set; }
        }

        public class DurationTime
        {
            public DateTime? LastStartTime { get; set; }
            public int Duration { get; set; }
            public DateTime? PauseTime { get; set; }
        }
    }
}
