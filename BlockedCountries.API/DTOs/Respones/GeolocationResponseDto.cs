using System.Text.Json.Serialization;

namespace BlockedCountries.API.DTOs.Respones
{
    public class GeolocationResponseDto
    {
        public string? Ip { get; set; }
        public Location? Location { get; set; }

        //// Helper properties for easy access
        //public string? CountryCode => Location?.CountryCode2;
        //public string? CountryName => Location?.CountryName;
        //public string? City => Location?.City;
        //public string? Region => Location?.Region;
        public string? Error { get; set; }
    }

    public class Location
    {
        [JsonPropertyName("country_code2")]
        public string? CountryCode2 { get; set; }

        [JsonPropertyName("country_name")]
        public string? CountryName { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("state_prov")]
        public string? Region { get; set; }

    }
}
