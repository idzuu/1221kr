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

        [Required]
        [ForeignKey("Consultation")]
        public int ConsultationId { get; set; }

        // Навигационное свойство
        public virtual Consultation Consultation { get; set; }

        [Required]
        public string ClientId { get; set; }

        [Required]
        public DateTime SelectedDate { get; set; }

        public string Notes { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.Now;

        public BookingStatus Status { get; set; } = BookingStatus.Pending;
    }

    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Completed
    }
}