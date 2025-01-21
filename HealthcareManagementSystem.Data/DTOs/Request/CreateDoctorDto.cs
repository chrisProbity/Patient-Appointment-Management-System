﻿namespace HealthcareManagementSystem.Data.DTOs.Request
{
    public class CreateDoctorDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required string Specialty { get; set; }
        public required string Password { get; set; }
    }
}
