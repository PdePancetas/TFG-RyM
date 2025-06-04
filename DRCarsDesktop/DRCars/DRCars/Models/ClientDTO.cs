using Newtonsoft.Json;

namespace DRCars.Models
{
    public class ClientDTO
    {
        [JsonProperty("dniCliente")]
        public string Id { get; set; }

        [JsonProperty("usuario")]
        public UserDTO User { get; set; }

        [JsonProperty("nombre")]
        public string Name { get; set; }

        [JsonProperty("apellidos")]
        public string Surname { get; set; }
    }

    public class UserDTO
    {
        [JsonProperty("usuario")]
        public string User { get; set; }
       
    }
}
