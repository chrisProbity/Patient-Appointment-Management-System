using System.Globalization;

namespace HealthcareManagementSystem.Data.Validations
{
    public static class Helper
    {
        public static bool IsValidTime(string time)
        {
            if (string.IsNullOrEmpty(time))
                return false;

            if (!DateTime.TryParseExact(time, "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime doctorStartTime))
                return false;
            return true;
        }
    }
}
