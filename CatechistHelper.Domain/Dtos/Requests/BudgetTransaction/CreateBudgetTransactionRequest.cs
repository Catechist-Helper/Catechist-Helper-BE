using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatechistHelper.Domain.Dtos.Requests.BudgetTransaction
{
    public class CreateBudgetTransactionRequest
    {
        public double FromBudget { get; set; }
        public double ToBudget { get; set; }
        public Guid EventId { get; set; }
    }
}
