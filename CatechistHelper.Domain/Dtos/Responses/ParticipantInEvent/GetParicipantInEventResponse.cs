using CatechistHelper.Domain.Dtos.Responses.Event;

namespace CatechistHelper.Domain.Dtos.Responses.ParticipantInEvent
{
    public class GetParicipantInEventResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = null!;
        public bool IsAttended { get; set; }
        public GetEventResponse? Event { get; set; }
    }
}
