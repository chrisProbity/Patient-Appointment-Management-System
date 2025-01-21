using System.ComponentModel.DataAnnotations;

namespace HealthcareManagementSystem.Data.DTOs.Request
{
    public class CreatePatientDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Password { get; set; }
    }
}
