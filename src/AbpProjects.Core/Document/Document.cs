using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Document
{
    [Table("Document")]
  public class document : FullAuditedEntity
    {
        public const int maxLength = 500;
        [Required]
        [MaxLength(maxLength)]
        public virtual string Title { get; set; }
        
        [Required]
       
        public virtual string Attachment { get; set; }
    }
}
