using BlockedCountries.Application.DTOs.Requests;
using BlockedCountries.Application.Helpers.GeneralResult;

namespace BlockedCountries.Application.Interfaces
{
    public interface ITemporalBlockService
    {
        ApiResponse BlockTemporarily(TemporalBlockRequestDto requestDto);
        ApiResponse Unblock(string countryCode);
        bool IsBlocked(string countryCode);
    }
}
