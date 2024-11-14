namespace CatechistHelper.Domain.Dtos.Requests.AbsenceRequest
{
    public class AbsenceRequestDto
    {
        public Guid CatechistId { get; set; }
        public Guid SlotId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public Guid? ReplacementCatechistId { get; set; }
    }
}
