using BlockedCountries.API.Domain;
using BlockedCountries.API.DTOs.Respones;
using BlockedCountries.API.Services.Interfaces;
using System.Net;

namespace BlockedCountries.API.Services.Implementations
{
    public class GeolocationService : IGeolocationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GeolocationService> _logger;
        private readonly ICountryService _countryService;
        private readonly ITemporalBlockService _temporalBlockService;
        private readonly IBlockedAttemptsLogger _loggerService;
        private readonly IConfiguration _configuration;



        public GeolocationService(HttpClient httpClient,ILogger<GeolocationService> logger,
            ICountryService countryService,ITemporalBlockService temporalBlockService,
            IBlockedAttemptsLogger blockedAttemptsLogger,IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _countryService = countryService;
            _temporalBlockService = temporalBlockService;
            _loggerService = blockedAttemptsLogger;
            _configuration = configuration;
        }
        public async Task<GeolocationResponseDto?> GetCountryByIpAsync(string ip)
        {

            if (string.IsNullOrWhiteSpace(ip))
                return new GeolocationResponseDto { Error = "IP address is required." };

            if (!System.Net.IPAddress.TryParse(ip, out _))
                return new GeolocationResponseDto { Error = "Invalid IP address format." };

            try
            {

                var apiKey = _configuration["GeolocationApi:ApiKey"];
                var url = $"https://api.ipgeolocation.io/v2/ipgeo?apiKey={apiKey}&ip={ip}";

                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Geolocation API failed for IP {Ip} with status {Status}", ip, response.StatusCode);
                    return new GeolocationResponseDto { Error = $"Geolocation API error: {response.StatusCode}" };
                }

                var json = await response.Content.ReadAsStringAsync();
                var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = System.Text.Json.JsonSerializer.Deserialize<GeolocationResponseDto>(json, options);
                return result ?? new GeolocationResponseDto { Error = "Empty response." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Looking up IP {ip}", ip);
                return new GeolocationResponseDto { Error = "Failed to fetch geolocation data." };
            }
        }


        public async Task<IpCheckResult> CheckIfIpIsBlockedAsync(string clientIp, string userAgent)
        {
            var geo = await GetCountryByIpAsync(clientIp);
            if (!string.IsNullOrEmpty(geo.Error))
                return new IpCheckResult(clientIp, "", null, false, null, geo.Error);
            if (string.IsNullOrEmpty(geo.Location.CountryCode2))
                return new IpCheckResult(clientIp, "", null, false, null, "Could not determine country.");

            var countryCode = geo.Location.CountryCode2.ToUpperInvariant();

            var isPermanentlyBlocked = _countryService.IsCountryBlocked(countryCode);
            var isTemporarilyBlocked = _temporalBlockService.IsBlocked(countryCode);
            var isBlocked = isPermanentlyBlocked || isTemporarilyBlocked;


            // logging attemp
            await _loggerService.LogAttemptAsync(new BlockedAttempt
            {
                IpAddress = clientIp,
                CountryCode = countryCode,
                Timestamp = DateTime.UtcNow,
                IsBlocked = isBlocked,
                UserAgent = userAgent
            });

            var reason = isPermanentlyBlocked ? "Permanent" : isTemporarilyBlocked ? "Temporary" : null;
            return new IpCheckResult(clientIp, countryCode, geo.Location.CountryName, isBlocked, reason);
        }



    }
}
