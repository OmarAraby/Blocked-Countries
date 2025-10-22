using BlockedCountries.API.Domain;
using BlockedCountries.API.DTOs.Requests;
using BlockedCountries.API.DTOs.Respones;
using BlockedCountries.API.Helpers.GeneralResult;
using BlockedCountries.API.Helpers.Pagination;
using BlockedCountries.API.Repositories.Interfaces;
using BlockedCountries.API.Services.Interfaces;

namespace BlockedCountries.API.Services.Implementations
{
    public class CountryService : ICountryService
    {
        private readonly IBlockedCountriesRepository _repo;

        public CountryService(IBlockedCountriesRepository repo)
        {
            _repo = repo;
        }
        public ApiResponse BlockCountry(BlockCountryRequestDto request)
        {
            if (_repo.Exists(request.CountryCode))
                return ApiResponse.ErrorResponse("Country is already blocked.");

            var country = new BlockedCountry { CountryCode = request.CountryCode };
            _repo.Add(country);
            return ApiResponse.SuccessResponse("Country blocked successfully");
        }
        public ApiResponse<PageList<BlockedCountryDto>> GetBlockedCountries(CountryQueryParams queryParams)
        {
            var all = _repo.GetAll().AsQueryable();


            // search
            if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
            {
                var term = queryParams.SearchTerm.ToUpperInvariant();
                all = all.Where(c => c.CountryCode.Contains(term));
            }

            // Sort
            all = queryParams.SortBy?.ToLower() switch
            {
                "blockedat" => queryParams.SortDescending
                    ? all.OrderByDescending(c => c.BlockedAt)
                    : all.OrderBy(c => c.BlockedAt),
                _ => all.OrderBy(c => c.BlockedAt)
            };

            var totalCount = all.Count();
            var items = all
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .Select(c => new BlockedCountryDto(c.CountryCode, c.BlockedAt))
                .ToList();

            var paged = new PageList<BlockedCountryDto>(
                items,
                totalCount,
                queryParams.PageNumber,
                queryParams.PageSize
            );

            return ApiResponse<PageList<BlockedCountryDto>>.SuccessResponse(paged);
        }

        public ApiResponse UnblockCountry(string countryCode)
        {
            //throw new NotImplementedException();
            if (!_repo.Exists(countryCode))
                return ApiResponse.ErrorResponse("contry not found");

            _repo.Remove(countryCode);
            return ApiResponse.SuccessResponse("Country unBlocked Successfully");
        }
    }
}
