using HealthcareManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthcareManagementSystem.Data.DataContext
{
    public class HealthMgtSystemDbContext : DbContext
    {
        public HealthMgtSystemDbContext(DbContextOptions<HealthMgtSystemDbContext> options): base (options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<DoctorAvailability> DoctorAvailabilities { get; set; }
        public DbSet<Appointment> Appointments { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Doctor" },
                new Role { Id = 3, Name = "Patient" }
            );
        }
    }
}
