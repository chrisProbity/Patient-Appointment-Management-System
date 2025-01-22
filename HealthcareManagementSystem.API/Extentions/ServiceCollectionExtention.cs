using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HealthcareManagementSystem.Data.DataContext;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using HealthcareManagementSystem.Domain.Interfaces;
using HealthcareManagementSystem.Domain.Implementations;
using Microsoft.AspNetCore.Identity;
using HealthcareManagementSystem.Domain.Helpers;

namespace HealthcareManagementSystem.API.Extentions
{
    public static class ServiceCollectionExtention 
    {
        public static IServiceCollection ConfigureSwaggerWithJwtHeader(this IServiceCollection builder)
        {
            builder.AddSwaggerGen(s =>
            {
                s.EnableAnnotations();
                s.SwaggerDoc("v1", new OpenApiInfo { Title = "Patient Appointment Management System", Version = "v1" });
                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Enter bearer token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                s.AddSecurityRequirement(new OpenApiSecurityRequirement(){
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Name = "Bearer"
                    }, new List<string>()
                }});
            });
            return builder;
        }

        public static IServiceCollection ConfigureJwtAuthentication(this IServiceCollection builder, IConfiguration configuration)
        {
            builder.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration.GetValue<string>("JwtSettings:ValidIssuer"),
                ValidAudience = configuration.GetValue<string>("JwtSettings:ValidAudience"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JwtSettings:Secret")!)),
            });
            builder.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("DoctorOnly", policy => policy.RequireRole("Doctor"));
                options.AddPolicy("PatientOnly", policy => policy.RequireRole("Patient"));
            });

            return builder;
        }

        public static IServiceCollection AddDbContext(this IServiceCollection builder, IConfiguration configuration)
        {
            builder.AddDbContext<HealthMgtSystemDbContext>(option => option.UseSqlServer(configuration.GetConnectionString("HealthMgtSystemConnection")));

            return builder;
        }

        public static void ConfigureMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app!.ApplicationServices!.GetService<IServiceScopeFactory>()!.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<HealthMgtSystemDbContext>();
                var container = serviceScope.ServiceProvider.GetService<Container>();
                context!.Database!.Migrate();
                context.Database.EnsureCreated();
                var seeder = serviceScope.ServiceProvider.GetService<DataSeeder>();
                seeder.Seed().Wait();
            }
        }

        public static IServiceCollection RegisterServices(this IServiceCollection builder)
        {
            builder.AddScoped<IPatientService, PatientService>();
            builder.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));
            builder.AddScoped<IDoctorService, DoctorService>();
            builder.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.AddScoped<IAppointmentService, AppointmentService>();
            builder.AddTransient<DataSeeder>();
            return builder;
        }
    }
}
