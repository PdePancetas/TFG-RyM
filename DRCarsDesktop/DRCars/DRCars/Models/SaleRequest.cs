using System;

namespace DRCars.Models
{
    public enum RequestStatus
    {
        Pending,
        Scheduled,
        Completed,
        Cancelled
    }

    public class SaleRequest
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public string Comments { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public int? AssignedUserId { get; set; }
        public User AssignedUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Campos adicionales para compatibilidad
        public string DesiredBrand { get; set; }
        public string DesiredModel { get; set; }
        public decimal? Budget { get; set; }
        public string DeliveryTimeframe { get; set; }
        public string AdditionalDetails { get; set; }
        public DateTime? AppointmentDate { get; set; }

        public SaleRequest()
        {
            RequestDate = DateTime.Now;
            CreatedAt = DateTime.Now;
            Status = RequestStatus.Pending;
        }
    }
}
