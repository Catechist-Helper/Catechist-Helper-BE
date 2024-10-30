using CatechistHelper.Domain.Dtos.Responses.Account;
using CatechistHelper.Domain.Dtos.Responses.Certificate;
using CatechistHelper.Domain.Dtos.Responses.Level;

namespace CatechistHelper.Domain.Dtos.Responses.Catechist
{
    public class GetCatechistResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }  
        public string BirthPlace { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
        public bool IsTeaching { get; set; } = true;
        public GetAccountResponse Account { get; set; }
        public Guid ChristianNameId { get; set; }
        public string ChristianName { get; set; } = string.Empty;
        public Guid LevelId { get; set; }
        public GetLevelResponse Level { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string FatherName { get; set; } = string.Empty;
        public string MotherName { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string LevelName { get; set; } = string.Empty;
        public List<GetCertificateResponse> Certificates { get; set; } = [];
    }

}
