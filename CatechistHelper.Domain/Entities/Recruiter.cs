using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatechistHelper.Domain.Entities
{
    [Table("recruiter")]
    public class Recruiter
    {
        [Column("user_id")]
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; } = null!;

        [Column("application_id")]
        public Guid ApplicationId { get; set; }
        public virtual Application Application { get; set; } = null!;
    }
}
