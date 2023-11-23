using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Reports.Dto
{
    public class GetYearDto
    {
        public virtual int year { get; set; }
    }
    public class GetMonth
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
    }
}
