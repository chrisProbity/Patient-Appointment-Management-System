using FluentValidation;
using HealthcareManagementSystem.Data.DTOs.Request;

namespace HealthcareManagementSystem.Data.Validations
{
    public class DoctorAvailablePeriodValidator : AbstractValidator<DoctorAvailablePeriod>
    {
        public DoctorAvailablePeriodValidator()
        {
            RuleFor(x => x.Day).NotEmpty().WithMessage("Day is required");
            RuleFor(x => x.Day).Must(Helper.IsValidDayOfWeek).WithMessage("Enter a valid day of the week. Eg: Monday");
            RuleFor(x => x.ResumptionTime).NotEmpty().WithMessage("Resumption time is required");
            RuleFor(x => x.ResumptionTime).Must(Helper.IsValidTime).WithMessage("Invalid time format. Use 'hh:mm AM/PM' format. Eg: 09:00 AM");
            RuleFor(x => x.ClosingTime).NotEmpty().WithMessage("Closing time is required");
            RuleFor(x => x.ClosingTime).Must(Helper.IsValidTime).WithMessage("Invalid time format. Use 'hh:mm AM/PM' format.");
        }
    }
}
