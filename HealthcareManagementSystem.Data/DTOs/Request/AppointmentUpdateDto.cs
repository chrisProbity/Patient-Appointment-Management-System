namespace HealthcareManagementSystem.Data.DTOs.Request
{
    public class AppointmentUpdateDto
    {
        public DateTime AppointmentDate { get; set; }
        public required string AppointmentTime { get; set; }
    }
}
