using Abp.AutoMapper;
using AbpProjects.Receipt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Dasboard.Dto
{
    public class GetsumInput 
    {
        public string Filter { get; set; }
      

    }
    [AutoMapFrom(typeof(BillPymtRecd))]
    public class SumDto
    {
        public virtual decimal? totalcollection { get; set; }
        public virtual decimal? totalinvoice { get; set; }
        public virtual decimal? totaloutstanding { get; set; }
        public virtual int? year { get; set; }
        public virtual int? month { get; set; }
        public virtual DateTime? Date { get; set; }

        public virtual int? PymtRecd { get; set; }
    }
}
