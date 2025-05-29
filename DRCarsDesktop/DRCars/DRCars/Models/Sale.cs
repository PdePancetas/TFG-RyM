using System;

namespace DRCars.Models
{
    public enum PaymentMethod
    {
        EFECTIVO,
        TRANSFERENCIA,
        CREDITO,
        FINANCIACION,
        OTRO
    }

    public class Sale
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerIdNumber { get; set; }
        public int CustomerId { get; set; } // Añadido para compatibilidad
        public decimal SalePrice { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentMethod { get; set; } // Cambiado a string para compatibilidad
        public string PaymentDetails { get; set; }
        public int SalesAgentId { get; set; }
        public User SalesAgent { get; set; }
        public DateTime SaleDate { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Sale()
        {
            SaleDate = DateTime.Now;
            CreatedAt = DateTime.Now;
        }
    }
}
