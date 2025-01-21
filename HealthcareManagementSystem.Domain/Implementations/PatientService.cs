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
    public class PatientService(HealthMgtSystemDbContext dbContext, IMapper mapper, IPasswordHasher<User> passwordHasher) : IPatientService
    {
        public async Task<GlobalResponse<CreatePatientResponseDto>> CreatePatient(CreatePatientDto createPatientDto)
        {
            bool patientExists = await dbContext.Users.AnyAsync(x => x.Username == createPatientDto.PhoneNumber);

            if (patientExists)
            {
                return new GlobalResponse<CreatePatientResponseDto>
                {
                    Status = false,
                    StatusCode = 400,
                    Message = "Phone number has already been used"
                };
            }

            //Create new instance of User and Patient objects
            Patient newPatient = mapper.Map<Patient>(createPatientDto);
            newPatient.MRN = generateMedicalRecordNumber(newPatient.PhoneNumber);
            User user = mapper.Map<User>(createPatientDto);
            user.PasswordHash = passwordHasher.HashPassword(user, createPatientDto.Password);
            user.RoleId = Role.Patient.Id;
            user.Patient = newPatient;

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var patientDetails = mapper.Map<CreatePatientResponseDto>(newPatient);

            return new GlobalResponse<CreatePatientResponseDto>
            {
                Status = true,
                StatusCode = 200,
                Message = "Patient created successfully.",
                Data = patientDetails,
                
            };
        }

        public async Task<GlobalResponse<List<CreatePatientResponseDto>>> GetAllPatients()
        {
            var patients = await dbContext.Patients.OrderByDescending(x => x.DateTimeCreated).Select(s => new CreatePatientResponseDto 
            {
                Id = s.Id,
                MedicalRecordNumber = s.MRN,
                FirstName = s.FirstName,
                LastName = s.LastName,
                PhoneNumber = s.PhoneNumber,
                DateCreated = s.DateTimeCreated
            }).ToListAsync();

            return new GlobalResponse<List<CreatePatientResponseDto>>
            {
                Status = true,
                StatusCode = 200,
                Message = patients.Count > 0 ? "Successfully fetched patients" : "No patients available.",
                Data = patients
            };
        }

        public async Task<GlobalResponse<CreatePatientResponseDto>> GetPatient(Guid patientId)
        {
            var patient = await dbContext.Patients.FirstOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return new GlobalResponse<CreatePatientResponseDto>
                {
                    Status = false,
                    StatusCode = 404,
                    Message = "Patient record was not found"
                };
            }

            return new GlobalResponse<CreatePatientResponseDto>
            {
                Status = true,
                StatusCode = 200,
                Message = "Patient's record was fetched successful",
                Data = mapper.Map<CreatePatientResponseDto>(patient)
            };

        }

        public async Task<GlobalResponse<CreatePatientResponseDto>> UpdatePatient(PatientUpdateRequestDto requestModel, Guid patientId)
        {
            var patientProfile = await dbContext.Patients.Include(i => i.User).FirstOrDefaultAsync(x => x.Id == patientId);

            if (patientProfile == null)
            {
                return new GlobalResponse<CreatePatientResponseDto>
                {
                    Status = false,
                    StatusCode = 404,
                    Message = "Patient record was not found"
                };
            }

            if (patientProfile.PhoneNumber != requestModel.PhoneNumber)
            {
                var patientWithPhoneExists = await dbContext.Patients.AnyAsync(x => x.PhoneNumber == requestModel.PhoneNumber);

                if(patientWithPhoneExists)
                    return new GlobalResponse<CreatePatientResponseDto>
                    {
                        Status = false,
                        StatusCode = 400,
                        Message = "Phone number is already linked to a patient's profile"
                    };
                patientProfile.User.Username = requestModel.PhoneNumber;
                patientProfile.User.DateTimeModified = DateTime.UtcNow.AddHours(1);
            }

           patientProfile.FirstName = requestModel.FirstName;
           patientProfile.LastName = requestModel.LastName;
           patientProfile.PhoneNumber = requestModel.PhoneNumber;
           patientProfile.DateTimeModified = DateTime.UtcNow.AddHours(1);

            dbContext.Patients.Update(patientProfile);
            await dbContext.SaveChangesAsync();

            return new GlobalResponse<CreatePatientResponseDto>
            {
                Status = true,
                StatusCode = 200,
                Message = "Patient updated successfully",
                Data = mapper.Map<CreatePatientResponseDto>(patientProfile)
            };
        }

        private string generateMedicalRecordNumber(string phoneNumber)
        {
            return phoneNumber.Substring(5);
        }
    }
}
