using System;

namespace DRCars.Models
{
    public enum VehicleStatus
    {
        InStock = 0,
        InGarage = 1,
        ForSale = 2,
        Sold = 3,
        InRepair = 4
    }

    public class Vehicle
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string FuelType { get; set; }
        public string Transmission { get; set; }
        public int Kilometers { get; set; }
        public VehicleStatus Status { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
