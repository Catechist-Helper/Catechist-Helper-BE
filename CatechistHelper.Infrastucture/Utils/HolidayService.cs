using CatechistHelper.Domain.Dtos.Responses.Timetable;
using Newtonsoft.Json;
using System.Globalization;

namespace CatechistHelper.Infrastructure.Utils
{
    public static class HolidayService
    {
        private const string Key = "AIzaSyBNuIiZaijfVoeYNvSi1kBE8k4jR0yAJDs";

        private const string Url = "https://www.googleapis.com/calendar/v3/calendars/en.vietnamese%23holiday%40group.v.calendar.google.com/events?key=";

        public static async Task<HolidayResponse> GetAllHolidays()
        {
            var client = new HttpClient();

            string fullUrl = $"{Url}{Key}";

            HttpResponseMessage response = await client.GetAsync(fullUrl);

            string responseBody = await response.Content.ReadAsStringAsync();

            var settings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            HolidayResponse? apiResponse = JsonConvert.DeserializeObject<HolidayResponse>(responseBody, settings);

            return Validator.EnsureNonNull(apiResponse);

        }

        public static async Task<List<CalendarEvent>> GetHolidaysInRange(DateTime startDate, DateTime? endDate)
        {
            var holidays = await GetAllHolidays();

            var eventsInRange = holidays.Items
                .Where(e => 
                    ParseDate(e.Start.Date, "yyyy-MM-dd") >= startDate && 
                    ParseDate(e.End.Date, "yyyy-MM-dd") <= endDate)
                .ToList();

            return eventsInRange;
        }

        public static DateTime ParseDate(string date, string format)
        {
            return DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
        }

    }


}
