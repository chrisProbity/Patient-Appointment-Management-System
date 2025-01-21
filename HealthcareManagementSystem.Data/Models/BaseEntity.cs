using System.ComponentModel.DataAnnotations;

namespace HealthcareManagementSystem.Data.Models
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime DateTimeCreated { get; set; } = DateTime.UtcNow.AddHours(1);
        public DateTime? DateTimeModified { get; set; }
    }
}
