using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.MeghPlanningSupports
{
    [Table("TblTypename")]
    public class Typename : FullAuditedEntity
    {
        public const int MaxNameLength = 500;

        [MaxLength(MaxNameLength)]
        public virtual string Name { get; set; }
    }
}
