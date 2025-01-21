using HealthcareManagementSystem.Data.DTOs.Request;
using HealthcareManagementSystem.Data.DTOs.Response;

namespace HealthcareManagementSystem.Domain.Interfaces
{
    public interface IDoctorService
    {
        Task<GlobalResponse<DoctorResponseDto>> CreateDoctor(CreateDoctorDto requestDto);
        Task<GlobalResponse<DoctorResponseDto>> UpdateDoctor(DoctorUpdateRequestDto requestModel, Guid doctorId);
        Task<GlobalResponse<DoctorResponseDto>> GetDoctor(Guid doctorId);
        Task<GlobalResponse<List<DoctorResponseDto>>> GetAllDoctors();
        Task<GlobalResponse<DoctorAvailabilityResponse>> CreateAvailablePeriod(DoctorAvailablePeriod requestDto, Guid doctorId);
    }
}
