using CatechistHelper.Domain.Dtos.Responses.MemberOfProcess;
using CatechistHelper.Domain.Entities;
using CatechistHelper.Domain.Enums;

namespace CatechistHelper.Domain.Dtos.Responses.Process
{
    public class GetReceiptImage
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public DateTime UploadAt { get; set; }
        public Guid ProcessId { get; set; }
    }

    public class GetProcessResponse
    {
        public Guid Id { get; set; } 
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public long Duration { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Fee { get; set; }
        public double ActualFee { get; set; }
        public string Note { get; set; }
        public string Comment { get; set; }
        public ProcessStatus Status { get; set; }
        public Guid EventId { get; set; }
        public List<GetReceiptImage> ReceiptImages { get; set; }
        public List<GetMemberOfProcessRepsonse> MemberOfProcesses {  get; set; }

    }
}
