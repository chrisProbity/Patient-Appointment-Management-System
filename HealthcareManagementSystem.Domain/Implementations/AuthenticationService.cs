using HealthcareManagementSystem.Data.DataContext;
using HealthcareManagementSystem.Data.DTOs.Request;
using HealthcareManagementSystem.Data.DTOs.Response;
using HealthcareManagementSystem.Data.Models;
using HealthcareManagementSystem.Domain.Helpers;
using HealthcareManagementSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthcareManagementSystem.Domain.Implementations
{
    public class AuthenticationService(HealthMgtSystemDbContext dbContext, IPasswordHasher<User> passwordHasher) : IAuthenticationService
    {
        public async Task<GlobalResponse<LoginResponse>> Login(LoginRequest request)
        {
            var user = await dbContext.Users.Include(i => i.Role).FirstOrDefaultAsync(x => x.Username == request.Username);

            if (user == null)
            {
                return new GlobalResponse<LoginResponse>
                {
                    Status = false,
                    StatusCode = 400,
                    Message = "Incorrect username/password"
                };
            }

            var verifyPasswordResponse = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (verifyPasswordResponse == PasswordVerificationResult.Failed)
            {
                return new GlobalResponse<LoginResponse>
                {
                    Status = false,
                    StatusCode = 400,
                    Message = "Incorrect username/password"
                };
            }

            string jwtAccessToken = JwtHelper.GenerateJwtToken(user);

            return new GlobalResponse<LoginResponse>
            {
                Status = true,
                StatusCode = 200,
                Message = "Logged in successfully",
                Data = new LoginResponse
                {
                    UserId = user.Id,
                    PhoneNumber = user.Username,
                    AccessToken = jwtAccessToken,
                }
            };
        }
    }
}
