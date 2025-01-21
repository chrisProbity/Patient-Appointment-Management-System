namespace HealthcareManagementSystem.Data.Models
{
    public class User : BaseEntity
    {
        public required string Username { get; set; }
        public int RoleId { get; set; }
        public required Role Role { get; set; }
        public required string PasswordHash { get; set; }
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
