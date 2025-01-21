namespace HealthcareManagementSystem.Data.DTOs.Response
{
    public class CreatePatientResponseDto
    {
        public Guid Id { get; set; }
        public required string MedicalRecordNumber { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
