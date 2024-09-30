using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.PastoralYear
{
    public class CreatePastoralYearRequest
    {
        [Required(ErrorMessage = MessageConstant.PastoralYear.Require.NameRequired)]
        public string Name { get; set; } = string.Empty;

        public string? Note { get; set; }

        [Required(ErrorMessage = MessageConstant.PastoralYear.Require.StatusRequired)]
        public PastoralYearStatus PastoralYearStatus { get; set; }
    }
}
