using HealthcareManagementSystem.Data.DTOs.Request;
using HealthcareManagementSystem.Data.DTOs.Response;
using HealthcareManagementSystem.Data.Validations;
using HealthcareManagementSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace HealthcareManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController(IDoctorService doctorService) : ControllerBase
    {
        [SwaggerOperation(Summary = "Endpoint to create/onboard Doctor")]
        [HttpPost("[action]")]
        [ProducesResponseType(typeof(GlobalResponse<DoctorResponseDto>), 200)]
        [ProducesResponseType(typeof(GlobalResponse<DoctorResponseDto>), 400)]
        [ProducesResponseType(typeof(GlobalResponse<DoctorResponseDto>), 500)]
        public async Task<IActionResult> CreateDoctor(CreateDoctorDto model)
        {
            var validation = new CreateDoctorDtoValidator().Validate(model);

            if (!validation.IsValid)
            {
                var errorResponse = new GlobalResponse<string>
                {
                    Status = false,
                    StatusCode = 400,
                    Message = string.Join(",", validation.Errors.Select(s => s.ErrorMessage).ToList())
                };
                return BadRequest(errorResponse);
            }

            var response = await doctorService.CreateDoctor(model);

            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [SwaggerOperation(Summary = "Endpoint for Doctor to update their profile")]
        [Authorize(Policy = "DoctorOnly")]
        [HttpPut("[action]/{doctorId}")]
        [ProducesResponseType(typeof(GlobalResponse<DoctorResponseDto>), 200)]
        [ProducesResponseType(typeof(GlobalResponse<DoctorResponseDto>), 400)]
        [ProducesResponseType(typeof(GlobalResponse<DoctorResponseDto>), 404)]
        [ProducesResponseType(typeof(GlobalResponse<DoctorResponseDto>), 500)]
        public async Task<IActionResult> UpdateDoctor(DoctorUpdateRequestDto requestModel, Guid doctorId)
        {
            var validation = new DoctorUpdateRequestDtoValidator().Validate(requestModel);

            if (!validation.IsValid)
            {
                var errorResponse = new GlobalResponse<string>
                {
                    Status = false,
                    StatusCode = 400,
                    Message = string.Join(",", validation.Errors.Select(s => s.ErrorMessage).ToList())
                };
                return BadRequest(errorResponse);
            }

            var response = await doctorService.UpdateDoctor(requestModel, doctorId);

            if (response.Status)
            {
                return Ok(response);
            }
            if (response.Status && response.StatusCode == 404)
            {
                return NotFound(response);
            }
            return BadRequest(response);

        }

        [SwaggerOperation(Summary = "Endpoint for Doctor to view their profile")]
        [Authorize(Policy = "DoctorOnly")]
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(GlobalResponse<DoctorResponseDto>), 200)]
        [ProducesResponseType(typeof(GlobalResponse<DoctorResponseDto>), 404)]
        [ProducesResponseType(typeof(GlobalResponse<DoctorResponseDto>), 500)]
        public async Task<IActionResult> GetDoctor()
        {
            Guid doctorId = Guid.Parse(User!.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)!.Value);

            var response = await doctorService.GetDoctor(doctorId);

            if (response.Status)
            {
                return Ok(response);
            }
            if (response.Status && response.StatusCode == 404)
            {
                return NotFound(response);
            }
            return BadRequest(response);
        }

        [SwaggerOperation(Summary = "Endpoint for admin and patient to view all Doctors")]
        [Authorize(Roles = "Admin,Patient")]
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(GlobalResponse<DoctorResponseDto>), 200)]
        [ProducesResponseType(typeof(GlobalResponse<DoctorResponseDto>), 500)]
        public async Task<IActionResult> GetAllDoctors()
        {
            var response = await doctorService.GetAllDoctors();

            return Ok(response);
        }

        [SwaggerOperation(Summary = "Endpoint for Doctor to create their available period")]
        [Authorize(Policy = "DoctorOnly")]
        [HttpPost("[action]")]
        [ProducesResponseType(typeof(GlobalResponse<DoctorAvailabilityResponse>), 200)]
        [ProducesResponseType(typeof(GlobalResponse<DoctorAvailabilityResponse>), 400)]
        [ProducesResponseType(typeof(GlobalResponse<DoctorAvailabilityResponse>), 500)]
        public async Task<IActionResult> CreateDoctorAvailablePeriod(DoctorAvailablePeriod model)
        {
            var validation = new DoctorAvailablePeriodValidator().Validate(model);

            if (!validation.IsValid)
            {
                var errorResponse = new GlobalResponse<string>
                {
                    Status = false,
                    StatusCode = 400,
                    Message = string.Join(",", validation.Errors.Select(s => s.ErrorMessage).ToList())
                };
                return BadRequest(errorResponse);
            }

            Guid doctorId = Guid.Parse(User!.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)!.Value);

            var response = await doctorService.CreateAvailablePeriod(model, doctorId);

            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
