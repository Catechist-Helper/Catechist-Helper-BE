namespace CatechistHelper.Domain.Dtos.Responses.Timetable
{
    public class ClassResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int NumberOfCatechist { get; set; }

        public string Note { get; set; } = string.Empty;

        public string PastoralYearName { get; set; } = string.Empty;

        public string GradeName { get; set; } = string.Empty;
    }
}
