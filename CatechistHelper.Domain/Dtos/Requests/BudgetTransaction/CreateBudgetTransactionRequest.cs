using CatechistHelper.Domain.Constants;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.BudgetTransaction
{
    public class CreateBudgetTransactionRequest
    {
        [Range(0, double.MaxValue, ErrorMessage = MessageConstant.Common.NegativeNumberError)]
        public double FromBudget { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = MessageConstant.Common.NegativeNumberError)]
        public double ToBudget { get; set; }

        [MaxLength(500, ErrorMessage = "Tối đa {1} kí tự!")]
        public string? Note { get; set; }

        public Guid EventId { get; set; }

        public List<IFormFile> TransactionImages { get; set; } = [];
    }
}
