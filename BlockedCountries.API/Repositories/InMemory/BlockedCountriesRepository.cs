using BlockedCountries.API.Domain;
using BlockedCountries.API.Repositories.Interfaces;
using System.Collections.Concurrent;

namespace BlockedCountries.API.Repositories.InMemory
{
    public class BlockedCountriesRepository : IBlockedCountriesRepository
    {
        private readonly ConcurrentDictionary<string,BlockedCountry> _blockCountries = new(StringComparer.OrdinalIgnoreCase);
        public bool Add(BlockedCountry country)
        {
            return _blockCountries.TryAdd(country.CountryCode, country);
        }

        public bool Exists(string countryCode)
        {
            return _blockCountries.ContainsKey(countryCode);
        }

        public IReadOnlyList<BlockedCountry> GetAll()
        {
            //throw new NotImplementedException();
            return _blockCountries.Values.ToList().AsReadOnly();
        }

        public BlockedCountry GetByCode(string countryCode)
        {
            _blockCountries.TryGetValue(countryCode, out var country);
            return country;
        }

        public bool Remove(string countryCode)
        {
            return  _blockCountries.TryRemove(countryCode, out _);
        }
    }
}
