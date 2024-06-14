using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nexus
{
    public enum TransDocType { Receipt, Invoice, Return };

    public struct TransDocService
    {
        public DateTime date;
        public string description;
        public decimal hours;
        public decimal rate;
        public decimal serviceFee;
        public decimal discount;
        public decimal amount;
    }

    public struct TransDocItem
    {
        public DateTime date;
        public string description;
        public string quantity;
        public string rate;
        public decimal amount;
    }

    public struct TransDocPayment
    {
        public DateTime date;
        public string description;
        public decimal amount;
    }

    public class TransDoc
    {
        public FranchiseStruct franchise;
        public CustomerStruct customer;
        public bool isVoid = false;
        public string transNumber = null;
        public string memo = null;
        public decimal total = decimal.MinValue;

        public string emailAddress = null;
        public string emailSubject = null;

        public List<TransDocService> serviceList = new List<TransDocService>();
        public List<TransDocItem> itemList = new List<TransDocItem>();
        public List<TransDocPayment> paymentList = new List<TransDocPayment>();

        public TransDoc(int franchiseMask, int customerID, bool isVoid = false, string transNumber = null, string memo = null)
        {
            if (customerID > 0) Database.GetCustomerByID(franchiseMask, customerID, out this.customer);
            if (customer.customerID > 0) this.franchise = Globals.GetFranchiseByMask(customer.franchiseMask);
            this.isVoid = isVoid;
            this.transNumber = transNumber;
            this.memo = memo;
            this.emailAddress = customer.email;
        }

        public void AddService(DateTime date, string description, decimal hours, decimal rate, decimal serviceFee, decimal discount, decimal amount)
        {
            TransDocService service = new TransDocService();
            service.date = date;
            service.description = description;
            service.hours = hours;
            service.rate = rate;
            service.serviceFee = serviceFee;
            service.discount = discount;
            service.amount = amount;
            serviceList.Add(service);
        }

        public void AddItem(DateTime date, string description, string quantity, string price, decimal amount)
        {
            TransDocItem item = new TransDocItem();
            item.date = date;
            item.description = description;
            item.quantity = quantity;
            item.rate = price;
            item.amount = amount;
            itemList.Add(item);
        }

        public void AddPayment(DateTime date, string description, decimal amount)
        {
            TransDocPayment payment = new TransDocPayment();
            payment.date = date;
            payment.description = description;
            payment.amount = amount;
            paymentList.Add(payment);
        }

        public string GetHTML()
        {
            try
            {
                if (customer.customerID <= 0 || franchise.franchiseID <= 0)
                {
                    return "Invalid Customer Lookup";
                }

                DateTime mst = Globals.UtcToMst(DateTime.UtcNow);

                TransDocType docType = total < 0 ? TransDocType.Return : TransDocType.Receipt;
                if (paymentList.Count == 0) docType = TransDocType.Invoice;

                string html = @"
                <table style=""width: 100%; text-align: left; border-collapse: collapse;"">
                    <tr>
                        <td>
                            <img src=""" + Globals.baseUrl + @"2LG_Logo_Small.jpg"" alt=""none"" />
                        </td>
                        <td style=""text-align: right;"">
                            <span style=""font-size:130%; font-weight:bold;"">2 Local Gals</span><br />
                            " + franchise.address + @"<br />
                            " + franchise.city + @", " + franchise.state + @" " + franchise.zip + @"<br />
                            Phone: " + franchise.phone + @"<br>
                            <a href=""" + franchise.webLink + @""">" + franchise.webLink + @"</a><br />
                        </td>
                    </tr>  
                </table>
                <div style=""width: 100%; margin:25px auto 5px auto; text-align: right; font-size: 180%; font-weight: bold;"">" + (isVoid ? "(VOID) " : "") + docType.ToString() + @" " + (transNumber ?? "") + @"</div>
                <table style=""width: 100%; text-align: left; border-collapse: collapse;"">
                    <tr>
                        <td style=""width: 110px; padding: 5px; border: 1px solid black; border-right: none; vertical-align: top; font-size:70%; font-weight:bold;"">NAME</td>
                        <td style=""padding: 5px; border: 1px solid black; border-left: none;"">" + Globals.FormatFullName(customer.firstName, customer.lastName, "") + @"</td>
                        <td style=""width: 110px; padding: 5px; border: 1px solid black; border-right: none; vertical-align: top; font-size:70%; font-weight:bold;"">DATE</td>
                        <td style=""padding: 5px; border: 1px solid black; border-left: none;"">" + mst.ToString("d") + @"</td>
                    </tr>
                    <tr>
                        <td style=""width: 110px; padding: 5px; border: 1px solid black; border-right: none; vertical-align: top; font-size:70%; font-weight:bold;"">ADDRESS</td>
                        <td style=""padding: 5px; border: 1px solid black; border-left: none;"">" + Globals.CleanAddr(customer.locationAddress) + @"</td>
                        <td style=""width: 110px; padding: 5px; border: 1px solid black; border-right: none; vertical-align: top; font-size:70%; font-weight:bold;"">CITY</td>
                        <td style=""padding: 5px; border: 1px solid black; border-left: none;"">" + customer.locationCity + ", " + customer.locationState + " " + customer.locationZip + @"</td>
                    </tr>
                </table>";
                if (!customer.billingSame)
                {
                    html += @"
                    <table style=""width: 100%; margin-top: 20px; text-align: left; border-collapse: collapse;"">
                        <tr>
                            <td style=""width: 110px; padding: 5px; border: 1px solid black; border-right: none; vertical-align: top; font-size:70%; font-weight:bold;"">BILLING NAME</td>
                            <td colspan=""3"" style=""padding: 5px; border: 1px solid black; border-left: none;"">" + customer.billingName + @"</td>
                        </tr>
                        <tr>
                            <td style=""width: 110px; padding: 5px; border: 1px solid black; border-right: none; vertical-align: top; font-size:70%; font-weight:bold;"">BILLING ADDRESS</td>
                            <td style=""padding: 5px; border: 1px solid black; border-left: none;"">" + Globals.CleanAddr(customer.billingAddress) + @"</td>
                            <td style=""width: 110px; padding: 5px; border: 1px solid black; border-right: none; vertical-align: top; font-size:70%; font-weight:bold;"">BILLING CITY</td>
                            <td style=""padding: 5px; border: 1px solid black; border-left: none;"">" + customer.billingCity + ", " + customer.billingState + " " + customer.billingZip + @"</td>
                        </tr>
                    </table>";
                }

                if (customer.points > 0)
                {
                    html += @"
                    <table style=""width: 100%; margin-top: 20px; text-align: left; border-collapse: collapse;"">
                        <tr>
                            <td style=""width: 110px; padding: 5px; border: 1px solid black; border-right: none; vertical-align: top; font-size:70%; font-weight:bold;"">REWARD POINTS</td>
                            <td colspan=""3"" style=""padding: 5px; border: 1px solid black; border-left: none;"">" + Globals.FormatPoints(customer.points, customer.ratePerHour) + @"</td>
                        </tr>
                    </table>";
                }

                if (serviceList.Count > 0)
                {
                    html += @"
                    <table style=""width: 100%; margin-top: 20px; text-align: left; border-collapse: collapse;"">
                        <tr style=""background-color: #DDD; text-align: center;"">
                            <td style=""width: 100px; padding: 3px; border: 1px solid black;"">DATE</td>
                            <td style=""padding: 3px; border: 1px solid black;"">DESCRIPTION</td>
                            <td style=""width: 100px; padding: 3px; border: 1px solid black;"">HOURS</td>
                            <td style=""width: 100px; padding: 3px; border: 1px solid black;"">RATE</td>
                            <td style=""width: 100px; padding: 3px; border: 1px solid black;"">DISCOUNT</td>
                            <td style=""width: 100px; padding: 3px; border: 1px solid black;"">SERVICE FEE</td>
                            <td style=""width: 100px; padding: 3px; border: 1px solid black;"">AMOUNT</td>
                        </tr>";

                    decimal addedTotal = 0;
                    foreach (TransDocService service in serviceList)
                    {
                        html += @"
                        <tr>
                            <td style=""text-align: center; padding: 3px; border: 1px solid black;"">" + service.date.ToString("d") + @"</td>
                            <td style=""text-align: center; padding: 3px; border: 1px solid black;"">" + service.description + @"</td>
                            <td style=""text-align: center; padding: 3px; border: 1px solid black;"">" + Globals.FormatHours(service.hours) + @"</td>
                            <td style=""text-align: center; padding: 3px; border: 1px solid black;"">" + Globals.FormatMoney(service.rate) + @"</td>
                            <td style=""text-align: center; padding: 3px; border: 1px solid black;"">" + Globals.FormatMoney(service.discount) + @"</td>
                            <td style=""text-align: center; padding: 3px; border: 1px solid black;"">" + Globals.FormatMoney(service.serviceFee) + @"</td>
                            <td style=""text-align: center; padding: 3px; border: 1px solid black;"">" + Globals.FormatMoney(service.amount) + @"</td>
                        </tr>";
                        addedTotal += service.amount;
                    }

                    html += @"
                        <tr>
                            <td colspan=""6"" style=""text-align: right; padding-right: 10px; font-weight: bold;"">TOTAL</td>
                            <td style=""text-align: center; padding: 3px; border: 1px solid black; font-weight: bold;"">" + Globals.FormatMoney(total == decimal.MinValue ? addedTotal : total) + @"</td>
                        </tr>
                    </table>";
                }

                if (itemList.Count > 0)
                {
                    html += @"
                    <table style=""width: 100%; margin-top: 20px; text-align: left; border-collapse: collapse;"">
                        <tr style=""background-color: #DDD; text-align: center;"">
                            <td style=""width: 100px; padding: 3px; border: 1px solid black;"">DATE</td>
                            <td style=""padding: 3px; border: 1px solid black;"">DESCRIPTION</td>
                            <td style=""width: 100px; padding: 3px; border: 1px solid black;"">QUANTITY</td>
                            <td style=""width: 100px; padding: 3px; border: 1px solid black;"">RATE</td>
                            <td style=""width: 100px; padding: 3px; border: 1px solid black;"">AMOUNT</td>
                        </tr>";

                    decimal addedTotal = 0;
                    foreach (TransDocItem item in itemList)
                    {
                        string amount = Globals.FormatMoney(item.amount);
                        if (item.description.ToLower().Contains("discount")) amount = "(" + amount + ")";
                        html += @"
                        <tr>
                            <td style=""text-align: center; padding: 3px; border: 1px solid black;"">" + item.date.ToString("d") + @"</td>
                            <td style=""text-align: center; padding: 3px; border: 1px solid black;"">" + item.description + @"</td>
                            <td style=""text-align: center; padding: 3px; border: 1px solid black;"">" + item.quantity + @"</td>
                            <td style=""text-align: center; padding: 3px; border: 1px solid black;"">" + item.rate + @"</td>
                            <td style=""text-align: center; padding: 3px; border: 1px solid black;"">" + amount + @"</td>
                        </tr>";
                        addedTotal += item.amount;
                    }

                    html += @"
                        <tr>
                            <td colspan=""4"" style=""text-align: right; padding-right: 10px; font-weight: bold;"">TOTAL</td>
                            <td style=""text-align: center; padding: 3px; border: 1px solid black; font-weight: bold;"">" + Globals.FormatMoney(total == decimal.MinValue ? addedTotal : total) + @"</td>
                        </tr>
                    </table>";
                }

                if (paymentList.Count > 0)
                {
                    html += @"
                    <table style=""width: 100%; margin-top: 20px; text-align: left; background-color: white; border-collapse: collapse;"">
                        <tr style=""background-color: #DDD; text-align: center;"">
                            <td style=""width: 100px; padding: 3px; border: 1px solid black;"">DATE</td>
                            <td style=""padding: 3px; border: 1px solid black;"">PAYMENT</td>
                            <td style=""width: 100px; padding: 3px; border: 1px solid black;"">AMOUNT</td>
                        </tr>";

                    foreach (TransDocPayment payment in paymentList)
                    {
                        html += @"
                        <tr>
                            <td style=""text-align: center; padding: 3px; border: 1px solid black;"">" + payment.date.ToString("d") + @"</td>
                            <td style=""text-align: left; padding: 3px; border: 1px solid black;"">" + (isVoid ? "(VOID) " : "") + payment.description + @"</td>
                            <td style=""text-align: center; padding: 3px; border: 1px solid black;"">" + Globals.FormatMoney(payment.amount) + @"</td>
                        </tr>";
                    }

                    html += @"</table>";
                }

                html += @"<div style=""margin-top: 10px; text-align: center; font-size: 100%;"">" + (string.IsNullOrEmpty(memo) ? "Thank You for your business!!" : memo) + @"</div>";

                return html;
            }
            catch (Exception ex)
            {
                return "TransDoc.GetHTML Error: " + ex.Message;
            }
        }

        public static TransDoc GetTransactionDoc(int franchiseMask, TransactionStruct trans)
        {
            try
            {
                TransDoc doc = new TransDoc(franchiseMask, trans.customerID, trans.isVoid, trans.transID == 0 ? null : trans.transID.ToString());
                doc.AddItem(trans.dateApply, "Housekeeping", Globals.FormatHours(trans.hoursBilled) + " hours", Globals.FormatMoney(trans.hourlyRate), trans.hoursBilled * trans.hourlyRate);

                if (trans.hoursBilled * trans.hourlyRate > 0 || trans.serviceFee > 0)
                {
                    if (trans.discountReferral > 0) doc.AddItem(trans.dateApply, "Referral Discount", null, Globals.FormatPercent(trans.discountReferral), Globals.CalculateDiscountPercent(trans.hoursBilled, trans.hourlyRate, trans.serviceFee, trans.discountReferral, trans.dateApply));
                    if (trans.discountPercent > 0) doc.AddItem(trans.dateApply, "Discount", null, Globals.FormatPercent(trans.discountPercent), Globals.CalculateDiscountPercent(trans.hoursBilled, trans.hourlyRate, trans.serviceFee, trans.discountPercent, trans.dateApply));
                }
                if (trans.discountAmount > 0) doc.AddItem(trans.dateApply, "Discount", null, null, trans.discountAmount);
                doc.AddItem(trans.dateApply, "Service Fee (Travel & Supplies)", null, null, trans.serviceFee);
                if (trans.subContractorCC != 0) doc.AddItem(trans.dateApply, "Carpet Cleaning", null, null, trans.subContractorCC);
                if (trans.subContractorWW != 0) doc.AddItem(trans.dateApply, "Window Washing", null, null, trans.subContractorWW);
                if (trans.subContractorHW != 0) doc.AddItem(trans.dateApply, "Home Guard", null, null, trans.subContractorHW);
                if (trans.salesTax > 0) doc.AddItem(trans.dateApply, "Tax", null, Globals.FormatPercent(trans.salesTax, false), Globals.ExtractSalesTaxFromTotal(trans.total, trans.tips, trans.salesTax));
                if (trans.tips > 0) doc.AddItem(trans.dateApply, "Tips", null, null, trans.tips);
                if (trans.transType != "Invoice") doc.AddPayment(trans.dateCreated, trans.paymentType + (Globals.IsPaymentCreditCard(trans.paymentType) ? " <b> xxxx-xxxx-xxxx-" + trans.lastFourCard + "</b>" : ""), (trans.transType == "Return" ? -trans.total : trans.total));
                doc.total = (trans.transType == "Return" ? -trans.total : trans.total);
                doc.memo = trans.notes;
                doc.emailAddress = trans.email;
                doc.emailSubject = trans.transType == "Invoice" ? "2 Local Gals Invoice" : "2 Local Gals Sales Receipt";

                if (trans.customerID > 0)
                {
                    foreach (AppStruct app in Database.GetAppsByCustomerID(trans.customerID))
                    {
                        if (app.appointmentDate.Date == trans.dateApply.Date)
                        {
                            string followUpUrl = Globals.BuildQueryString(Globals.baseUrlSecure + @"FollowUp.aspx", "appID", Globals.Encrypt(app.appointmentID.ToString()));
                            if (string.IsNullOrEmpty(doc.memo)) doc.memo = "";
                            else doc.memo += @"<br/><br/>";
                            doc.memo += @"<a href=""" + followUpUrl + @""">How did we do? Your feedback is very important to us.</a>";
                            break;
                        }
                    }
                }
                return doc;
            }
            catch
            {
                return new TransDoc(0, 0);
            }
        }


        public static TransDoc GetGiftCardDoc(int franchiseMask, GiftCardStruct giftCard)
        {
            try
            {
                TransDoc doc = new TransDoc(franchiseMask, giftCard.customerID, giftCard.isVoid, giftCard.giftCardID.ToString());
                doc.AddItem(Globals.UtcToMst(giftCard.dateCreated), "E-Gift Card (" + Globals.EncodeZBase32((uint)giftCard.giftCardID) + ")", "1", null, giftCard.amount);
                doc.AddPayment(Globals.UtcToMst(giftCard.dateCreated), giftCard.paymentType + (Globals.IsPaymentCreditCard(giftCard.paymentType) ? " <b> xxxx-xxxx-xxxx-" + giftCard.lastFourCard + "</b>" : ""), giftCard.amount);
                doc.total = giftCard.amount;
                doc.emailAddress = giftCard.billingEmail;
                doc.emailSubject = "2 Local Gals E-Gift Card Receipt";
                return doc;
            }
            catch
            {
                return new TransDoc(0, 0);
            }
        }

        public static TransDoc GetCleaningPackDoc(int franchiseMask, DBRow pack)
        {
            try
            {
                int cleaningPackID = pack.GetInt("cleaningPackID");
                int customerID = pack.GetInt("customerID");
                bool isVoid = pack.GetBool("isVoid");
                int visits = pack.GetInt("visits");
                string transType = pack.GetString("transType");
                string paymentType = pack.GetString("paymentType");
                string email = pack.GetString("email");
                string lastFourCard = pack.GetString("lastFourCard");
                string memo = pack.GetString("memo");
                decimal hoursPerVisit = pack.GetDecimal("hoursPerVisit");
                decimal serviceFeePerVisit = pack.GetDecimal("serviceFeePerVisit");
                decimal ratePerHour = pack.GetDecimal("ratePerHour");
                decimal amount = pack.GetDecimal("amount");
                DateTime created = Globals.UtcToMst(pack.GetDate("dateCreated"));

                TransDoc doc = new TransDoc(franchiseMask, customerID, isVoid, cleaningPackID.ToString());
                doc.AddItem(created, "Housekeeping", visits.ToString(), Globals.FormatMoney(ratePerHour * hoursPerVisit), ratePerHour * hoursPerVisit * visits);
                doc.AddItem(created, "Service Fee", visits.ToString(), Globals.FormatMoney(serviceFeePerVisit), serviceFeePerVisit * visits);
                doc.AddItem(created, "Cleaing Pack Savings", null, null, ((ratePerHour * hoursPerVisit + serviceFeePerVisit) * visits) - amount);
                doc.AddPayment(created, paymentType + (Globals.IsPaymentCreditCard(paymentType) ? " <b> xxxx-xxxx-xxxx-" + lastFourCard + "</b>" : ""), (transType == "Return" ? -amount : amount));
                doc.total = (transType == "Return" ? -amount : amount);
                doc.memo = memo;
                doc.emailSubject = "2 Local Gals Cleaning Pack Receipt";
                doc.emailAddress = email;

                return doc;
            }
            catch
            {
                return new TransDoc(0, 0);
            }
        }
    }
}