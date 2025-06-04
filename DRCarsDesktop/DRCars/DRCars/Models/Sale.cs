using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRCars.Models
{
    public class Sale
    {
        [JsonProperty("idVenta")]
        public long Id { get; set; }

        [JsonProperty("cliente")]
        public ClientDTO cliente { get; set; }

        [JsonProperty("vehiculo")]
        public Vehicle Vehicle { get; set; }

        [JsonProperty("fechaVenta")]
        public DateTime SaleDate { get; set; }

        [JsonProperty("precioVenta")]
        public decimal SalePrice { get; set; }
    }
}
