using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Reports.Dto
{
    public class InputLoginLogoutReportDto : PagedAndSortedResultRequestDto
    {
        public virtual DateTime FromDate { get; set; }

        public virtual DateTime ToDate { get; set; }
    }

    public class EmployeeInOutDetails: PagedAndSortedResultRequestDto
    {
        public virtual int userId { get; set; }
        public virtual DateTime FromDate { get; set; }

        public virtual DateTime ToDate { get; set; }
    }
}
