using FluentValidation;
using HealthcareManagementSystem.Data.DTOs.Request;

namespace HealthcareManagementSystem.Data.Validations
{
    public class DoctorUpdateRequestDtoValidator : AbstractValidator<DoctorUpdateRequestDto>
    {
        public DoctorUpdateRequestDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("A valid phone number is required");
            RuleFor(x => x.PhoneNumber).Matches("^[0-9]*$").WithMessage("Phone number must be only numbers");
            RuleFor(x => x.PhoneNumber).MaximumLength(11).WithMessage("Phone number must be 11 digits");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required");
            RuleFor(x => x.Specialty).NotEmpty().WithMessage("Area of specialization is required");
        }
    }
}
