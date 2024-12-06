namespace CatechistHelper.Domain.Dtos.Requests.LeaveRequest
{
    public class LeaveRequestDto
    {
        public Guid CatechistId { get; set; }
        public DateTime LeaveDate { get; set; }
        public string Reason { get; set; } = null!;
    }


}
