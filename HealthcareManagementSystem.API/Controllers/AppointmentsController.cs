﻿using HealthcareManagementSystem.Data.DTOs.Request;
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
    public class AppointmentsController(IAppointmentService appointmentService) : ControllerBase
    {
        [SwaggerOperation(Summary = "Endpoint for patient to book an appointment with Doctor")]
        [Authorize(Policy = "PatientOnly")]
        [HttpPost("[action]")]
        [ProducesResponseType(typeof(GlobalResponse<AppointmentResponse>), 200)]    
        [ProducesResponseType(typeof(GlobalResponse<AppointmentResponse>), 400)]    
        [ProducesResponseType(typeof(GlobalResponse<AppointmentResponse>), 500)]    
        public async Task<IActionResult> BookAppointment(AppointmentRequestDto request)
        {
            var validation = new AppointmentRequestDtoValidator().Validate(request);

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

            Guid userId = Guid.Parse(User!.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)!.Value);

            var response = await appointmentService.BookAppointment(request, userId);

            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [SwaggerOperation(Summary = "Endpoint for Doctor to view their schedules/appointments")]
        [Authorize(Policy = "DoctorOnly")]
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(GlobalResponse<DoctorScheduleResponse>), 200)]
        [ProducesResponseType(typeof(GlobalResponse<DoctorScheduleResponse>), 500)]
        public async Task<IActionResult> GetDoctorSchedules()
        {
            Guid userId = Guid.Parse(User!.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)!.Value);

            var response = await appointmentService.GetDoctorSchedules(userId);

            return Ok(response);
        }

        [SwaggerOperation(Summary = "Endpoint for admin to view all appointments")]
        [Authorize(Policy = "Adminonly")]
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(GlobalResponse<AppointmentResponse>), 200)]
        [ProducesResponseType(typeof(GlobalResponse<AppointmentResponse>), 500)]
        public async Task<IActionResult> GetAllAppointments()
        {
            var response = await appointmentService.GetAllAppointments();

            return Ok(response);
        }

        [SwaggerOperation(Summary = "Endpoint for patient to update an appointment")]
        [Authorize(Policy = "PatientOnly")]
        [HttpPut("[action]/{appointmentId:guid}")]
        [ProducesResponseType(typeof(GlobalResponse<AppointmentResponse>), 200)]
        [ProducesResponseType(typeof(GlobalResponse<AppointmentResponse>), 400)]
        [ProducesResponseType(typeof(GlobalResponse<AppointmentResponse>), 404)]
        [ProducesResponseType(typeof(GlobalResponse<AppointmentResponse>), 500)]
        public async Task<IActionResult> UpdateAppointment(AppointmentUpdateDto request, Guid appointmentId)
        {
            var validation = new AppointmentUpdateDtoValidator().Validate(request);

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
            var response = await appointmentService.UpdateAppointment(request, appointmentId);

            if (response.Status)
            {
                return Ok(response);
            }
            if(response.Status && response.StatusCode == 404)
            {
                return NotFound(response);
            }
            return BadRequest(response);
        }

        [SwaggerOperation(Summary = "Endpoint for patient and admin to cancel an appointment")]
        [Authorize(Roles = "Patient,Admin")]
        [HttpDelete("[action]/{appointmentId:guid}")]
        [ProducesResponseType(typeof(GlobalResponse<bool>), 200)]
        [ProducesResponseType(typeof(GlobalResponse<bool>), 400)]
        [ProducesResponseType(typeof(GlobalResponse<bool>), 404)]
        [ProducesResponseType(typeof(GlobalResponse<bool>), 500)]
        public async Task<IActionResult> CancelAppointment(Guid appointmentId)
        {
            var response = await appointmentService.CancelAppointment(appointmentId);

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
    }
}
