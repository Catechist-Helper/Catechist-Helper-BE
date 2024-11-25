using CatechistHelper.Domain.Enums;

namespace CatechistHelper.Domain.Dtos.Requests.Registration
{
    public class UpdateRegistrationRequest
    {
        public RegistrationStatus Status { get; set; }
        public List<Guid>? Accounts { get; set; }
        public string? Reason { get; set; }
        public string? Note { get; set; }
    }
}
