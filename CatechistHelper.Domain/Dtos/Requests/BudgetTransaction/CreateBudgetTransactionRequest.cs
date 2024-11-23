using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.BudgetTransaction
{
    public class CreateBudgetTransactionRequest
    {
        [Range(0, double.MaxValue, ErrorMessage = MessageConstant.Common.NegativeNumberError)]
        public double FromBudget { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = MessageConstant.Common.NegativeNumberError)]
        public double ToBudget { get; set; }
        public Guid EventId { get; set; }
    }
}
