using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Project
{
    [Table("tbl_User_Story")]
    public class ImportUserStoryDetails: FullAuditedEntity
    {
        public virtual string UserStory { get; set; }
        public virtual int ProjectId { get; set; }
        public virtual decimal? DeveloperHours { get; set; }
        public virtual decimal? ExpectedHours { get; set; }
        public virtual decimal? ActualHours { get; set; }
        public virtual int status { get; set; }
        public virtual int EmployeeId { get; set; } = 0;
        public virtual DateTime? AssignToDate { get; set; }
    }
}
