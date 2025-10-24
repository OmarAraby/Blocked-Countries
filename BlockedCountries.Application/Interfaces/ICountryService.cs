using BlockedCountries.Application.DTOs.Requests;
using BlockedCountries.Application.DTOs.Respones;
using BlockedCountries.Application.Helpers.GeneralResult;
using BlockedCountries.Application.Helpers.Pagination;

namespace BlockedCountries.Application.Interfaces
{
    public interface ICountryService
    {
        ApiResponse BlockCountry(BlockCountryRequestDto request);
        ApiResponse UnblockCountry(string countryCode);
        ApiResponse<PageList<BlockedCountryDto>> GetBlockedCountries(CountryQueryParams queryParams);
        bool IsCountryBlocked(string countryCode);

    }
}
