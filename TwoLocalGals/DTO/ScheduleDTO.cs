using System;

namespace TwoLocalGals.DTO
{
    public class ScheduleDTO
    {

        public int AppointmentId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCity { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public decimal Hours { get; set; }
        public string Miles { get; set; }
        public double Minutes { get; set; }
        public bool Keys { get; set; }
    }
}