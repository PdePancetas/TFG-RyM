using Newtonsoft.Json;
using System;

namespace DRCars.Models
{

    public class Appointment
    {
        [JsonProperty("idReserva")]
        public long Id { get; set; }

        [JsonProperty("cliente")]
        public ClientDTO client { get; set; }

        [JsonProperty("vehiculo")]
        public Vehicle Vehicle { get; set; }

        [JsonProperty("fechaReserva")]
        public DateTime AppointmentDate { get; set; }

        [JsonProperty("precioReserva")]
        public Decimal AppointmentPrice { get; set; }

        [JsonProperty("notas")]
        public String AppointmentNotes { get; set; }


        public Appointment()
        {

        }
    }

    
}
