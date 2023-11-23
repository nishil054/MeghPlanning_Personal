using Abp.Domain.Entities.Auditing;
using AbpProjects.ProjectType;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Project
{
    [Table("Projecttype_details")]
  public class Projecttype_details : FullAuditedEntity
    {
        public virtual string hours { get; set; }
        public virtual decimal Price { get; set; }
        public virtual int TypeID { get; set; }

        [ForeignKey("TypeID")]
        public virtual projecttype ProjectType { get; set; }
        public virtual int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual project project { get; set; }
        public virtual bool? IsOutSource { get; set; }
        public virtual decimal? CostforCompany { get; set; }
        public virtual string Comments { get; set; }
    }
}
