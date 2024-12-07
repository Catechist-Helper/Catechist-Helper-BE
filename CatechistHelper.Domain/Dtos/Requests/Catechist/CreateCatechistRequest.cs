﻿using Microsoft.AspNetCore.Http;

namespace CatechistHelper.Domain.Dtos.Requests.Catechist
{
    public class CreateCatechistRequest : UpdateCatechistRequest
    {
        public IFormFile? ImageUrl { get; set; }
    }
}
