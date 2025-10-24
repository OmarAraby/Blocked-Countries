using BlockedCountries.Application.DTOs.Requests;
using FluentValidation;

namespace BlockedCountries.Application.Validators
{
    public class BlockCountryRequestValidator : AbstractValidator<BlockCountryRequestDto>
    {
        public BlockCountryRequestValidator()
        {
            RuleFor(x => x.CountryCode)
                .NotEmpty().WithMessage("Country code is required.")
                .Length(2).WithMessage("Country code must be exactly 2 characters.")
                .Matches("^[A-Z]{2}$").WithMessage("Country code must be uppercase letters.")
                .NotEqual("XX").WithMessage("Invalid country code.");
        }
    }
}
