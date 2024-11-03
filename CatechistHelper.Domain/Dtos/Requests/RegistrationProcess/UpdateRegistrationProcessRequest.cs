using CatechistHelper.Domain.Constants;
using CatechistHelper.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.RegistrationProcess
{
    public class UpdateRegistrationProcessRequest
    {
        [Required(ErrorMessage = MessageConstant.RegistrationProcess.Require.NameRequired)]
        [MaxLength(20, ErrorMessage = "Tối đa {1} kí tự!")]
        public string Name { get; set; } = null!;
        public RegistrationProcessStatus Status { get; set; }
    }
}
