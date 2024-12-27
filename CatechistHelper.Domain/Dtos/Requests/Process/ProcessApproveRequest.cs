using CatechistHelper.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Process
{
    public class ProcessApproveRequest
    {
        public string? Comment { get; set; }

        public ProcessStatus Status { get; set; }
    }
}
