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

    public class SaleRequest
    {
        [JsonProperty("idReserva")]
        public int Id { get; set; }

        [JsonProperty("cliente")]
        public RequestUser cliente { get; set; }
        
        [JsonProperty("vehiculo")]
        public Vehicle Vehicle { get; set; }

        [JsonProperty("descripcion")]
        public string Comments { get; set; }
        public RequestStatus Status { get; set; }

        [JsonProperty("fechaReserva")]
        public DateTime RequestDate { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public int? AssignedUserId { get; set; }
        public User AssignedUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Campos adicionales para compatibilidad
        public string DesiredBrand { get; set; }
        public string DesiredModel { get; set; }
        public decimal? Budget { get; set; }
        public string DeliveryTimeframe { get; set; }
        public string AdditionalDetails { get; set; }
        public DateTime? AppointmentDate { get; set; }

        public SaleRequest()
        {
            RequestDate = DateTime.Now;
            CreatedAt = DateTime.Now;
            Status = RequestStatus.Pending;
        }


        

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
