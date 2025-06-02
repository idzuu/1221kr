using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _1221kr.Models
{
    using _1221kr.Models;
    using System.ComponentModel.DataAnnotations;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ConsultationDetailsViewModel
    {
        public Consultation Consultation { get; set; }
        public ConsultationBooking BookingModel { get; set; }
    }

    public class ConsultationBooking
    {

        public int Id { get; set; }
        public int ConsultationId { get; set; }
        public Consultation Consultation { get; set; }
        public string ClientId { get; set; }
        public DateTime SelectedDate { get; set; }
        public string Notes { get; set; }
        public DateTime BookingDate { get; set; }
        public BookingStatus Status { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public List<DayOfWeek> AvailableDays { get; set; }

    }

    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Completed
    }
}