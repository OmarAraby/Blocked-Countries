namespace BlockedCountries.API.DTOs.Respones
{
    public record BlockedAttemptDto(string IpAddress, string CountryCode, DateTime AttemptedAt, bool BlockedStatus, string UserAgent);

}
