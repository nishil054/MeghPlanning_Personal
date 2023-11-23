using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.NotificationServices.Dto
{
   public class GetNotificationDetailsDto : EntityDto
    {
        //public virtual string Title { get; set; }
        //public virtual int NotificationId { get; set; }
        public virtual string[] UserId { get; set; }
        public virtual string Name { get; set; }
        public virtual long? UId { get; set; }
    }
}
