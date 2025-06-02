﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace DRCars.Models
{
    public enum UserRole
    {
        ADMIN,
        USER
    }

    public class User
    {
       
        public string Name { get; set; }

        [JsonProperty("usuario")]
        public string Email { get; set; }

        [JsonProperty("contraseña")]
        public string Password { get; set; }

        [JsonProperty("tipoUsuario")]
        [JsonConverter(typeof(StringEnumConverter))]
        public UserRole Role { get; set; }

        [JsonProperty("ultimo_acceso")]
        public DateTime? LastLogin { get; set; }

        [JsonProperty("registro_cuenta")]
        public DateTime CreatedAt { get; set; }

        public bool IsActive {get; set;}

        public User()
        {
            DateTime original = DateTime.Now;
            DateTime truncated = new DateTime(
                original.Year,
                original.Month,
                original.Day,
                original.Hour,
                original.Minute,
                original.Second
            );
            CreatedAt = truncated;
            IsActive = true;
        }
    }
}
