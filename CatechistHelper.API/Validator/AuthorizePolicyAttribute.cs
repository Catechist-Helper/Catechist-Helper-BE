using CatechistHelper.API.Utils;
using CatechistHelper.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace CatechistHelper.API.Validator
{
    public class AuthorizePolicyAttribute : AuthorizeAttribute
    {
        public AuthorizePolicyAttribute(params RoleEnum[] roleEnums)
        {
            var allowedRolesAsString = roleEnums.Select(EnumUtil.GetDescriptionFromEnum);
            Roles = string.Join(",", allowedRolesAsString);
        }
    }
}
