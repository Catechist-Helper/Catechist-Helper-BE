﻿namespace CatechistHelper.Domain.Dtos.Responses.Authentication
{
    public class LoginResponse
    {
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}