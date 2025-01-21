using HealthcareManagementSystem.Data.DataContext;
using HealthcareManagementSystem.Data.DTOs.Request;
using HealthcareManagementSystem.Data.DTOs.Response;
using HealthcareManagementSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthcareManagementSystem.Domain.Implementations
{
    public class AppointmentService(HealthMgtSystemDbContext dbContext) : IAppointmentService
    {
        public async Task<GlobalResponse<AppointmentResponse>> BookAppointment(AppointmentRequestDto request, Guid userId)
        {
            var user = await dbContext.Users.Include(i => i.Patient).FirstOrDefaultAsync(p => p.Id == userId);

            var doctor = dbContext.Doctors.Include(i => i.DoctorAvailabilities).FirstOrDefaultAsync(d => d.Id == request.DoctorId);

            if (user == null)
            {
                return new GlobalResponse<AppointmentResponse>
                {
                    Status = false,
                    StatusCode = 400,
                    Message = "Invalid patient Id"
                };
            }

            if (doctor == null)
            {
                return new GlobalResponse<AppointmentResponse>
                {
                    Status = false,
                    StatusCode = 400,
                    Message = "Invalid Doctor Id"
                };
            }

            var existingAppointments = await dbContext.Appointments.Where(a => a.PatientId == user!.Patient!.Id 
                && a.DoctorId == request.DoctorId && a.Date.Date == request.AppointmentDate.Date).ToListAsync();
            
            if (existingAppointments.Any())
            {
                return new GlobalResponse<AppointmentResponse>
                {
                    Status = false,
                    StatusCode = 400,
                    Message = "You already have an appointment with this doctor on this date."
                };
            }

            var doctorAppointments = await dbContext.Appointments.CountAsync(a => a.DoctorId == request.DoctorId 
                && a.Date.Date == request.AppointmentDate.Date);
             

            if (doctorAppointments >= 10)
            {
                return new GlobalResponse<AppointmentResponse>
                {
                    Status = false,
                    StatusCode = 400,
                    Message = "The doctor is fully booked for this date."
                };
            }

            return new GlobalResponse<AppointmentResponse>
            {
                Status = true,
                StatusCode = 200,
                Message = "Appointment booked successfully."
            };

        }
    }
}
