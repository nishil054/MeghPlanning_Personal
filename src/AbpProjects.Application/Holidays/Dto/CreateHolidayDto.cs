using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Holidays.Dto
{
    [AutoMapTo(typeof(Holiday))]
    public class CreateHolidayDto
    {
        public const int maxLengthName = 100;
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual int Type { get; set; }
        public virtual string Title { get; set; }
    }
}
