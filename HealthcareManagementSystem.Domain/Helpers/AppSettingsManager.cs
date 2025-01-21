using Microsoft.Extensions.Configuration;

namespace HealthcareManagementSystem.Domain.Helpers
{
    public class AppSettingsManager
    {
        public static IConfiguration configuration;
        public static string FetchJwtConfig(string configName)
        {
            return configuration[$"JwtSettings:{configName}"];
        }
    }
}
