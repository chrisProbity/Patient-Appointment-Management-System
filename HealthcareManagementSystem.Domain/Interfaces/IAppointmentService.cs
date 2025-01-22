using HealthcareManagementSystem.Data.DTOs.Request;
using HealthcareManagementSystem.Data.DTOs.Response;

namespace HealthcareManagementSystem.Domain.Interfaces
{
    public interface IAppointmentService
    {
        Task<GlobalResponse<AppointmentResponse>> BookAppointment(AppointmentRequestDto request, Guid userId);
        Task<GlobalResponse<List<DoctorScheduleResponse>>> GetDoctorSchedules(Guid doctorId);
        Task<GlobalResponse<List<AppointmentResponse>>> GetAllAppointments();
        Task<GlobalResponse<AppointmentResponse>> UpdateAppointment(AppointmentUpdateDto request, Guid appointmentId);
        Task<GlobalResponse<bool>> CancelAppointment(Guid appointmentId);
    }
}
