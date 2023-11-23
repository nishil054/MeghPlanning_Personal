using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.VPS
{
    [Table("VPS")]
    public class vps : FullAuditedEntity
    {
        public const int maxLength = 300;

        [Required]
        public virtual string Title { get; set; }
        [Required]
        public virtual string IP { get; set; }
        [Required]
        public virtual string UserName { get; set; }
        [Required]
        public virtual string Password { get; set; }

        [MaxLength(maxLength)]
     
        public virtual string Comment { get; set; }
    }
}
