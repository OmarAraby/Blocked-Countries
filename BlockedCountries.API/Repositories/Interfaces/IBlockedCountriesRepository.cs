using BlockedCountries.API.Domain;

namespace BlockedCountries.API.Repositories.Interfaces
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
