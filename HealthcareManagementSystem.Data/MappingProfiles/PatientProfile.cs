using AutoMapper;
using HealthcareManagementSystem.Data.DTOs.Request;
using HealthcareManagementSystem.Data.DTOs.Response;
using HealthcareManagementSystem.Data.Models;

namespace HealthcareManagementSystem.Data.MappingProfiles
{
    public class PatientProfile : Profile
    {
        public PatientProfile()
        {
            CreateMap<CreatePatientDto, Patient>()
                .ForMember(m => m.MRN, f => f.Ignore());
                
            CreateMap<CreatePatientDto, User>()
                .ForMember(m => m.Username, f => f.MapFrom(x => x.PhoneNumber));

            CreateMap<Patient, CreatePatientResponseDto>()
                .ForMember(m => m.DateCreated, f => f.MapFrom(x => x.DateTimeCreated))
                .ForMember(m => m.MedicalRecordNumber, f => f.MapFrom(x => x.MRN));

            CreateMap<PatientUpdateRequestDto, Patient>()
                .ForMember(m => m.User, f => f.Ignore());
        }
    }
}
