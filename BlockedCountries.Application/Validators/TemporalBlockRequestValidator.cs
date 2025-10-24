using BlockedCountries.Application.DTOs.Requests;
using FluentValidation;

namespace BlockedCountries.Application.Validators
{
    public class TemporalBlockRequestValidator : AbstractValidator<TemporalBlockRequestDto>
    {
        public TemporalBlockRequestValidator()
        {
            RuleFor(x => x.CountryCode)
                .NotEmpty().WithMessage("Country code is required.")
                .Length(2).WithMessage("Country code must be exactly 2 characters.")
                .Matches("^[A-Z]{2}$").WithMessage("Country code must be uppercase letters.")
                .NotEqual("XX").WithMessage("Invalid country code.");


            RuleFor(x => x.DurationMinutes)
                .InclusiveBetween(1, 1440)
                .WithMessage("Duration must be between 1 and 1440 minutes (24 hours).");
        }
    }
}
