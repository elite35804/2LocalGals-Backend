using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoLocalGals.DTO
{
    public class PaymentDTO
    {
        public DateTime Date { get; set; }
        public string Customer { get; set; }
        public List<PaymentDetail> Details { get; set; }
        public decimal Total { get; set; }
        public decimal AveragePayRate { get; set; }
        public string Balance { get; set; }
        public string PymentType { get; set; }
    }

    public class PaymentDetail
    {
        public int AppointmentId { get; set; }
        public string ContractorTitle { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal SubCon { get; set; }
        public string Hours { get; set; }
        public decimal HourRate { get; set; }
        public decimal Tips { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal AproxPay { get; set; }
    }
}