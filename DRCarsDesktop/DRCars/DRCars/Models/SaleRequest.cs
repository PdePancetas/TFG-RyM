using System;

namespace DRCars.Models
{
    public enum RequestStatus
    {
        Pending = 0,
        Scheduled = 1,
        Completed = 2,
        Cancelled = 3
    }

    public class SaleRequest
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string DesiredBrand { get; set; }
        public string DesiredModel { get; set; }
        public decimal? Budget { get; set; }
        public string DeliveryTimeframe { get; set; }
        public string AdditionalDetails { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? AppointmentDate { get; set; }
    }
}
