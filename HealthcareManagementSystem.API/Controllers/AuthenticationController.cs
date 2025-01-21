using HealthcareManagementSystem.Data.DTOs.Request;
using HealthcareManagementSystem.Data.DTOs.Response;
using HealthcareManagementSystem.Data.Validations;
using HealthcareManagementSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HealthcareManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IAuthenticationService authenticationService) : ControllerBase
    {
        [SwaggerOperation(Summary = "Endpoint for Admin, Doctor and Patient to login")]
        [HttpPost("[action]")]
        [ProducesResponseType(typeof(GlobalResponse<LoginResponse>), 200)]
        [ProducesResponseType(typeof(GlobalResponse<LoginResponse>), 400)]
        [ProducesResponseType(typeof(GlobalResponse<LoginResponse>), 500)]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var validation = new LoginRequestValidator().Validate(loginRequest);

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
            var response = await authenticationService.Login(loginRequest);

            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
