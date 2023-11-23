using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.VPS
{
    [AutoMapTo(typeof(vps))]
    public   class CreateVPSDto : EntityDto
    {
        [Required]
        public virtual string Title { get; set; }
        [Required]
        public virtual string IP { get; set; }
        [Required]
        public virtual string UserName { get; set; }
        [Required]
        public virtual string Password { get; set; }
        public virtual string Comment { get; set; }
    }
}
