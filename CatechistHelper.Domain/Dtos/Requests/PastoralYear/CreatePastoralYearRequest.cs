using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.PastoralYear
{
    public class CreatePastoralYearRequest
    {
        [Required(ErrorMessage = MessageConstant.PastoralYear.Require.NameRequired)]
        [MaxLength(50, ErrorMessage = "Tối đa {1} kí tự!")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Tối đa {1} kí tự!")]
        public string? Note { get; set; }

        public PastoralYearStatus PastoralYearStatus { get; set; }
    }
}
