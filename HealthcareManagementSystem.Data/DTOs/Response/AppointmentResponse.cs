namespace HealthcareManagementSystem.Data.DTOs.Response
{
    public class AppointmentResponse
    {
        public Guid Id { get; set; }
        public string? PatientName { get; set; }
        public string? PatientMRN { get; set; }
        public string? DoctorName { get; set; }
        public DateTime Date { get; set; }
        public string? Time { get; set; }
    }
}
