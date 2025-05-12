using System;

namespace DRCars.Models
{
    public enum UserRole
    {
        Admin,
        Manager,
        SalesAgent,
        Viewer
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; } // Añadido campo Phone
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public User()
        {
            CreatedAt = DateTime.Now;
            IsActive = true;
        }
    }
}
