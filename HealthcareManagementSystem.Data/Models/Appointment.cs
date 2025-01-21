namespace HealthcareManagementSystem.Data.Models
{
    public class Appointment : BaseEntity
    {
        public Guid PatientId { get; set; }
        public required Patient Patient { get; set; }
        public Guid DoctorId { get; set; }
        public required Doctor Doctor { get; set; }
        public DateTime Date { get; set; }
        public required string Time { get; set; }
    }
}
