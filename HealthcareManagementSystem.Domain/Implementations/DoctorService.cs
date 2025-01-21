using AutoMapper;
using HealthcareManagementSystem.Data.DataContext;
using HealthcareManagementSystem.Data.DTOs.Request;
using HealthcareManagementSystem.Data.DTOs.Response;
using HealthcareManagementSystem.Data.Models;
using HealthcareManagementSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthcareManagementSystem.Domain.Implementations
{
    public class DoctorService(HealthMgtSystemDbContext dbContext, IMapper mapper, IPasswordHasher<User> passwordHasher) : IDoctorService
    {
        public async Task<GlobalResponse<DoctorAvailabilityResponse>> CreateAvailablePeriod(DoctorAvailablePeriod availablePeriod, Guid doctorId)
        {
            bool availabilityPeriodExists = await dbContext.DoctorAvailabilities.AnyAsync(x => x.Day.ToLower() == availablePeriod.Day.ToLower() && x.ResumptionTime.ToLower() == availablePeriod.ClosingTime.ToLower() && x.DoctorId == doctorId);

            if (availabilityPeriodExists)
            {
                return new GlobalResponse<DoctorAvailabilityResponse>
                {
                    Status = false,
                    StatusCode = 400,
                    Message = "Record already exists"
                };
            }

            var doctor = await dbContext.Doctors.FirstOrDefaultAsync(x => x.Id == doctorId);
            var availability = mapper.Map<DoctorAvailability>(availablePeriod);
            availability.Doctor = doctor;

            await dbContext.DoctorAvailabilities.AddAsync(availability);
            await dbContext.SaveChangesAsync();

            var responseData = mapper.Map<DoctorAvailabilityResponse>(availability);
            responseData.Doctor = $"{doctor.FirstName} {doctor.LastName}";

            return new GlobalResponse<DoctorAvailabilityResponse>
            {
                Status = true,
                StatusCode = 200,
                Message = "Availability period was successfully created",
                Data = responseData
            };

        }

        public async Task<GlobalResponse<DoctorResponseDto>> CreateDoctor(CreateDoctorDto requestDto)
        {
            var doctor = await dbContext.Doctors.FirstOrDefaultAsync(x => x.PhoneNumber == requestDto.PhoneNumber || x.Email == requestDto.Email);

            if (doctor != null)
            {
                return new GlobalResponse<DoctorResponseDto>
                {
                    Status = false,
                    StatusCode = 400,
                    Message = doctor.PhoneNumber == requestDto.PhoneNumber ? "Phone number has already been used" : "Email has already been used"
                };
            }

            //Create new instance of User and Doctor objects
            Doctor newDoctor = mapper.Map<Doctor>(requestDto);
            User user = mapper.Map<User>(requestDto);
            user.PasswordHash = passwordHasher.HashPassword(user, requestDto.Password);
            user.RoleId = Role.Doctor.Id;
            user.Doctor = newDoctor;

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var doctorDetails = mapper.Map<DoctorResponseDto>(newDoctor);

            return new GlobalResponse<DoctorResponseDto>
            {
                Status = true,
                StatusCode = 200,
                Message = "Doctor created successfully.",
                Data = doctorDetails,
            };
        }

        public async Task<GlobalResponse<List<DoctorResponseDto>>> GetAllDoctors()
        {
            var doctors = await dbContext.Doctors.OrderByDescending(x => x.DateTimeCreated).Select(s => new DoctorResponseDto
            {
                Id = s.Id,
                Email = s.Email,
                FirstName = s.FirstName,
                LastName = s.LastName,
                PhoneNumber = s.PhoneNumber,
                Specialty = s.Specialty,
                DateCreated = s.DateTimeCreated
            }).ToListAsync();

            return new GlobalResponse<List<DoctorResponseDto>>
            {
                Status = true,
                StatusCode = 200,
                Message = doctors.Count > 0 ? "Successfully fetched doctors" : "No Doctors available.",
                Data = doctors
            };
        }

        public async Task<GlobalResponse<DoctorResponseDto>> GetDoctor(Guid doctorId)
        {
            var doctor = await dbContext.Patients.FirstOrDefaultAsync(x => x.Id == doctorId);

            if (doctor == null)
            {
                return new GlobalResponse<DoctorResponseDto>
                {
                    Status = false,
                    StatusCode = 404,
                    Message = "Doctor's record was not found"
                };
            }

            return new GlobalResponse<DoctorResponseDto>
            {
                Status = true,
                StatusCode = 200,
                Message = "Doctor's record was fetched successful",
                Data = mapper.Map<DoctorResponseDto>(doctor)
            };

        }

        public async Task<GlobalResponse<DoctorResponseDto>> UpdateDoctor(DoctorUpdateRequestDto requestModel, Guid doctorId)
        {
            var doctorProfile = await dbContext.Doctors.Include(i => i.User).FirstOrDefaultAsync(x => x.Id == doctorId);

            if (doctorProfile == null)
            {
                return new GlobalResponse<DoctorResponseDto>
                {
                    Status = false,
                    StatusCode = 404,
                    Message = "Doctor's record was not found"
                };
            }

            if (doctorProfile.PhoneNumber != requestModel.PhoneNumber || doctorProfile.Email != requestModel.Email)
            {
                var doctorWithPhone = await dbContext.Doctors.FirstOrDefaultAsync(x => x.PhoneNumber == requestModel.PhoneNumber || x.Email == requestModel.Email);

                if (doctorWithPhone != null)
                    return new GlobalResponse<DoctorResponseDto>
                    {
                        Status = false,
                        StatusCode = 400,
                        Message = doctorProfile.PhoneNumber != requestModel.PhoneNumber?
                        "Phone number is already linked to a Doctor's profile" : "Email is already linked to a Doctor's profile"
                    };
                doctorProfile.User.Username = requestModel.PhoneNumber;
                doctorProfile.User.DateTimeModified = DateTime.UtcNow.AddHours(1);
            }

            doctorProfile.FirstName = requestModel.FirstName;
            doctorProfile.LastName = requestModel.LastName;
            doctorProfile.PhoneNumber = requestModel.PhoneNumber;
            doctorProfile.Email = requestModel.Email;
            doctorProfile.Specialty = requestModel.Specialty;
            doctorProfile.DateTimeModified = DateTime.UtcNow.AddHours(1);

            dbContext.Doctors.Update(doctorProfile);
            await dbContext.SaveChangesAsync();

            return new GlobalResponse<DoctorResponseDto>
            {
                Status = true,
                StatusCode = 200,
                Message = "Doctor updated successfully",
                Data = mapper.Map<DoctorResponseDto>(doctorProfile)
            };
        }
    }
}
