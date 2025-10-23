using BlockedCountries.API.Domain;
using BlockedCountries.API.Repositories.Interfaces;
using System.Collections.Concurrent;

namespace BlockedCountries.API.Repositories.InMemory
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
