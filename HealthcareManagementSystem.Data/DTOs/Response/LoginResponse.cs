namespace HealthcareManagementSystem.Data.DTOs.Response
{
    public class LoginResponse
    {
        public Guid UserId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AccessToken { get; set; }
    }
}
