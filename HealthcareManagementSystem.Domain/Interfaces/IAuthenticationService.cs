using HealthcareManagementSystem.Data.DTOs.Request;
using HealthcareManagementSystem.Data.DTOs.Response;

namespace HealthcareManagementSystem.Domain.Interfaces
{
    public interface IAuthenticationService
    {
        Task<GlobalResponse<LoginResponse>> Login(LoginRequest request);
    }
}
