using System;

namespace DRCars.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public int CustomerId { get; set; }
        public int SalesAgentId { get; set; }
        public decimal SalePrice { get; set; }
        public DateTime SaleDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Notes { get; set; }
    }
}
