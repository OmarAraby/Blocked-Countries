using BlockedCountries.API.Domain;

namespace BlockedCountries.API.Repositories.Interfaces
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
