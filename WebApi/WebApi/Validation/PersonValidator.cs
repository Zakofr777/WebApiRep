using System;
using FluentValidation;
using WebApi;
using WebApi.Validation;

namespace WebApi.Validators;

public class PersonValidator : AbstractValidator<Person>
{ 
    public PersonValidator()
    {
        RuleFor(x => x.CreateDate)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("type valid date");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("name should not be empty.")
            .MaximumLength(50).WithMessage(" length should not exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(" last name field should not be empty. ")
            .MaximumLength(50).WithMessage("  length should not exceed 50 characters. ");

        RuleFor(x => x.jobPosition)
            .NotEmpty().WithMessage(" job position should not be empty.")
            .MaximumLength(50).WithMessage("   length should not exceed 50 characters.");

        RuleFor(x => x.Salary)
            .InclusiveBetween(0, 10000).WithMessage(" salary must be between 0 and 10000.");

        RuleFor(x => x.WorkExperince)
            .NotNull().WithMessage(" work experience is required.")
            .GreaterThanOrEqualTo(0).WithMessage(" work experience must be greater than or equal to 0.");

        RuleFor(x => x.PersonAddress)
            .NotNull().WithMessage(" person address is required.")
            .SetValidator(new AddressValidator());
    }
}