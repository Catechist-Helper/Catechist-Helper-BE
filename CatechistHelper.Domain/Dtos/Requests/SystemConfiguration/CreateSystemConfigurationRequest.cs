using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.SystemConfiguration
{
    public class CreateSystemConfigurationRequest
    {
        [Required(ErrorMessage = MessageConstant.SystemConfiguration.Require.KeyRequired)]
        [MaxLength(100, ErrorMessage = "Tối đa {1} kí tự!")]
        public string Key { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.SystemConfiguration.Require.ValueRequired)]
        [MaxLength(100, ErrorMessage = "Tối đa {1} kí tự!")]
        public string Value { get; set; } = null!;
    }
}
