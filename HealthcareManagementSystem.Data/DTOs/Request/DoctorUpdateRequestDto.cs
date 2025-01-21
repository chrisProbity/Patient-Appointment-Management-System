namespace HealthcareManagementSystem.Data.DTOs.Request
{
    public class DoctorUpdateRequestDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required string Specialty { get; set; }
    }
}
