using BlockedCountries.API.DTOs.Requests;
using BlockedCountries.API.Helpers.GeneralResult;

namespace BlockedCountries.API.Services.Interfaces
{
    public interface ITemporalBlockService
    {
        ApiResponse BlockTemporarily(TemporalBlockRequestDto requestDto);
        ApiResponse Unblock(string countryCode);
        bool IsBlocked(string countryCode);
    }
}
