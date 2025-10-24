
using BlockedCountries.Application.DTOs.Respones;
using BlockedCountries.Application.Helpers.GeneralResult;
using BlockedCountries.Application.Helpers.Pagination;
using BlockedCountries.Application.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountries.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly IBlockedAttemptsLogger _blockedAttemptsLogger;

        public LogsController(IBlockedAttemptsLogger blockedAttemptsLogger)
        {
            _blockedAttemptsLogger = blockedAttemptsLogger;
        }

        [HttpGet("blocked-attemps")]
        public async Task<Ok<ApiResponse<PageList<BlockedAttemptDto>>>> GetBlockedAttempts(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var pagedResult = _blockedAttemptsLogger.GetBlockedAttempts(page, pageSize);
            var response = ApiResponse<PageList<BlockedAttemptDto>>.SuccessResponse(pagedResult);
            return TypedResults.Ok(response);
        }
    }
}
