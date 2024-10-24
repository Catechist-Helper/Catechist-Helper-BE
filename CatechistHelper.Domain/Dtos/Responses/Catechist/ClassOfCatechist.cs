using CatechistHelper.Domain.Dtos.Responses.Class;

namespace CatechistHelper.Domain.Dtos.Responses.Catechist
{
    public class ClassOfCatechist
    {
        public GetClassResponse? Class { get; set; }
        public bool IsMain { get; set; }
    }


}
