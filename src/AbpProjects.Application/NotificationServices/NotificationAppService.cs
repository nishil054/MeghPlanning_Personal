using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using AbpProjects.Authorization;
using AbpProjects.Authorization.Roles;
using AbpProjects.Authorization.Users;
using AbpProjects.Authorization.Users.Dto;
using AbpProjects.InvoiceRequest;
using AbpProjects.MeghPlanningNotification;
using AbpProjects.MeghPlanningSupports;
using AbpProjects.NotificationServices.Dto;
using AbpProjects.Productions;
using AbpProjects.Project;
using AbpProjects.TimeSheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AbpProjects.NotificationServices
{
    [AbpAuthorize(PermissionNames.Pages_Notification)]
    public class NotificationAppService : AbpProjectsApplicationModule, INotificationAppService
    {
        private readonly IRepository<Notification> _notificationRepository;
        private readonly IRepository<UserNotification> _usernotificationRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IAbpSession _abpSession;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Production> _productionRepository;
        private readonly IRepository<project> _projectRepository;
        private readonly IRepository<Client> _clientRepository;
        private readonly IRepository<ProjectStatus> _projectStatusRepository;
        private readonly IRepository<timesheet> _timesheetRepository;
        private readonly IRepository<invoicerequest> _invoiceRequestRepository;
        private readonly IUserEmailer _userEmailer;

        public NotificationAppService(IRepository<Notification> notificationRepository,
            IRepository<UserNotification> usernotificationRepository,IRepository<User, long> userRepository,
            IAbpSession abpSession,
            IRepository<Role> roleRepository, IRepository<UserRole, long> userRoleRepository,
             IRepository<Production> productionRepository,
             IRepository<project> projectRepository, IRepository<Client> clientRepository,
             IRepository<ProjectStatus> projectStatusRepository, IRepository<timesheet> timesheetRepository,
             IRepository<invoicerequest> invoiceRequestRepository, IUserEmailer userEmailer

            )
        {
            _notificationRepository = notificationRepository;
            _usernotificationRepository= usernotificationRepository;
            _userRepository = userRepository;
            _abpSession = abpSession;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _productionRepository = productionRepository;
            _projectRepository = projectRepository;
            _clientRepository = clientRepository;
            _projectStatusRepository = projectStatusRepository;
            _timesheetRepository = timesheetRepository;
            _invoiceRequestRepository = invoiceRequestRepository;
            _userEmailer = userEmailer;
        }

        public List<GetNotificationDto> GetNotification()
        { 
            
            var notificationlist = (from a in _notificationRepository.GetAll()
                            select new GetNotificationDto
                            {
                                Id = a.Id,
                                Title = a.Title,
                            }).ToList();

            return notificationlist;
        }

        public async Task<GetNotificationDetailsDto> GetNotificationDetails()
        {
            GetNotificationDetailsDto items = new GetNotificationDetailsDto();
            var userids = _usernotificationRepository.GetAll().Where(x=>x.NotificationId==1).Select(x => x.UserId.ToString()).ToArray();
            items.UserId = userids;
            string ary = "";
            if (userids.Length > 0)
            {
                foreach (var item in userids)
                {

                    int userid = Convert.ToInt32(item);
                    var nm = _userRepository.GetAll().Where(e => e.Id == userid).Select(x => x.Name).FirstOrDefault();
                    var surnm = _userRepository.GetAll().Where(e => e.Id == userid).Select(x => x.Surname).FirstOrDefault();
                    var ans = nm + " " + surnm;
                    ary += ans + ", ";

                }
                ary = ary.Remove(ary.LastIndexOf(','));
            }
            items.Name = ary;
            return items;
        }

        public List<ProjectDataDto> GetReminderProjectList()
        {
          
                var User_List = _userRepository.GetAll();

                var username = (from user in User_List
                                join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                                from ur in urJoined.DefaultIfEmpty()
                                join us in _roleRepository.GetAll() on ur.RoleId equals us.Id into usJoined
                                from us in usJoined.DefaultIfEmpty()
                                where (us != null && user.Id == _abpSession.UserId)
                                select (us.DisplayName)).FirstOrDefault();

                var actualhours = _timesheetRepository.GetAll()
                            .GroupBy(a => a.ProjectId)
                            .Select(a => new { Hours = a.Sum(b => b.Hours), ProjectId = a.Key })
                            .OrderByDescending(a => a.Hours)
                            .ToList();

                var cc = (from p in _projectRepository.GetAll()
                          join Pro in _productionRepository.GetAll()
                          on p.Id equals Pro.Projectid into productiondata

                          join Invreq in _invoiceRequestRepository.GetAll()
                          on p.Id equals Invreq.ProjectId into projrepo

                          join c in _clientRepository.GetAll()
                          on p.ClientId equals c.Id

                          join ud in _userRepository.GetAll()
                          on (Int32)p.Marketing_LeaderId equals ud.Id into userd
                          from ud in userd.DefaultIfEmpty()

                          join s in _projectStatusRepository.GetAll()
                          on p.Status equals s.Id
                          where p.Status == 4 || p.Status == 5 || p.Status == 6 || p.Status == 7 || p.Status == 10 || p.Status == 1
                          select new ProjectDataDto
                          {
                              Id = p.Id,
                              ProjectName = p.ProjectName,
                              totalhours = p.totalhours,
                              actualhours = 0,
                          }).ToList();

                foreach (var item in cc)
                {
                    item.actualhours = actualhours.Where(x => x.ProjectId == item.Id).Select(x => x.Hours).FirstOrDefault();
                    item.hourPercentage = item.totalhours > 0 ? ((decimal)actualhours.Where(s => s.ProjectId == item.Id).Select(s => s.Hours).DefaultIfEmpty(0).Sum()) * 100 / item.totalhours : 0;
                   
                }
             
            //var data = cc.Where(x => x.hourPercentage >= 100).Select(x => x).ToList();
            var data = cc.Where(x => x.hourPercentage >= 60).Select(x => x).ToList();
            return data;



        }
        public async Task UpdateNotification(NotificationDto input)
        {
           
            if (input.UserId != null)
            {
                var deleteitem = _usernotificationRepository.GetAll().Where(x => x.NotificationId == input.NotificationId).ToList();
                foreach (var item in deleteitem)
                {
                    var id = item.Id;
                    await _usernotificationRepository.DeleteAsync(id);
                }
                foreach (var id in input.UserId)
                {
                    UserNotification notification = new UserNotification();
                    notification.NotificationId = input.NotificationId;
                    notification.UserId = id;
                    notification.MapTo<UserNotification>();


                    //if (input.ProjectDetails.Count > 0)
                    //{
                    //    #region sendmail
                    //    GetProjectListDto item = new GetProjectListDto();
                    //    item.toEmail = _userRepository.GetAll().Where(x => x.Id == id).Select(x => x.EmailAddress).FirstOrDefault();
                    //    item.FromEmail = _userRepository.GetAll().Where(x => x.UserName == "admin").Select(x => x.EmailAddress).FirstOrDefault();
                    //    //item.toEmail = "sneha.patel@meghtechnologies.com";
                    //    item.EmailTitle = "Project Details";
                    //    item.ProjectDetails = input.ProjectDetails;
                    //    await _userEmailer.SendEmailReminderProjectList(item);
                    //    #endregion
                    //}
                    _usernotificationRepository.InsertAsync(notification);
                }


            }
        }



    }
}
