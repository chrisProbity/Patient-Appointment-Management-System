namespace HealthcareManagementSystem.Data.Models
{
    public class Doctor : BaseEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Specialty { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public Guid UserId { get; set; }
        public required User User { get; set; }
        public ICollection<DoctorAvailability> DoctorAvailabilities { get; set; } = new List<DoctorAvailability>();
    }
}
