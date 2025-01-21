namespace HealthcareManagementSystem.Data.DTOs.Request
{
    public class PatientUpdateRequestDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
    }
}
