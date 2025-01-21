namespace HealthcareManagementSystem.Data.DTOs.Response
{
    public class AppointmentResponse
    {
        public Guid Id { get; set; }
        public string? Patient { get; set; }
        public string? Doctor { get; set; }
        public DateTime Date { get; set; }
        public string? Time { get; set; }
    }
}
