namespace BlockedCountries.Domain.Entities
{
    public class BlockedCountry
    {
        public string CountryCode { get; set; }
        public DateTime BlockedAt { get; set; } = DateTime.UtcNow; 


    }
}
