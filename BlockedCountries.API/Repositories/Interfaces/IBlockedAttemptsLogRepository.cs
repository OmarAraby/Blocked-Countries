using BlockedCountries.API.Domain;

namespace BlockedCountries.API.Repositories.Interfaces
{
    public interface IBlockedAttemptsLogRepository
    {
        void Add(BlockedAttempt attempt);
        IReadOnlyList<BlockedAttempt> GetAll();
    }
}
