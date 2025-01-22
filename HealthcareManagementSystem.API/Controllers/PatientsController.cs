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
    public class PatientsController(IPatientService patientService) : ControllerBase
    {
        [SwaggerOperation(Summary = "Endpoint to create/onboard patient")]
        [HttpPost("[action]")]
        [ProducesResponseType(typeof(GlobalResponse<CreatePatientResponseDto>), 200)]
        [ProducesResponseType(typeof(GlobalResponse<CreatePatientResponseDto>), 400)]
        [ProducesResponseType(typeof(GlobalResponse<CreatePatientResponseDto>), 500)]
        public async Task<IActionResult> CreatePatient(CreatePatientDto model)
        {
            var validation = new CreatePatientDtoValidator().Validate(model);

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

            var response = await patientService.CreatePatient(model);

            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [SwaggerOperation(Summary = "Endpoint for patient's to update their profile")]
        [Authorize(Policy = "PatientOnly")]
        [HttpPut("[action]/{patientId}")]
        [ProducesResponseType(typeof(GlobalResponse<CreatePatientResponseDto>), 200)]
        [ProducesResponseType(typeof(GlobalResponse<CreatePatientResponseDto>), 400)]
        [ProducesResponseType(typeof(GlobalResponse<CreatePatientResponseDto>), 404)]
        [ProducesResponseType(typeof(GlobalResponse<CreatePatientResponseDto>), 500)]
        public async Task<IActionResult> UpdatePatient(PatientUpdateRequestDto requestModel, Guid patientId)
        {
            var validation = new PatientUpdateRequestDtoValidator().Validate(requestModel);

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

            var response = await patientService.UpdatePatient(requestModel, patientId);

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

        [SwaggerOperation(Summary = "Endpoint for patient to view their profile")]
        [Authorize(Policy = "PatientOnly")]
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(GlobalResponse<CreatePatientResponseDto>), 200)]
        [ProducesResponseType(typeof(GlobalResponse<CreatePatientResponseDto>), 404)]
        [ProducesResponseType(typeof(GlobalResponse<CreatePatientResponseDto>), 500)]
        public async Task<IActionResult> GetPatient()
        {
            Guid patientId = Guid.Parse(User!.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)!.Value);

            var response = await patientService.GetPatient(patientId);

            if(response.Status)
            {
                return Ok(response);
            }
            if (response.Status && response.StatusCode == 404)
            {
                return NotFound(response);
            }
            return BadRequest(response);
        }

        [SwaggerOperation(Summary = "Endpoint for admin to view all patients")]
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(GlobalResponse<CreatePatientResponseDto>), 200)]
        [ProducesResponseType(typeof(GlobalResponse<CreatePatientResponseDto>), 500)]
        public async Task<IActionResult> GetAllPatients()
        {
            var response = await patientService.GetAllPatients();

            return Ok(response);
        }
    }
}
