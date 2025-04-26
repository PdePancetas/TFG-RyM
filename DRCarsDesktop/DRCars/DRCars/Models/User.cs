using System;

namespace DRCars.Models
{
    public enum UserRole
    {
        Admin = 0,
        Manager = 1,
        SalesAgent = 2,
        Viewer = 3
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; } // In a real app, this would be hashed
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
