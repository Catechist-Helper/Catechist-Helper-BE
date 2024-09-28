using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Registration
{
    public class UpdateRegistrationRequest
    {
        public RegistrationStatus Status { get; set; }
        public List<Guid>? Accounts { get; set; }
    }
}
