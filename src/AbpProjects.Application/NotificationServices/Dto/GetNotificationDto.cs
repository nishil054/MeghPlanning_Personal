using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.MeghPlanningNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.NotificationServices.Dto
{
    [AutoMapFrom(typeof(Notification))]
    public class GetNotificationDto : EntityDto
    {
        public virtual string Title { get; set; }
    }
}
