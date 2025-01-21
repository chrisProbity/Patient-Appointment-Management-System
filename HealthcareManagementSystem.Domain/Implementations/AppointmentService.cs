using AutoMapper;
using HealthcareManagementSystem.Data.DataContext;
using HealthcareManagementSystem.Data.DTOs.Request;
using HealthcareManagementSystem.Data.DTOs.Response;
using HealthcareManagementSystem.Data.Models;
using HealthcareManagementSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HealthcareManagementSystem.Domain.Implementations
{
    public class AppointmentService(HealthMgtSystemDbContext dbContext, IMapper mapper) : IAppointmentService
    {
        public async Task<GlobalResponse<AppointmentResponse>> BookAppointment(AppointmentRequestDto request, Guid userId)
        {
            var user = await dbContext.Users.Include(i => i.Patient).FirstOrDefaultAsync(p => p.Id == userId);

            var doctor = await dbContext.Doctors.Include(i => i.DoctorAvailabilities).FirstOrDefaultAsync(d => d.Id == request.DoctorId);

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

            int doctorAppointments = await dbContext.Appointments.CountAsync(a => a.DoctorId == request.DoctorId 
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

            var doctorWorkHour = doctor.DoctorAvailabilities.FirstOrDefault(x => x.Day == nameof(request.AppointmentDate.DayOfWeek));

            if (doctorWorkHour == null)
            {
                return new GlobalResponse<AppointmentResponse>
                {
                    Status = false,
                    StatusCode = 400,
                    Message = "The doctor is not available for this date."
                };
            }

            DateTime.TryParseExact(doctorWorkHour.ResumptionTime, "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime doctorStartTime);
            DateTime.TryParseExact(doctorWorkHour.ClosingTime, "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime doctorEndTime);
            DateTime.TryParseExact(doctorWorkHour.ClosingTime, "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime appointmentTime);

            if (appointmentTime < doctorStartTime || appointmentTime > doctorEndTime)
            {
                return new GlobalResponse<AppointmentResponse>
                {
                    Status = false,
                    StatusCode = 400,
                    Message = "The requested time is outside the doctor's working hours."
                };
            }

            bool appointmentTimeIsNotAvailable = await dbContext.Appointments.AnyAsync(x => x.Date.Date == request.AppointmentDate.Date 
                 && x.Time.ToLower() == request.AppointmentTime.ToLower());
            
            if (appointmentTimeIsNotAvailable)
            {
                return new GlobalResponse<AppointmentResponse>
                {
                    Status = false,
                    StatusCode = 400,
                    Message = "This time slot is already booked."
                };
            }

            var newAppointment = new Appointment
            {
                Patient = user.Patient,
                Doctor = doctor,
                Date = request.AppointmentDate,
                Time = request.AppointmentTime,
            };

            await dbContext.Appointments.AddAsync(newAppointment);
            await dbContext.SaveChangesAsync();

            var bookingResponse = mapper.Map<AppointmentResponse>(newAppointment);
            bookingResponse.PatientName = $"{user!.Patient!.FirstName} {user.Patient.LastName}";
            bookingResponse.DoctorName = $"{doctor.FirstName} {doctor.LastName}";

            return new GlobalResponse<AppointmentResponse>
            {
                Status = true,
                StatusCode = 200,
                Message = "Appointment booked successfully.",
                Data = bookingResponse
            };
        }

        public async Task<GlobalResponse<List<AppointmentResponse>>> GetAllAppointments()
        {
            var appointments = await dbContext.Appointments.Include(d => d.Patient).Include(i => i.Doctor)
                 .OrderByDescending(x => x.Date).Select(s => new AppointmentResponse
                 {
                     Id = s.Id,
                     PatientName = $"{s.Patient.FirstName} {s.Patient.LastName}",
                     PatientMRN = s.Patient.MRN,
                     Date = s.Date,
                     Time = s.Time
                 }).ToListAsync();
            return new GlobalResponse<List<AppointmentResponse>>
            {
                Status = true,
                StatusCode = 200,
                Message = "Operation was successful",
                Data = appointments
            };
        }

        public async Task<GlobalResponse<List<DoctorScheduleResponse>>> GetDoctorSchedules(Guid userId)
        {
            var user = await dbContext.Users.Include(i => i.Doctor).FirstOrDefaultAsync(x => x.Id == userId);

            var appointments = await dbContext.Appointments.Include(d => d.Patient)
                .Where(r => r.DoctorId == user!.Doctor!.Id).OrderByDescending(x => x.Date).Select(s => new DoctorScheduleResponse
                {
                    AppointmentId = s.Id,
                    PatientName = $"{s.Patient.FirstName} {s.Patient.LastName}",
                    PatientMRN = s.Patient.MRN,
                    AppointmentDate = s.Date.ToShortDateString(),
                    AppointmentTime = s.Time
                }).ToListAsync();
            return new GlobalResponse<List<DoctorScheduleResponse>>
            {
                Status = true,
                StatusCode = 200,
                Message = "Operation was successful",
                Data = appointments
            };
        }
    }
}
