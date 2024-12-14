using CatechistHelper.Domain.Constants;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Catechist
{
    public class UpdateCatechistRequest 
    {
        [MaxLength(50, ErrorMessage = "Tối đa {1} kí tự!")]
        public string? FatherName { get; set; }

        public string? FatherPhone { get; set; }

        [MaxLength(50, ErrorMessage = "Tối đa {1} kí tự!")]
        public string? MotherName { get; set; }

        public string? MotherPhone { get; set; }

        [MaxLength(200, ErrorMessage = "Tối đa {1} kí tự!")]
        public string? Address { get; set; }

        public string? Phone { get; set; }

        [MaxLength(50, ErrorMessage = "Tối đa {1} kí tự!")]
        public string? Qualification { get; set; }
        public bool IsTeaching { get; set; } = true;

        public IFormFile? ImageUrl { get; set; }

        // Foreign keys
        public Guid? AccountId { get; set; }
        public Guid ChristianNameId { get; set; }
    }
}
