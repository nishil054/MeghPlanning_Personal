using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.MeghPlanningSupports
{
    [Table("TblServerType")]
    public class ServerTypeDetail : FullAuditedEntity
    {
        public virtual string Name { get; set; }
    }

}
