using CatechistHelper.Domain.Enums;

namespace CatechistHelper.Domain.Dtos.Responses.PastoralYear
{
    public class GetPastoralYearResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }

        public PastoralYearStatus PastoralYearStatus { get; set; }
    }
}
