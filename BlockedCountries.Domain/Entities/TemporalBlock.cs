namespace BlockedCountries.Domain.Entities
{
    public class TemporalBlock
    {
        public string CountryCode { get; set; } = string.Empty;
        public DateTime BlockedAt { get; set; } = DateTime.UtcNow;
        public int DurationMinutes { get; set; }
        public DateTime ExpiryTime => BlockedAt.AddMinutes(DurationMinutes);
        public bool IsExpired => DateTime.UtcNow >= ExpiryTime;

    }
}
