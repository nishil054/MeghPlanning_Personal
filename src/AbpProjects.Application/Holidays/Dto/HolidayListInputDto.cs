using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Holidays.Dto
{
    [AutoMapFrom(typeof(Holiday))]
    public class HolidayListInputDto: EntityDto
    {
        
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual int Type { get; set; }
        public virtual string Title { get; set; }
       
    }

    public class HoildayMasterInput : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public string search { get; set; }
        public virtual string Title { get; set; }
        public void Normalize()
        {
            if (String.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    }
}
