using System.ComponentModel.DataAnnotations.Schema;

namespace CatechistHelper.Domain.Entities
{
    [Table("member_of_process")]
    public class MemberOfProcess
    {
        [Column("account_id")]
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; } = null!;

        [Column("process_id")]
        public Guid ProcessId { get; set; }
        public virtual Process Process { get; set; } = null!;

        [Column("is_main")]
        public bool IsMain { get; set; }
    }
}
