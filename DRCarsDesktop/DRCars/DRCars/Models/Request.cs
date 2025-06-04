using Newtonsoft.Json;
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

    public class Request
    {
        [JsonProperty("idSolicitud")]
        public int Id { get; set; }

        [JsonProperty("cliente")]
        public ClientDTO cliente { get; set; }
        
        [JsonProperty("vehiculo")]
        public Vehicle Vehicle { get; set; }
        public RequestStatus Status { get; set; }

        [JsonProperty("fechaSolicitud")]
        public DateTime RequestDate { get; set; }

        [JsonProperty("precioSolicitud")]
        public decimal Budget { get; set; }

        [JsonProperty("motivo")]
        public string RequestReason { get; set; }

        [JsonProperty("descripcion")]
        public string RequestDescription { get; set; }

        public Request()
        {
            Status = RequestStatus.Pending;
        }

        // Campos adicionales para compatibilidad
        public string DesiredBrand { get; set; }
        public string DesiredModel { get; set; }


        public class RequestUser
        {
            [JsonProperty("dniCliente")]
            public String Id { get; set; }

            [JsonProperty("nombre")]
            public string Name { get; set; }

            [JsonProperty("apellidos")]
            public string Surname { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }
        }
    }
}
