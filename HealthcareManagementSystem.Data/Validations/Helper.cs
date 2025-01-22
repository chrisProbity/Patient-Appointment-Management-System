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

        public static bool IsValidDayOfWeek(string day)
        {
            if(string.IsNullOrEmpty(day))
                return false;

            List<string> daysOfWeek = new List<string>() {"MONDAY","TUESDAY","WEDNESDAY",
                "THURSDAY","FRIDAY","SATURDAY","SUNDAY"};

            if (daysOfWeek.Contains(day.ToUpper()))
                return true;
            return false;
        }
        public static bool IsValidAppointmentDate(DateTime inputDate)
        {
            DateTime currentDate = DateTime.UtcNow.AddHours(1);

            if(inputDate.Date < currentDate.Date)
                return false;
            return true;
        }

        public static string DayofWeekToString(DayOfWeek dayOfWeek)
        {
            string day = string.Empty;
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    day = "Sunday";
                    break;
                case DayOfWeek.Monday:
                    day = "Monday";
                    break;
                case DayOfWeek.Tuesday:
                    day = "Tuesday";
                    break;
                case DayOfWeek.Wednesday:
                    day = "Wednesday";
                    break;
                case DayOfWeek.Thursday:
                    day = "Thursday";
                    break;
                case DayOfWeek.Friday:
                    day = "Friday";
                    break;
                case DayOfWeek.Saturday:
                    day = "Saturday";
                    break;
                default:
                    break;
            }
            return day;
        }
    }
}
