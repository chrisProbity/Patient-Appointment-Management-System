using HealthcareManagementSystem.Data.DTOs.Request;
using HealthcareManagementSystem.Data.DTOs.Response;

namespace HealthcareManagementSystem.Domain.Interfaces
{
    public interface IPatientService
    {
        Task<GlobalResponse<CreatePatientResponseDto>> CreatePatient(CreatePatientDto createPatientDto);
        Task<GlobalResponse<CreatePatientResponseDto>> UpdatePatient(PatientUpdateRequestDto requestModel, Guid patientId);
        Task<GlobalResponse<CreatePatientResponseDto>> GetPatient(Guid patientId);
        Task<GlobalResponse<List<CreatePatientResponseDto>>> GetAllPatients();

    }
}
