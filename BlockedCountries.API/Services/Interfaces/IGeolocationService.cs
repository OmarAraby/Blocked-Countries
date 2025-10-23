using BlockedCountries.API.DTOs.Respones;

namespace BlockedCountries.API.Services.Interfaces
{
    public interface IGeolocationService
    {
        Task<GeolocationResponseDto?> GetCountryByIpAsync(string ip);
        Task<IpCheckResult> CheckIfIpIsBlockedAsync(string clientIp, string userAgent);

    }
}
