using BlockedCountries.Domain.Entities;

namespace BlockedCountries.Domain.Interfaces.Repositories
{
    public interface ITemporalBlocksRepository
    {
        bool Add(TemporalBlock block);
        bool Remove(string countryCode);
        TemporalBlock? GetByCode(string countryCode);
        IReadOnlyList<TemporalBlock> GetAll();
        bool Exists(string countryCode);
        void RemoveExpired();
    }
}
