using BlockedCountries.API.Domain;
using BlockedCountries.API.DTOs.Respones;
using BlockedCountries.API.Helpers.Pagination;
using BlockedCountries.API.Repositories.Interfaces;
using BlockedCountries.API.Services.Interfaces;

namespace BlockedCountries.API.Services.Implementations
{
    public class BlockedAttemptsLogger : IBlockedAttemptsLogger
    {
        private readonly IBlockedAttemptsLogRepository _logRepository;

        public BlockedAttemptsLogger(IBlockedAttemptsLogRepository blockedAttemptsLogRepository)
        {
            _logRepository= blockedAttemptsLogRepository;
        }

        public PageList<BlockedAttemptDto> GetBlockedAttempts(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var all = _logRepository.GetAll()
                .Where(a => a.IsBlocked)
                .OrderByDescending(a => a.Timestamp)
                .ToList();

            var totalCount = all.Count;
            var items = all
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                 .Select(a => new BlockedAttemptDto(
                    a.IpAddress,
                    a.CountryCode,
                    a.Timestamp,
                    a.IsBlocked,
                    a.UserAgent
                ))
                .ToList();

            return new PageList<BlockedAttemptDto>(items, totalCount, pageNumber, pageSize);
        }

        public Task LogAttemptAsync(BlockedAttempt attempt)
        {
            _logRepository.Add(attempt);
            return Task.CompletedTask;
        }

    }
}
