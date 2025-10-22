namespace BlockedCountries.API.Domain
{
    public class BlockedCountry
    {
        public string CountryCode { get; set; }
        public DateTime BlockedAt { get; set; } = DateTime.UtcNow; 


    }
}
