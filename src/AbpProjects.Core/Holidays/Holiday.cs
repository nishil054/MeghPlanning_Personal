using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Holidays
{
    [Table("HolidayMaster")]
    public class Holiday : FullAuditedEntity
    {
        public const int maxLengthNumber = 100;
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual int Type { get; set; }
        public virtual string Title { get; set; }
    }
}
