namespace HealthcareManagementSystem.Data.DTOs.Response
{
    public class DoctorResponseDto
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required string Specialty { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
