using BlockedCountries.Domain.Entities;

namespace BlockedCountries.Domain.Interfaces.Repositories
{
    public interface IBlockedCountriesRepository
    {
        bool Add(BlockedCountry country); 
        bool Remove(string countryCode);
        BlockedCountry GetByCode(string countryCode);
        IReadOnlyList<BlockedCountry> GetAll();
        bool Exists(string countryCode);
    }
}
