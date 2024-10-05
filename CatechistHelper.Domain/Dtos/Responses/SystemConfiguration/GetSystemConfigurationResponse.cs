﻿namespace CatechistHelper.Domain.Dtos.Responses.SystemConfiguration
{
    public class GetSystemConfigurationResponse
    {
        public Guid Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
