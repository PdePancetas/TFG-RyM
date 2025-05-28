using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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
        [JsonProperty("idVehiculo")]
        public long Id { get; set; }

        [JsonProperty("marca")]
        public string Brand { get; set; }

        [JsonProperty("modelo")]
        public string Model { get; set; }

        [JsonProperty("annoFabricacion")]
        public int Year { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("kilometraje")]
        public int Kilometers { get; set; }

        [JsonProperty("precioCompra")]
        public decimal Price { get; set; }

        [JsonProperty("matricula")]
        public string LicensePlate { get; set; }

        [JsonProperty("numeroChasis")]
        public string VIN { get; set; }

        [JsonProperty("estado")]
        public string StatusString { get; set; }

        [JsonIgnore]
        public VehicleStatus Status
        {
            get
            {
                if (string.IsNullOrEmpty(StatusString))
                    return VehicleStatus.InStock;

                switch (StatusString.ToUpper())
                {
                    case "VENTA":
                        return VehicleStatus.ForSale;
                    case "VENDIDO":
                        return VehicleStatus.Sold;
                    case "REPARACION":
                        return VehicleStatus.InRepair;
                    case "GARAJE":
                        return VehicleStatus.InGarage;
                    default:
                        return VehicleStatus.InStock;
                }
            }
            set
            {
                switch (value)
                {
                    case VehicleStatus.ForSale:
                        StatusString = "VENTA";
                        break;
                    case VehicleStatus.Sold:
                        StatusString = "VENDIDO";
                        break;
                    case VehicleStatus.InRepair:
                        StatusString = "REPARACION";
                        break;
                    case VehicleStatus.InGarage:
                        StatusString = "GARAJE";
                        break;
                    default:
                        StatusString = "STOCK";
                        break;
                }
            }
        }

        [JsonProperty("proveedor")]
        public Supplier Supplier { get; set; }

        // Propiedades que no están en el JSON pero necesitamos para la aplicación
        [JsonIgnore]
        public decimal CostPrice { get; set; }

        [JsonProperty("transmision")]
        public string Transmission { get; set; } = "Automática";

        [JsonProperty("combustible")]
        public string FuelType { get; set; } = "Gasolina";

        [JsonIgnore]
        public string Category { get; set; } = "Estándar";

        [JsonIgnore]
        public int Doors { get; set; } = 4;

        [JsonIgnore]
        public int Seats { get; set; } = 5;

        [JsonIgnore]
        public int EnginePower { get; set; }

        [JsonIgnore]
        public int EngineCapacity { get; set; }

        [JsonIgnore]
        public string Description { get; set; }

        [JsonIgnore]
        public string Features { get; set; }

        [JsonIgnore]
        public string ImageUrl { get; set; }

        [JsonIgnore]
        public List<string> AdditionalImages { get; set; }

        [JsonIgnore]
        public DateTime ImportDate { get; set; }

        [JsonIgnore]
        public DateTime? SaleDate { get; set; }

        [JsonIgnore]
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public DateTime? UpdatedAt { get; set; }

        public Vehicle()
        {
            AdditionalImages = new List<string>();
            CreatedAt = DateTime.Now;
            Status = VehicleStatus.InStock;
        }
    }

    public class Supplier
    {
        [JsonProperty("idProveedor")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public string Name { get; set; }

        [JsonProperty("tipoProveedor")]
        public string Type { get; set; }

        [JsonProperty("telefono")]
        public string Phone { get; set; }

        [JsonProperty("ciudad")]
        public string City { get; set; }
    }
}
