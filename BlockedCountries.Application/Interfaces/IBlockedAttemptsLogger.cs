using BlockedCountries.Application.DTOs.Respones;
using BlockedCountries.Application.Helpers.Pagination;
using BlockedCountries.Domain.Entities;

namespace BlockedCountries.Application.Interfaces
{
    public interface IBlockedAttemptsLogger
    {
        Task LogAttemptAsync(BlockedAttempt attempt);
        PageList<BlockedAttemptDto> GetBlockedAttempts(int pageNumber, int pageSize);


    }
}
