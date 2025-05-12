using System;
using System.Collections.Generic;

namespace DRCars.Models
{
    public enum VehicleStatus
    {
        InStock,
        InGarage,
        ForSale,
        Sold,
        InRepair
    }

    public enum TransmissionType
    {
        Manual,
        Automatic,
        SemiAutomatic
    }

    public enum FuelType
    {
        Gasoline,
        Diesel,
        Electric,
        Hybrid,
        PlugInHybrid,
        LPG,
        CNG
    }

    public class Vehicle
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public int Kilometers { get; set; }
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public string VIN { get; set; }
        public string LicensePlate { get; set; }
        public string Transmission { get; set; } // Cambiado a string para compatibilidad
        public string FuelType { get; set; } // Cambiado a string para compatibilidad
        public string Category { get; set; } // Añadido campo Category
        public int Doors { get; set; }
        public int Seats { get; set; }
        public int EnginePower { get; set; }
        public int EngineCapacity { get; set; }
        public VehicleStatus Status { get; set; }
        public string Description { get; set; }
        public string Features { get; set; }
        public string ImageUrl { get; set; }
        public List<string> AdditionalImages { get; set; }
        public DateTime ImportDate { get; set; }
        public DateTime? SaleDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Vehicle()
        {
            AdditionalImages = new List<string>();
            CreatedAt = DateTime.Now;
            Status = VehicleStatus.InStock;
        }
    }
}
