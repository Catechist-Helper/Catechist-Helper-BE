﻿using CatechistHelper.Domain.Dtos.Responses.Event;

namespace CatechistHelper.Domain.Dtos.Responses.BudgetTransaction
{
    public class GetBudgetTransactionResponse
    {
        public Guid Id { get; set; }
        public double FromBudget { get; set; }
        public double ToBudget { get; set; }
        public GetEventResponse? Event { get; set; }
    }
}