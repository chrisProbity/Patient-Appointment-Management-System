using AutoMapper;
using HealthcareManagementSystem.Data.DTOs.Response;
using HealthcareManagementSystem.Data.Models;

namespace HealthcareManagementSystem.Data.MappingProfiles
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            CreateMap<Appointment, AppointmentResponse>()
                .ForMember(m => m.PatientName, f => f.Ignore())
                .ForMember(m => m.DoctorName, f => f.Ignore());
        }
    }
}
