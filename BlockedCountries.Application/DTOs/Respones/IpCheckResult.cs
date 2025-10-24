namespace BlockedCountries.Application.DTOs.Respones
{
    public record IpCheckResult(
    string Ip,
    string CountryCode,
    string? CountryName,
    bool IsBlocked,
    string? BlockedReason,
    string? Error = null);
}
