using CatechistHelper.Domain.Enums;

namespace CatechistHelper.Domain.Dtos.Responses.RegistrationProcess
{
    public class GetRegistrationProcessResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public RegistrationProcessStatus Status { get; set; }
    }
}
