using BlockedCountries.Domain.Entities;

namespace BlockedCountries.Domain.Interfaces.Repositories
{
    public interface IBlockedAttemptsLogRepository
    {
        void Add(BlockedAttempt attempt);
        IReadOnlyList<BlockedAttempt> GetAll();
    }
}
