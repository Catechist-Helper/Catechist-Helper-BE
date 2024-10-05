using CatechistHelper.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Requests.SystemConfiguration
{
    public class CreateSystemConfigurationRequest
    {
        [Required(ErrorMessage = MessageConstant.SystemConfiguration.Require.KeyRequired)]
        public string Key { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.SystemConfiguration.Require.ValueRequired)]
        public string Value { get; set; } = null!;
    }
}
