﻿namespace CatechistHelper.Domain.Dtos.Requests.Catechist
{
    public class UpdateCatechistRequest : CreateCatechistRequest
    {
        public Guid Id { get; set; }
    }
}