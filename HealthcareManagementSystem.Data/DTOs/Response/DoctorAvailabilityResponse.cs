namespace HealthcareManagementSystem.Data.DTOs.Response
{
    public class DoctorAvailabilityResponse
    {
        public Guid Id { get; set; }
        public required string Day { get; set; }
        public required string ResumptionTime { get; set; }
        public required string ClosingTime { get; set; }
        public required string Doctor { get; set; }
        public DateTime DateTimeCreated { get; set; }
    }
}
