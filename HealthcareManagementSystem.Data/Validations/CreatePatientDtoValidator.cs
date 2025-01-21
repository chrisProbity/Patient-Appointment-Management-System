using FluentValidation;
using HealthcareManagementSystem.Data.DTOs.Request;

namespace HealthcareManagementSystem.Data.Validations
{
    public class CreatePatientDtoValidator : AbstractValidator<CreatePatientDto>
    {
        public CreatePatientDtoValidator() 
        { 
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("A valid phone number is required");
            RuleFor(x => x.PhoneNumber).Matches("^[0-9]*$").WithMessage("Phone number must be only numbers");
            RuleFor(x => x.PhoneNumber).MaximumLength(11).WithMessage("Phone number must be 11 digits");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(x => x.Password).MinimumLength(8).WithMessage("Password must be at least 8 characters in length");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required");
        }
    }
}
