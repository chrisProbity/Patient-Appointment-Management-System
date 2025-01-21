using System.ComponentModel.DataAnnotations;

namespace HealthcareManagementSystem.Data.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }

        public static readonly Role Admin = new() { Id = 1, Name = "Admin" };
        public static readonly Role Doctor = new() { Id = 2, Name = "Doctor" };
        public static readonly Role Patient = new() { Id = 3, Name = "Patient" };
    }
}
