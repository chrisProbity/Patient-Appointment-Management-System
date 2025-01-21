namespace HealthcareManagementSystem.Data.DTOs.Response
{
    public class DoctorScheduleResponse
    {
        public Guid AppointmentId { get; set; }
        public string? PatientName { get; set; }
        public string? PatientMRN { get; set; }
        public string? AppointmentTime { get; set; }
        public string? AppointmentDate { get; set; }
    }
}
