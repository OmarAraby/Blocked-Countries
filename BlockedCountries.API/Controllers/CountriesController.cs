using BlockedCountries.API.DTOs.Requests;
using BlockedCountries.API.DTOs.Respones;
using BlockedCountries.API.Helpers.GeneralResult;
using BlockedCountries.API.Helpers.Pagination;
using BlockedCountries.API.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountries.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly ITemporalBlockService _tempBlockService;
        public CountriesController(ICountryService countryService, ITemporalBlockService tempBlockService)
        {
            _countryService = countryService;
            _tempBlockService = tempBlockService;
        }

        [HttpPost("block")]
        public async Task<Results<Ok<ApiResponse>,BadRequest<ApiResponse>>> BlockCountry( BlockCountryRequestDto requestDto)
        {
            var res = _countryService.BlockCountry(requestDto);
            if(res.Success) 
                return TypedResults.Ok(res);
            else 
                return TypedResults.BadRequest(res);

        }

        [HttpDelete("block/{countryCode}")]
        public async Task<Results<Ok<ApiResponse>,NotFound<ApiResponse>>> UnBlockCountry(string countryCode) { 

            var res =_countryService.UnblockCountry(countryCode);
            if(res.Errors.Contains("Country not found"))
                return TypedResults.NotFound(res);

            return TypedResults.Ok(res);
            
        }

        [HttpGet("blocked")]
        public async Task<Ok<ApiResponse<PageList<BlockedCountryDto>>>> GetBlockedCountries(
        [FromQuery] CountryQueryParams queryParams)
        {
            // Validate query params
            if (queryParams.PageNumber < 1) queryParams.PageNumber = 1;
            if (queryParams.PageSize < 1) queryParams.PageSize = 10;
            if (queryParams.PageSize > 100) queryParams.PageSize = 100;

            var result = _countryService.GetBlockedCountries(queryParams);
            return TypedResults.Ok(result);
        }


        // temp block 
        [HttpPost("temporal-block")]
        public async Task<Results<Ok<ApiResponse>, BadRequest<ApiResponse>, Conflict<ApiResponse>>> TempBlock(TemporalBlockRequestDto requestDto)
        {
            var result = _tempBlockService.BlockTemporarily(requestDto);


            if (!result.Success)
            {
                if (result.Message.Contains("already temporarily blocked"))
                    return TypedResults.Conflict(result);
                else
                    return TypedResults.BadRequest(result); 
            }

            return TypedResults.Ok(result);
        }

    }
}
