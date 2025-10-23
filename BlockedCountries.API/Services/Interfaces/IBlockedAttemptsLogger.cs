using BlockedCountries.API.Domain;
using BlockedCountries.API.DTOs.Respones;
using BlockedCountries.API.Helpers.Pagination;

namespace BlockedCountries.API.Services.Interfaces
{
    public interface IBlockedAttemptsLogger
    {
        Task LogAttemptAsync(BlockedAttempt attempt);
        PageList<BlockedAttemptDto> GetBlockedAttempts(int pageNumber, int pageSize);


    }
}
