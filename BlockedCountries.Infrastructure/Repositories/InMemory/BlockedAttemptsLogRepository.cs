using BlockedCountries.Domain.Entities;
using BlockedCountries.Domain.Interfaces.Repositories;
using System.Collections.Concurrent;

namespace BlockedCountries.Infrastructure.Repositories.InMemory
{
    public class BlockedAttemptsLogRepository : IBlockedAttemptsLogRepository
    {
        private readonly ConcurrentBag<BlockedAttempt> _blockedAttemps = new();
        public void Add(BlockedAttempt attempt)
        {
            _blockedAttemps.Add(attempt);
            
        }

        public IReadOnlyList<BlockedAttempt> GetAll()
        {
            return _blockedAttemps.OrderByDescending(a=>a.Timestamp).ToList();
        }
    }
}
