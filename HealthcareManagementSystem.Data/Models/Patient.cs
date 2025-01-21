namespace HealthcareManagementSystem.Data.Models
{
    public class Patient : BaseEntity
    {
        public required string MRN { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public Guid UserId { get; set; }
        public required User User { get; set; }
    }
}
