using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.TimeSheet
{
    [Table("TimeSheet")]
  public class timesheet : FullAuditedEntity
    {
        public const int maxLength = 1000;
        [Required]
        public virtual int ProjectId { get; set; }
        [Required]
        public virtual int WorkTypeId { get; set; }
        [Required]

        [MaxLength(maxLength)]
        public virtual string Description { get; set; }
        [Required]
        public virtual decimal Hours { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual int UserId { get; set; }
        public virtual decimal? Efforts { get; set; }
        public virtual int? UserStoryId { get; set; }
    }
}
