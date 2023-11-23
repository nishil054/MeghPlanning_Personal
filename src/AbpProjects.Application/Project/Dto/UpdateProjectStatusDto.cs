using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Project.Dto
{
    [AutoMapTo(typeof(project))]
    public class UpdateProjectStatusDto
    {
        public virtual UpdateStatus[] UpdateStatus { get; set; }
        public virtual UpdatePriority[] UpdatePriority { get; set; }
        public virtual int Id { get; set; }
        public virtual int? UpdatePrio { get; set; }
        public virtual int UpdateStatusId { get; set; }
    }
    public class UpdateStatus
    {
        public virtual int Id { get; set; }
        public virtual int StatusId { get; set; }

    }

    public class UpdatePriority
    {
        public virtual int Id { get; set; }
        public virtual int Priority { get; set; }
      

    }
}
