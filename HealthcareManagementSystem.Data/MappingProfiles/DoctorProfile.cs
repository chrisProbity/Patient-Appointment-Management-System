using AutoMapper;
using HealthcareManagementSystem.Data.DTOs.Request;
using HealthcareManagementSystem.Data.DTOs.Response;
using HealthcareManagementSystem.Data.Models;

namespace HealthcareManagementSystem.Data.MappingProfiles
{
    public class DoctorProfile : Profile
    {
        public DoctorProfile() 
        {
            CreateMap<CreateDoctorDto, Doctor>();
            CreateMap<CreateDoctorDto, User>()
                .ForMember(m => m.Username, f => f.MapFrom(x => x.PhoneNumber));
            CreateMap<Doctor, DoctorResponseDto>()
                .ForMember(m => m.DateCreated, f => f.MapFrom(f => f.DateTimeCreated));
            CreateMap<DoctorAvailablePeriod, DoctorAvailability>();
            CreateMap<DoctorAvailability, DoctorAvailabilityResponse>()
                .ForMember(m => m.Doctor, f => f.Ignore());
            CreateMap<UserDto, User>();
        }
    }
}
