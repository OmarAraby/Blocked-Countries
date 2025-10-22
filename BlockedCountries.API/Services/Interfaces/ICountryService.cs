using BlockedCountries.API.DTOs.Requests;
using BlockedCountries.API.DTOs.Respones;
using BlockedCountries.API.Helpers.GeneralResult;
using BlockedCountries.API.Helpers.Pagination;

namespace BlockedCountries.API.Services.Interfaces
{
    public interface ICountryService
    {
        ApiResponse BlockCountry(BlockCountryRequestDto request);
        ApiResponse UnblockCountry(string countryCode);
        ApiResponse<PageList<BlockedCountryDto>> GetBlockedCountries(CountryQueryParams queryParams);

    }
}
