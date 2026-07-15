using FluentValidation;

namespace WebApi.Validation;

internal sealed  class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(address => address.Country)
            .NotEmpty().WithMessage("Country is required");
        
        RuleFor(address => address.City)
            .NotEmpty().WithMessage("Please specify city");
        
        RuleFor(address => address.HomeNumber)
            .NotEmpty().WithMessage("Please specify home number");
    }
    
}