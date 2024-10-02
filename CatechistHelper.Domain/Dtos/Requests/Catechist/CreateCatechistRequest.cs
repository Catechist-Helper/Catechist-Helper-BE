namespace CatechistHelper.Domain.Dtos.Requests.Catechist
{
    public class CreateCatechistRequest
    {
        public string Code { get; set; } = null!;    
        public string FullName { get; set; } = null!; 
        public string Gender { get; set; } = null!;   
        public DateTime DateOfBirth { get; set; }     
        public string? BirthPlace { get; set; }     
        public string? FatherName { get; set; }      
        public string? FatherPhone { get; set; }      
        public string? MotherName { get; set; }        
        public string? MotherPhone { get; set; }      
        public string? ImageUrl { get; set; }       
        public string? Address { get; set; } 
        public string? Phone { get; set; }          
        public string? Qualification { get; set; }   
        public bool IsTeaching { get; set; } = true;  
        public string? Note { get; set; }   

        // Foreign keys
        public Guid AccountId { get; set; }        
        public Guid ChristianNameId { get; set; }     
        public Guid LevelId { get; set; }
    }
}
