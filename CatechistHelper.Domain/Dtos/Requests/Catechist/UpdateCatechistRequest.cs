using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.Catechist
{
    public class UpdateCatechistRequest 
    {
        [MaxLength(50, ErrorMessage = "Tối đa {1} kí tự!")]
        public string? FatherName { get; set; }

        [RegularExpression(@"^(\d{10})$", ErrorMessage = MessageConstant.Common.InvalidPhoneNumber)]
        public string? FatherPhone { get; set; }

        [MaxLength(50, ErrorMessage = "Tối đa {1} kí tự!")]
        public string? MotherName { get; set; }

        [RegularExpression(@"^(\d{10})$", ErrorMessage = MessageConstant.Common.InvalidPhoneNumber)]
        public string? MotherPhone { get; set; }

        [MaxLength(200, ErrorMessage = "Tối đa {1} kí tự!")]
        public string? Address { get; set; }

        [RegularExpression(@"^(\d{10})$", ErrorMessage = MessageConstant.Common.InvalidPhoneNumber)]
        public string? Phone { get; set; }

        [MaxLength(50, ErrorMessage = "Tối đa {1} kí tự!")]
        public string? Qualification { get; set; }
        public bool IsTeaching { get; set; } = true;
<<<<<<< Updated upstream

        [MaxLength(100, ErrorMessage = "Tối đa {1} kí tự!")]
        public string? Note { get; set; }
=======
        public IFormFile? ImageUrl { get; set; }
>>>>>>> Stashed changes

        // Foreign keys
        public Guid? AccountId { get; set; }
        public Guid ChristianNameId { get; set; }
    }
}
