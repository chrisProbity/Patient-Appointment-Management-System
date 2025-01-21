namespace HealthcareManagementSystem.Data.DTOs.Request
{
    public class DoctorAvailablePeriod
    {
        public required string Day { get; set; }
        public required string ResumptionTime { get; set; }
        public required string ClosingTime { get; set; }

    }
}
