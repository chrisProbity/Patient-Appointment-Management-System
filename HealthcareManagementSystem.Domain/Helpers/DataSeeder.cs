using AutoMapper;
using HealthcareManagementSystem.Data.DataContext;
using HealthcareManagementSystem.Data.DTOs.Request;
using HealthcareManagementSystem.Data.Models;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;

namespace HealthcareManagementSystem.Domain.Helpers
{
    public class DataSeeder(HealthMgtSystemDbContext dbContext, IPasswordHasher<User> passwordHasher, IMapper mapper)
    {
        public async Task Seed()
        {
            try
            {
                await CreateAdmin();
            }
            catch (Exception)
            {
            }
        }

        public async Task CreateAdmin()
        {
            string username = "Admin";

            bool userExists = await dbContext.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower());

            if (!userExists)
            {
                UserDto userDto = new UserDto
                {
                    Username = username,
                    RoleId = Role.Admin.Id,
                };
                string password = "Password@25";
                User newUser = mapper.Map<User>(userDto);
                newUser.PasswordHash = passwordHasher.HashPassword(newUser, password);

                await dbContext.Users.AddAsync(newUser);
                await dbContext.SaveChangesAsync();
            }
            return;
        }
    }
}
