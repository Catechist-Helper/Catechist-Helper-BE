using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CatechistHelper.Domain.Dtos.Responses.TransactionImages
{
    public class GetTransactionImagesResponse
    {
        public Guid Id { get; set; }
        public Guid BudgetTransactionId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime UploadAt { get; set; }
    }
}
