﻿using CatechistHelper.Domain.Constants;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Account
{
    public class UpdateAccountRequest
    {
        [Required(ErrorMessage = MessageConstant.Account.Require.FullNameRequired)]
        [MaxLength(50, ErrorMessage = "Vượt quá {1} kí tự")]
        public string FullName { get; set; } = null!;

        [MaxLength(10, ErrorMessage = "Vượt quá {1} kí tự")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.Require.PhoneRequired)]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "không đúng định dạng số điện thoại")]
        public string Phone { get; set; } = null!;
        public IFormFile? Avatar { get; set; }
    }
}
