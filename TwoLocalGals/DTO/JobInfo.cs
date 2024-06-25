using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoLocalGals.DTO
{
    public class JobInfo
    {
        public string customerName { get; set; }
        public string bestPhone { get; set; }
        public bool bestPhoneCell { get; set; }
        public string alternatePhoneOne { get; set; }
        public bool alternatePhoneOneCell { get; set; }
        public string alternatePhoneTwo { get; set; }
        public bool alternatePhoneTwoCell { get; set; }
    }
}