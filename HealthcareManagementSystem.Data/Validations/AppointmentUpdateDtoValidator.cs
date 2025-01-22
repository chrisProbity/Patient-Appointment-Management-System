using FluentValidation;
using HealthcareManagementSystem.Data.DTOs.Request;

namespace HealthcareManagementSystem.Data.Validations
{
    public class AppointmentUpdateDtoValidator : AbstractValidator<AppointmentUpdateDto>
    {
        public AppointmentUpdateDtoValidator()
        {
            RuleFor(x => x.AppointmentTime).NotEmpty();
            RuleFor(x => x.AppointmentTime).Must(Helper.IsValidTime).WithMessage("Invalid appointment time format. Use 'hh:mm AM/PM' format. Eg: 12:35 PM");
            RuleFor(x => x.AppointmentDate).Must(Helper.IsValidAppointmentDate).WithMessage("Appointment date cannot be a past date");
        }
    }
}
