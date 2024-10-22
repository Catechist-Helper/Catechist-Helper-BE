using CatechistHelper.Domain.Enums;

namespace CatechistHelper.Domain.Models
{
    public class GradeFilter
    {
        public Guid? MajorId { get; set; }
        public Guid? PastoralYearId { get; set; }
    }
}
