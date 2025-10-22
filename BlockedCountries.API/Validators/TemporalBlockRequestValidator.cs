using BlockedCountries.API.DTOs.Requests;
using FluentValidation;

namespace BlockedCountries.API.Validators
{
    public class TemporalBlockRequestValidator : AbstractValidator<TemporalBlockRequestDto>
    {
        public TemporalBlockRequestValidator()
        {
            RuleFor(x => x.CountryCode)
                .NotEmpty()
                .Length(2)
                .Matches("^[A-Z]{2}$")
                .NotEqual("XX");

            RuleFor(x => x.DurationMinutes)
                .InclusiveBetween(1, 1440)
                .WithMessage("Duration must be between 1 and 1440 minutes (24 hours).");
        }
    }
}
