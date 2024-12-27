using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Process
{
    public class UpdateProcessRequest : CreateProcessRequest
    {
        [Range(0, double.MaxValue, ErrorMessage = MessageConstant.Common.NegativeNumberError)]
        public double ActualFee { get; set; }

        public ProcessStatus Status { get; set; }

        public List<IFormFile> ReceiptImages { get; set; } = [];
    }
}
