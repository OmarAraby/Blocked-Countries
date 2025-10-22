using BlockedCountries.API.Domain;
using BlockedCountries.API.Repositories.Interfaces;
using System.Collections.Concurrent;

namespace BlockedCountries.API.Repositories.InMemory
{
    public class TemporalBlocksRepository : ITemporalBlocksRepository
    {
        private readonly ConcurrentDictionary<string, TemporalBlock> _tempBlock = new (StringComparer.OrdinalIgnoreCase);
        
        public bool Add(TemporalBlock block)
        {
            return _tempBlock.TryAdd(block.CountryCode, block);
        }

        public bool Exists(string countryCode)
        {
            if (_tempBlock.TryGetValue(countryCode, out var block))
                return !block.IsExpired;

            return false;
        }

        public IReadOnlyList<TemporalBlock> GetAll()
        {
            return _tempBlock.Values.Where(tb=>!tb.IsExpired).ToList().AsReadOnly();
        }

        public TemporalBlock? GetByCode(string countryCode)
        {
            _tempBlock.TryGetValue(countryCode, out var block);
            return block;
        }

        public bool Remove(string countryCode)
        {
            return _tempBlock.TryRemove(countryCode, out var block);
        }

        public void RemoveExpired()
        {
           var expired = _tempBlock.Where(tb=>tb.Value.IsExpired)
                                    .Select(tb=>tb.Key)
                                    .ToList();

            foreach (var block in expired)
            {
                _tempBlock.TryRemove(block, out _);
            }
        }
    }
}
