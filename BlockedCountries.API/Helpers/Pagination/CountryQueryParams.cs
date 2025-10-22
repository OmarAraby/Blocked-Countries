namespace BlockedCountries.API.Helpers.Pagination
{
    public class CountryQueryParams
    {
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; } = "blockedat";
        public bool SortDescending { get; set; } = false;
    }
}
