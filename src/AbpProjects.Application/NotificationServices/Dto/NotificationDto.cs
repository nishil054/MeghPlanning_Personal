using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.Authorization.Users.Dto;
using AbpProjects.MeghPlanningNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.NotificationServices.Dto
{
    [AutoMapTo(typeof(UserNotification))]
    public class NotificationDto : EntityDto
    {
        public virtual int NotificationId { get; set; }
        public virtual int[] UserId { get; set; }
        public virtual List<ProjectDataDto> ProjectDetails { get; set; }
       
    }
}
