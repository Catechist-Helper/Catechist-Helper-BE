namespace CatechistHelper.Domain.Dtos
{
    public class PastoralYearDto
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<MajorDto> Majors { get; set; } = [];
    }
    public class MajorDto
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<GradeDto> Grades { get; set; } = [];
    }

    public class GradeDto
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<ClassDto> Classes { get; set; } = [];
    }

    public class ClassDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfCatechist { get; set; }
    }
}
