using Newtonsoft.Json;
using System;

namespace DRCars.Models
{

    public class Sale
    {
        [JsonProperty("idVenta")]
        public long Id { get; set; }

        [JsonProperty("cliente")]
        public SalesClient cliente { get; set; }

        [JsonProperty("vehiculo")]
        public Vehicle Vehicle { get; set; }

        [JsonProperty("precioVenta")]
        public decimal SalePrice { get; set; }

        public decimal TotalPrice { get; set; }
        public int SalesAgentId { get; set; }
        public User SalesAgent { get; set; }

        public DateTime SaleDate { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Sale()
        {
            
            CreatedAt = DateTime.Now;
            DateTime original = DateTime.Now;
            DateTime truncated = new DateTime(
                original.Year,
                original.Month,
                original.Day,
                original.Hour,
                original.Minute,
                original.Second
            );
            SaleDate = truncated;
            CreatedAt = truncated;
        }
    }

    public class SalesClient {

        [JsonProperty("dniCliente")]
        public string Id { get; set; }

        [JsonProperty("nombre")]
        public string Name { get; set; }

        [JsonProperty("apellidos")]
        public string surname { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

    }
}
