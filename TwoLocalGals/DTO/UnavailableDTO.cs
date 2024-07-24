using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoLocalGals.DTO
{
    public class UnavailableDTO
    {
        public int unavailableID { get; set; }
        public DateTime dateRequested { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public int recurrenceID { get; set; }
        public int recurrenceType { get; set; }
        public bool contractorActive { get; set; }
        public bool contractorScheduled { get; set; }
    }
}