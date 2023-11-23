using Abp.AutoMapper;
using AbpProjects.gstdashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.GSTDashboard.Dto
{
    [AutoMapTo(typeof(gstDashboard))]

    public  class UpdateStatus
    {
        public virtual UpdateStatus[] UpdateStatusdata { get; set; }
        public virtual int Id { get; set; }
        public virtual int UpdateStatusId { get; set; }
    }
}
