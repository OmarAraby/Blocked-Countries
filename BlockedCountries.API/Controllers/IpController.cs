using BlockedCountries.API.DTOs.Respones;
using BlockedCountries.API.Helpers;
using BlockedCountries.API.Helpers.GeneralResult;
using BlockedCountries.API.Repositories.Interfaces;
using BlockedCountries.API.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountries.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class IpController : ControllerBase
    {
        private readonly IGeolocationService _geoService;
        private readonly ILogger<IpController> _logger;


        public IpController(
            IGeolocationService geoService,
            ILogger<IpController> logger)
        {
            _geoService = geoService;
            _logger = logger;
        }   


        [HttpGet("lookup")]
        public async Task<Results<Ok<ApiResponse<GeolocationResponseDto>>, BadRequest<ApiResponse>>> Lookup([FromQuery] string? ipAddress)
        {
            var ip = string.IsNullOrWhiteSpace(ipAddress) ? await IpAddressHelper.GetClientIpAddressAsync(HttpContext) : ipAddress.Trim();

            var geo = await _geoService.GetCountryByIpAsync(ip);

            if (!string.IsNullOrEmpty(geo.Error))
                return TypedResults.BadRequest(ApiResponse.ErrorResponse(geo.Error));

            var response = new GeolocationResponseDto
            {
                Ip = geo.Ip,
                Location = geo.Location
            };

            return TypedResults.Ok(ApiResponse<GeolocationResponseDto>.SuccessResponse(response));
        }

        [HttpGet("check-block")]
        public async Task<Results<Ok<ApiResponse<IpCheckResult>>, BadRequest<ApiResponse>>> CheckBlock()
        {
            var clientIp = await IpAddressHelper.GetClientIpAddressAsync(HttpContext);
            var userAgent = Request.Headers["User-Agent"].ToString() ?? "Unknown";

            var result = await _geoService.CheckIfIpIsBlockedAsync(clientIp, userAgent);

            if (!string.IsNullOrEmpty(result.Error))
                return TypedResults.BadRequest(ApiResponse.ErrorResponse(result.Error));


            return TypedResults.Ok(ApiResponse<IpCheckResult>.SuccessResponse(result));
        }


    }
}
