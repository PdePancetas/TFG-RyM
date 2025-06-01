using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DRCars.Models
{
    public enum VehicleStatus
    {
        STOCK,
        GARAJE,
        VENTA,
        VENDIDO
    }

    public enum TransmissionType
    {
        MANUAL,
        AUTOMATICA
    }

    public enum FuelType
    {
        GASOLINA,
        DIESEL,
        HIBRIDO,
        ELECTRICO
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
        [JsonConverter(typeof(StringEnumConverter))]
        public VehicleStatus Status { get; set; }
       
        [JsonProperty("proveedor")]
        public Supplier Supplier { get; set; }

        // Propiedades que no están en el JSON pero necesitamos para la aplicación
        [JsonIgnore]
        public decimal CostPrice { get; set; }

        
        [JsonProperty("transmision")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TransmissionType TransmissionType { get; set; }
        

        [JsonProperty("combustible")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FuelType FuelType { get; set; }
        
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
            Status = VehicleStatus.STOCK;
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
