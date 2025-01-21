namespace HealthcareManagementSystem.Data.DTOs.Request
{
    public class AppointmentRequestDto
    {
        public DateTime AppointmentDate { get; set; }
        public required string AppointmentTime { get; set; }
        public Guid DoctorId { get; set; }
    }
}
