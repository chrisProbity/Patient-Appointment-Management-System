using HealthcareManagementSystem.Data.DTOs.Request;
using HealthcareManagementSystem.Data.DTOs.Response;

namespace HealthcareManagementSystem.Domain.Interfaces
{
    public interface IAppointmentService
    {
        Task<GlobalResponse<AppointmentResponse>> BookAppointment(AppointmentRequestDto request, Guid userId);
    }
}
