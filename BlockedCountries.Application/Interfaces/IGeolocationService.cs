using BlockedCountries.Application.DTOs.Respones;

namespace BlockedCountries.Application.Interfaces
{
    public interface IGeolocationService
    {
        Task<GeolocationResponseDto?> GetCountryByIpAsync(string ip);
        Task<IpCheckResult> CheckIfIpIsBlockedAsync(string clientIp, string userAgent);

    }
}
