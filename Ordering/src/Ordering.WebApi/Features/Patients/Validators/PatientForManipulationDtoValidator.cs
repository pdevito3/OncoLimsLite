namespace Ordering.WebApi.Features.Patients.Validators
{
    using Ordering.Core.Dtos.Patient;
    using FluentValidation;
    using System;

    public class PatientForManipulationDtoValidator<T> : AbstractValidator<T> where T : PatientForManipulationDto
    {
        public PatientForManipulationDtoValidator()
        {
            RuleFor(p => p.FirstName).NotEmpty().WithMessage("Please specify a valid first name.");
            //RuleFor(p => p.LastName).NotEmpty().WithMessage("Please specify a valid last name.");
        }
    }
}