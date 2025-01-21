﻿using FluentValidation;
using HealthcareManagementSystem.Data.DTOs.Request;

namespace HealthcareManagementSystem.Data.Validations
{
    public class DoctorAvailablePeriodValidator : AbstractValidator<DoctorAvailablePeriod>
    {
        public DoctorAvailablePeriodValidator()
        {
            RuleFor(x => x.Day).NotEmpty().WithMessage("Day is required");
            RuleFor(x => x.ResumptionTime).NotEmpty().WithMessage("Resumption time is required");
            RuleFor(x => x.ResumptionTime).Must(Helper.IsValidTime).WithMessage("Invalid time format. Use 'hh:mm AM/PM' format.");
            RuleFor(x => x.ClosingTime).NotEmpty().WithMessage("Closing time is required");
            RuleFor(x => x.ClosingTime).Must(Helper.IsValidTime).WithMessage("Invalid time format. Use 'hh:mm AM/PM' format.");
        }
    }
}
