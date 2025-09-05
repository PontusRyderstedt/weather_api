namespace WeatherApi.Services
{
    public static class Utils
    {
        public static (string? station, DateTime? from, DateTime? to, IResult? error) ParseAndValidate(string? station, string? from, string? to)
        {
            DateTime? fromDate = null;
            DateTime? toDate = null;

            if (from != null)
            {
                if (!DateTime.TryParse(from, out var parsedFrom))
                {
                    return (null, null, null, Results.BadRequest(new { error = "Invalid 'from' date format. Use yyyy-MM-dd." }));
                }
                fromDate = parsedFrom;
            }

            if (to != null)
            {
                if (!DateTime.TryParse(to, out var parsedTo))
                {
                    return (null, null, null, Results.BadRequest(new { error = "Invalid 'to' date format. Use yyyy-MM-dd." }));
                }
                toDate = parsedTo;
            }

            station = station.NullIfWhiteSpace();

            if (station == null && (fromDate != null || toDate != null))
            {
                return (null, null, null, Results.BadRequest(new { error = "Filtering by date range without specifying a station is not supported." }));
            }

            return (station, fromDate, toDate, null);
        }

        public static string? NullIfWhiteSpace(this string? input) => string.IsNullOrWhiteSpace(input) ? null : input.Trim();
    }
}