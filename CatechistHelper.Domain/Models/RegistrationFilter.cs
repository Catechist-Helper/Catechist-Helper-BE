using CatechistHelper.Domain.Enums;

namespace CatechistHelper.Domain.Models
{
    public class RegistrationFilter
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Year { get; set; }
        public RegistrationStatus? Status { get; set; }
    }
}
