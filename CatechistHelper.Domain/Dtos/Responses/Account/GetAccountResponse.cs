using CatechistHelper.Domain.Dtos.Responses.Role;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Responses.Account
{
    public class GetAccountResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Avatar {  get; set; } = string.Empty;
        public RoleResponse Role { get; set; } 
    }
}
