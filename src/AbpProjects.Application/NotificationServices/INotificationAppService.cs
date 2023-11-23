using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.Authorization.Users.Dto;
using AbpProjects.NotificationServices.Dto;
using AbpProjects.Project.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.NotificationServices
{
    public interface INotificationAppService : IApplicationService
    {
        List<GetNotificationDto> GetNotification();
        Task UpdateNotification(NotificationDto input);
        Task<GetNotificationDetailsDto> GetNotificationDetails();
        List<ProjectDataDto> GetReminderProjectList();
       
    }
}
