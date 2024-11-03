using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.RegistrationProcess
{
    public class CreateRegistrationProcessRequest
    {
        public Guid RegistrationId { get; set; }

        [Required(ErrorMessage = MessageConstant.RegistrationProcess.Require.NameRequired)]
        [MaxLength(20, ErrorMessage = "Tối đa {1} kí tự!")]
        public string Name { get; set; } = null!;
    }
}
