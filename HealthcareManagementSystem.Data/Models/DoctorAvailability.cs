namespace HealthcareManagementSystem.Data.Models
{
    public class DoctorAvailability : BaseEntity
    {
        public required string Day { get; set; }
        public required string ResumptionTime { get; set; }
        public required string ClosingTime { get; set; }
        public Guid DoctorId { get; set; }
        public required Doctor Doctor { get; set; }
    }
}
