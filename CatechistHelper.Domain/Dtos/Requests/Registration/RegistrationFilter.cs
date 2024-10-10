using CatechistHelper.Domain.Enums;

namespace CatechistHelper.Domain.Dtos.Requests.Registration
{
    public class RegistrationFilter
    {
        public DateTime? StartDate {  get; set; }
        public DateTime? EndDate { get; set; }
        public RegistrationStatus? Status { get; set; }
    }
}
