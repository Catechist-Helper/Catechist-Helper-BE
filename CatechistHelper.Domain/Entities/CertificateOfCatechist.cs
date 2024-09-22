using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatechistHelper.Domain.Entities
{
    [Table("certificate_of_catechist")]
    public class CertificateOfCatechist
    {
        [Column("certificate_id")]
        public Guid CertificateId { get; set; }
        public virtual Certificate Certificate { get; set; } = null!;

        [Column("catechist_id")]
        public Guid CatechistId { get; set; }
        public virtual Catechist Catechist { get; set; } = null!;

        [Column("granted_date")]
        public DateTime GrantedDate { get; set; }
    }
}
