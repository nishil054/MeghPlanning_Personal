using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using AbpProjects.Project.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using AbpProjects.MeghPlanningSupports;
using AbpProjects.Authorization.Users;
using AutoMapper;
using AbpProjects.ProjectType;
using AbpProjects.Company;
using AbpProjects.InvoiceRequest;
using AbpProjects.TimeSheet;
using Abp.Runtime.Session;
using AbpProjects.Authorization.Roles;
using Abp.Authorization.Users;
using AbpProjects.Productions;
using AbpProjects.ProjectMilestone;
using Abp.Authorization;
using AbpProjects.Authorization;
using AbpProjects.Authorization.Users.Dto;
using AbpProjects.NotificationServices.Dto;
using AbpProjects.Notifications;
using Abp.Notifications;
using System.Globalization;
using System.Data;

namespace AbpProjects.Project
{
    [AbpAuthorize(PermissionNames.Pages_Project)]
    public class ProjectAppService : AbpProjectsApplicationModule, IProjectAppService
    {
        private readonly IRepository<project> _projectRepository;
        private readonly IRepository<Service> _serviceRepository;
        private readonly IRepository<ManageService> _manageserviceRepository;
        private readonly IRepository<invoicerequest> _invoiceRequestRepository;
        private readonly IRepository<Projecttype_details> _repositoryProjectsDetails;
        private readonly IRepository<Client> _clientRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ProjectStatus> _projectStatusRepository;
        private readonly IRepository<projecttype> _projecttypeRepository;
        private readonly IRepository<company> _companyRepository;
        private readonly IRepository<timesheet> _timesheetRepository;
        private readonly IRepository<ServerTypeDetail> _serverRepository;
        private readonly IRepository<Typename> _typenameRepository;
        private readonly UserManager _userManager;
        private readonly IAbpSession _abpSession;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Production> _productionRepository;
        private readonly IRepository<projectMilestone> _projectmilestoneRepository;
        private readonly IRepository<ServiceRequestHistory> _serviceRequestRepository;

        private readonly INotificationPublisher _notificationPublisher;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IAppNotifier _appNotifier;

        private readonly IAbpSession _session;
        private readonly IUserEmailer _userEmailer;

        public ProjectAppService(IRepository<project> projectRepository,
            IRepository<Projecttype_details> repositoryProjectsDetails,
            IRepository<Client> clientRepository, IRepository<User, long> userRepository,
            IRepository<ProjectStatus> projectStatusRepository, IRepository<projecttype> projecttypeRepository,
            IRepository<company> companyRepository, IRepository<invoicerequest> invoiceRequestRepository,
            IRepository<Service> serviceRepository, IRepository<ManageService> manageserviceRepository,
            IRepository<timesheet> timesheetRepository, IRepository<ServerTypeDetail> serverRepository,
            IRepository<Typename> typenameRepository, UserManager userManager, IAbpSession abpSession,
            IRepository<Role> roleRepository, IRepository<UserRole, long> userRoleRepository,
             IRepository<Production> productionRepository, IRepository<projectMilestone> projectmilestoneRepository,
             IRepository<ServiceRequestHistory> serviceRequestRepository,
              INotificationPublisher notificationPublisher,
          INotificationSubscriptionManager notificationSubscriptionManager,
          IAppNotifier appNotifier,
          IAbpSession session, IUserEmailer userEmailer

             )
        {
            _projectRepository = projectRepository;
            _repositoryProjectsDetails = repositoryProjectsDetails;
            _clientRepository = clientRepository;
            _userRepository = userRepository;
            _projectStatusRepository = projectStatusRepository;
            _projecttypeRepository = projecttypeRepository;
            _companyRepository = companyRepository;
            _invoiceRequestRepository = invoiceRequestRepository;
            _serviceRepository = serviceRepository;
            _manageserviceRepository = manageserviceRepository;
            _timesheetRepository = timesheetRepository;
            _serverRepository = serverRepository;
            _typenameRepository = typenameRepository;
            _userManager = userManager;
            _abpSession = abpSession;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _productionRepository = productionRepository;
            _projectmilestoneRepository = projectmilestoneRepository;
            _serviceRequestRepository = serviceRequestRepository;

            _notificationPublisher = notificationPublisher;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _appNotifier = appNotifier;
            _session = session;
            _userEmailer = userEmailer;

        }
        public async Task CreateProject(CreateProjectDto input)
        {
            var result = input.MapTo<project>();
            int id = 0;
            try
            {
                id = await _projectRepository.InsertAndGetIdAsync(result);
            }
            catch (Exception ex)
            {

                throw;
            }
            if (id > 0)
            {
                foreach (var item in input.ProjectDetail)
                {
                    Projecttype_details obj = new Projecttype_details();
                    obj.ProjectId = id;
                    obj.TypeID = item.ProjectType;
                    obj.Price = item.ProjectPrice;
                    obj.hours = item.hours;
                    await _repositoryProjectsDetails.InsertAsync(obj);



                }
            }
        }
        public async Task DeleteProject(EntityDto input)
        {
            await _projectRepository.DeleteAsync(input.Id);
            var dataList = _repositoryProjectsDetails.GetAll().Where(x => x.ProjectId == input.Id).ToList();
            if (dataList.Count > 0)
            {
                foreach (var item in dataList)
                {
                    await _repositoryProjectsDetails.DeleteAsync(x => x.Id == item.Id);
                }

            }
        }

        public async Task<ProjectDto> GetProjectEdit(EntityDto input)
        {
            var project = (await _projectRepository.GetAsync(input.Id)).MapTo<ProjectDto>();
            return project;
        }
        public async Task<string> GetProjectName(EntityDto input)
        {
            string projectName = "";
            var data = await _projectRepository.GetAsync(input.Id);
            if (data != null)
            {
                projectName = data.ProjectName;
            }
            return projectName;
        }

        public async Task<ProjectViewDto> GetProjectViewById(EntityDto input)
        {
            ProjectViewDto lst = new ProjectViewDto();
            try
            {
                var data = await _projectRepository.GetAsync(input.Id);
                lst.Id = data.Id;
                lst.BeneficiaryCompanyId = data.BeneficiaryCompanyId;

                var Cname = _companyRepository.GetAll().Where(x => x.Id == data.BeneficiaryCompanyId).FirstOrDefault();
                if (Cname != null)
                {
                    lst.CompanyName = Cname.Beneficial_Company_Name;
                }

                lst.ProjectName = data.ProjectName;
                lst.Description = data.Description;
                lst.StartDate = data.StartDate;
                lst.EndDate = data.EndDate;
                lst.TeamDeadline = data.TeamDeadline;
                if (data.ActualEndDate.HasValue)
                {
                    lst.ActualEndDate = data.ActualEndDate.Value;
                }
                //lst.CompanyName = data.CompanyName;
                if (data.Marketing_LeaderId.HasValue)
                {
                    lst.Marketing_LeaderId = data.Marketing_LeaderId.Value;
                    var username = _userRepository.GetAll().Where(x => x.Id == data.Marketing_LeaderId.Value).FirstOrDefault();
                    if (username != null)
                    {
                        lst.Marketing_LeaderName = username.FullName;
                    }

                }
                lst.Price = data.Price;
                lst.totalhours = data.totalhours;
                var clientData = _clientRepository.GetAll().Where(x => x.Id == data.ClientId).FirstOrDefault();
                if (clientData != null)
                {
                    lst.ClientName = clientData.ClientName;
                }
                var statusData = _projectStatusRepository.GetAll().Where(x => x.Id == data.Status).FirstOrDefault();
                if (statusData != null)
                {
                    lst.Status = statusData.Status;
                }

            }
            catch (Exception ex)
            {
            }
            return lst;
        }

        //public async Task<PagedResultDto<ProjectDto>> GetProjectLists(ProjectMasterInput input)
        //{
        //    var cc = _projectRepository.GetAll()
        //       .WhereIf(!input.ProjectName.IsNullOrEmpty(), p => p.ProjectName.ToLower().Contains(input.ProjectName.ToLower())
        //      );
        //    var userData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
        //    var userCount = cc.Count();
        //    return new PagedResultDto<ProjectDto>(userCount, userData.MapTo<List<ProjectDto>>());
        //}

        public async Task<ListResultDto<ProjectDto>> GetProjects()
        {
            var projects = _projectRepository.GetAll().OrderByDescending(p => p.Id).ToList();
            return new ListResultDto<ProjectDto>(projects.MapTo<List<ProjectDto>>());
        }

        public PagedResultDto<ProjectDto> GetProjectData(GetProjectDto input)
        {
            var Query = _projectRepository.GetAll();
            var userData = Query.OrderBy(input.Sorting).PageBy(input).ToList();
            var userCount = Query.Count();
            return new PagedResultDto<ProjectDto>(userCount, userData.MapTo<List<ProjectDto>>());
        }

        public PagedResultDto<ProjectDto> GetProjectList(GetProjectDto input)
        {
            try
            {
                var User_List = _userRepository.GetAll();
                // var uid = (int)User_List.Where(u => u.Id == _abpSession.UserId).Select(t => t.Id).FirstOrDefault();
                var username = (from user in User_List
                                join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                                from ur in urJoined.DefaultIfEmpty()
                                join us in _roleRepository.GetAll() on ur.RoleId equals us.Id into usJoined
                                from us in usJoined.DefaultIfEmpty()
                                where (us != null && user.Id == _abpSession.UserId)
                                select (us.DisplayName)).FirstOrDefault();


                // var projrepo = _invoiceRequestRepository.GetAll();
                var actualhours = _timesheetRepository.GetAll()
                            .GroupBy(a => a.ProjectId)
                            .Select(a => new { Hours = a.Sum(b => b.Hours), ProjectId = a.Key })
                            .OrderByDescending(a => a.Hours)
                            .ToList();

                var cc = (from p in _projectRepository.GetAll()
                          join Pro in _productionRepository.GetAll() on p.Id equals Pro.Projectid into productiondata
                          join Invreq in _invoiceRequestRepository.GetAll() on p.Id equals Invreq.ProjectId into projrepo
                          join c in _clientRepository.GetAll()
                          on p.ClientId equals c.Id

                          //join t in _timesheetRepository.GetAll()
                          //on p.Id equals t.ProjectId
                          join ud in _userRepository.GetAll()
                          on (Int32)p.Marketing_LeaderId equals ud.Id into userd
                          from ud in userd.DefaultIfEmpty()
                          join s in _projectStatusRepository.GetAll()
                          on p.Status equals s.Id
                          //where(ud.Id== uid)
                          select new ProjectDto
                          {
                              Id = p.Id,
                              ProjectName = p.ProjectName,
                              Description = p.Description,
                              StartDate = p.StartDate,
                              EndDate = p.EndDate,
                              Price = p.Price,
                              ClientName = c.ClientName,
                              Marketing_LeaderId = p.Marketing_LeaderId.HasValue ? p.Marketing_LeaderId.Value : 0,
                              MarketingLeadName = ud.Name + " " + ud.Surname,
                              ClientId = p.ClientId,
                              ProjectStatusId = p.Status,
                              ProjectStatus = s.Status,
                              totalhours = p.totalhours,
                              actualhours = 0,
                              username = "",
                              Priority = p.Priority,
                              Invoiceamount = projrepo.Where(x => x.Status == 1).Sum(x => x.Amount),
                              pricesum = projrepo.Where(p => p.Status == 0 || p.Status == 1).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(p => p.Status == 10).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(t => t.ProjectId == p.Id && p.Status == 10).Select(t => t.Amount).Sum(),
                              PendingAmount = ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)) == null ? p.Price : ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)),
                              enableStatus = p.IsEnable
                          })
                .WhereIf(!input.ProjectName.IsNullOrEmpty(), s => s.ProjectName.ToLower().Contains(input.ProjectName.ToLower()) /*|| s.ClientName.ToLower().Contains(input.ProjectName.ToLower()) || s.MarketingLeadName.ToLower().Contains(input.ProjectName.ToLower())*/)
                .WhereIf(input.ClientId.HasValue, s => s.ClientId == input.ClientId.Value)
               .WhereIf(input.ProjectId.HasValue, s => s.Id == input.ProjectId.Value)
               .WhereIf(input.MarketingleadId.HasValue, s => s.Marketing_LeaderId == input.MarketingleadId.Value)
                .WhereIf(input.ProjectStatusId.Length > 0, s => input.ProjectStatusId.Contains(s.ProjectStatusId));
                //var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
                var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
                foreach (var item in ccData)
                {
                    item.objProjectStatusList = GetStatusList();
                    item.actualhours = actualhours.Where(x => x.ProjectId == item.Id).Select(x => x.Hours).FirstOrDefault();
                    item.username = username;

                    //foreach (var datahours in actualhours)
                    //{
                    //    if (datahours.ProjectId==item.Id)
                    //    {
                    //        item.hours = datahours.Hours.ToString();
                    //    }
                    //    else
                    //    {
                    //        item.hours = "0";
                    //    }
                    //}
                }
                var ccCount = cc.Count();

                return new PagedResultDto<ProjectDto>(ccCount, ccData.MapTo<List<ProjectDto>>());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        public List<StatusddlList> GetStatusList()
        {
            List<StatusddlList> objstatus = new List<StatusddlList>();
            try
            {
                var dataStatus = _projectStatusRepository.GetAll().OrderBy(x => x.Status).ToList();

                if (dataStatus != null)
                {
                    foreach (var item in dataStatus)
                    {
                        StatusddlList list = new StatusddlList();
                        list.statusId = item.Id;
                        list.statusname = item.Status;
                        objstatus.Add(list);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return objstatus;
        }

        //public bool ProjectExsistence(CreateProjectDto input)
        //{
        //    return _projectRepository.GetAll().Where(e => e.ProjectName == input.ProjectName).Any();

        //}
        public bool ProjectExsistence(ExsistProjectValidationDto input)
        {
            if (input.Id.HasValue)
            {
                return _projectRepository.GetAll().Where(e => e.ProjectName == input.ProjectName && e.Id != input.Id).Any();
            }
            else
            {
                return _projectRepository.GetAll().Where(e => e.ProjectName == input.ProjectName).Any();
            }
        }


        //public bool ProjectExsistenceById(CreateProjectDto input)
        //{
        //    return _projectRepository.GetAll().Where(e => e.ProjectName == input.ProjectName && e.Id != input.Id).Any();
        //}

        public async Task UpdateProject(EditProjectDto input)
        {
            var project = await _projectRepository.GetAsync(input.Id);

            project.ProjectName = input.ProjectName;
            project.Description = input.Description;
            project.BeneficiaryCompanyId = input.BeneficiaryCompanyId;
            project.StartDate = input.StartDate;
            //project.EndDate = input.EndDate;
            //project.TeamDeadline = input.TeamDeadline;
            if (input.EndDate.HasValue)
            {
                project.EndDate = input.EndDate.Value;
            }
            else
            {
                project.EndDate = null;
            }
            if (input.TeamDeadline.HasValue)
            {
                project.TeamDeadline = input.TeamDeadline.Value;
            }
            else
            {
                project.TeamDeadline = null;
            }
            if (input.ActualEndDate.HasValue)
            {
                project.ActualEndDate = input.ActualEndDate.Value;
            }
            else
            {
                project.ActualEndDate = null;
            }
            project.CompanyName = input.CompanyName;
            if (input.Marketing_LeaderId.HasValue)
            {
                project.Marketing_LeaderId = input.Marketing_LeaderId.Value;
            }
            else
            {
                project.Marketing_LeaderId = null;
            }
            //project.Price = input.Price;
            project.ClientId = input.ClientId;
            await _appNotifier.NewProjectRegisteredAsync();
            await _projectRepository.UpdateAsync(project);
        }

        public async Task<ListResultDto<ProjectDetailsDto>> GetprojectDetailsList(EntityDto input)
        {
            List<ProjectDetailsDto> obj = new List<ProjectDetailsDto>();
            var data = _repositoryProjectsDetails.GetAll().Where(x => x.ProjectId == input.Id).ToList();
            if (data.Count > 0)
            {

                foreach (var item in data)
                {
                    obj.Add(new ProjectDetailsDto
                    {
                        Id = item.Id,
                        ProjecttypeId = item.TypeID,
                        Typeprice = item.Price,
                        hours = item.hours,
                        ProjectType = GetProjectType(item.TypeID),
                        Comments = item.Comments,
                        CostforCompany = item.CostforCompany,
                        MilestoneCount = GetProjectMilestoneCount(item.Id)
                    });
                }
            }
            var dataList = obj.OrderByDescending(x => x.Id).ToList();
            return new ListResultDto<ProjectDetailsDto>(dataList.MapTo<List<ProjectDetailsDto>>());
        }

        private string GetProjectType(int projectTypeId)
        {
            string ProjectTypeName = "";
            try
            {
                var data = _projecttypeRepository.GetAll().Where(x => x.Id == projectTypeId).FirstOrDefault();
                if (data != null)
                {
                    ProjectTypeName = data.ProjectTypeName;
                }
            }
            catch (Exception ex)
            {
            }
            return ProjectTypeName;
        }

        private int GetProjectMilestoneCount(int projectTypeId)
        {
            int entity = (from pm in _projectmilestoneRepository.GetAll()
                          join pt in _repositoryProjectsDetails.GetAll()
                          on pm.ProjectTypeId equals pt.Id
                          where pm.IsDeleted == false && pm.ProjectTypeId == projectTypeId
                          group pm by new
                          {
                              pm.ProjectTypeId

                          } into g
                          select new ProjectDetailsDto
                          {

                              MilestoneCount = g.Count()
                          }).Select(x => x.MilestoneCount).FirstOrDefault();


            return entity;
        }

        public async Task<ListResultDto<GetStatusDto>> GetStatus()
        {
            var enumData = (from s in _projectStatusRepository.GetAll()
                            select new GetStatusDto
                            {
                                Id = s.Id,
                                Status = s.Status
                            }).OrderBy(x => x.Status).ToList();

            //return new ListResultDto<GetStatusDto>(ObjectMapper.Map<List<GetStatusDto>>(enumData));
            return new ListResultDto<GetStatusDto>(enumData.MapTo<List<GetStatusDto>>());
        }

        public async Task<EditProjectTypeByProject> GetProjectTypeByProjectEdit(MasterInputs input)
        {
            var data = (_repositoryProjectsDetails.GetAll().Where(x => x.Id == input.Id).FirstOrDefault()).MapTo<EditProjectTypeByProject>();
            return data;
        }

        public async Task UpdateProjectType(ProjectTypeUpdate input)
        {
            var data = await _repositoryProjectsDetails.GetAsync(input.Id);
            if (data != null)
            {
                data.Price = input.Price;
                data.hours = input.hours;
                data.Comments = input.Comments;
                data.IsOutSource = input.IsOutSource;
                data.CostforCompany = input.CostforCompany.HasValue ? input.CostforCompany : null;
                if (!data.IsOutSource.HasValue)
                {
                    data.CostforCompany = null;
                }
                await _repositoryProjectsDetails.UpdateAsync(data);
                decimal totalprice = 0;
                int hours = 0;
                var getdata = _repositoryProjectsDetails.GetAll().Where(x => x.ProjectId == data.ProjectId).ToList();
                if (getdata.Count > 0)
                {
                    foreach (var item in getdata)
                    {
                        //  decimal pricevalue = item.Price;
                        int currenthours = 0;
                        //if (data.IsOutSource.Value)
                        //{
                        //    pricevalue = input.CostforCompany.GetValueOrDefault() + pricevalue;
                        //}
                        //else
                        //{
                        //    pricevalue = item.Price + input.Price;
                        //}
                        totalprice = item.Price + totalprice;
                        int.TryParse(item.hours.ToString(), out currenthours);
                        hours = hours + currenthours;
                    }
                }

                var proj = await _projectRepository.GetAsync(data.ProjectId);

                if (proj != null)
                {
                    //decimal pricevalue = proj.Price;
                    //if (data.IsOutSource.Value)
                    //{
                    //    proj.Price = pricevalue + input.CostforCompany.Value;
                    //}
                    //else
                    //{
                    //    proj.Price = pricevalue + input.Price;
                    //}
                    proj.Price = totalprice;
                    proj.totalhours = hours;
                    await _projectRepository.UpdateAsync(proj);
                }
            }
        }

        public async Task CreateProjectType(createProjectTypeByProjectDto input)
        {
            Projecttype_details obj = new Projecttype_details();
            obj.ProjectId = input.ProjectId;
            obj.TypeID = input.projecttypeId;
            obj.Price = input.Price;
            obj.hours = input.hours;
            obj.Comments = input.Comments;
            obj.IsOutSource = input.IsOutSource;
            obj.CostforCompany = input.CostforCompany.HasValue ? input.CostforCompany : null;
            //var result = input.MapTo<Projecttype_details>();
            _repositoryProjectsDetails.Insert(obj);
            decimal totalprice = 0;
            int hours = 0;
            var getdata = _repositoryProjectsDetails.GetAll().Where(x => x.ProjectId == input.ProjectId).ToList();
            if (getdata.Count > 0)
            {
                foreach (var item in getdata)
                {
                    int currenthours = 0;
                    decimal pricevalue = item.Price;
                    //item.IsOutSource = item.IsOutSource.HasValue ? true : false;
                    //if (obj.IsOutSource.Value)
                    //{
                    //    pricevalue = input.CostforCompany.GetValueOrDefault() + pricevalue;
                    //}
                    //else
                    //{
                    //    pricevalue = item.Price + input.Price;
                    //}

                    int.TryParse(item.hours.ToString(), out currenthours);
                    hours = hours + currenthours;
                }
            }
            var proj = _projectRepository.Get(input.ProjectId);

            if (proj != null)
            {
                decimal pricevalue = proj.Price;
                if (obj.IsOutSource.HasValue) // vikas Changed
                {
                    proj.Price = pricevalue + input.CostforCompany.Value;
                }
                else
                {
                    proj.Price = pricevalue + input.Price;
                }

                int inputHours = 0;
                int.TryParse(input.hours, out inputHours);
                hours = hours + inputHours;
                proj.totalhours = hours;
                await _projectRepository.UpdateAsync(proj);
            }
        }

        //public async Task CreateProjectType(createProjectTypeByProjectDto input)
        //{
        //    Projecttype_details obj = new Projecttype_details();
        //    obj.ProjectId = input.ProjectId;
        //    obj.TypeID = input.projecttypeId;
        //    obj.Price = input.Price;
        //    obj.hours = input.hours;
        //    obj.Comments = input.Comments;
        //    obj.IsOutSource = input.IsOutSource.HasValue ? true : false;
        //    obj.CostforCompany = input.CostforCompany.HasValue ? input.CostforCompany : null;
        //    //var result = input.MapTo<Projecttype_details>();
        //    _repositoryProjectsDetails.Insert(obj);
        //    decimal totalprice = 0;
        //    int hours = 0;
        //    var getdata = _repositoryProjectsDetails.GetAll().Where(x => x.ProjectId == input.ProjectId).ToList();
        //    if (getdata.Count > 0)
        //    {
        //        foreach (var item in getdata)
        //        {
        //            int currenthours = 0;
        //            item.IsOutSource = input.IsOutSource.HasValue ? true : false;
        //            if (item.IsOutSource.Value)
        //            {
        //                totalprice = item.CostforCompany.GetValueOrDefault() + totalprice;
        //            }
        //            else        
        //            {
        //                totalprice = item.Price + totalprice;
        //            }

        //            int.TryParse(item.hours.ToString(), out currenthours);
        //            hours = hours + currenthours;
        //        }
        //    }
        //    var proj = _projectRepository.Get(input.ProjectId);

        //    if (proj != null)
        //    {

        //        if (obj.IsOutSource.Value)
        //        {
        //            proj.Price = totalprice + input.CostforCompany.Value;
        //        }
        //        else
        //        {
        //            proj.Price = totalprice + input.Price;
        //        }
        //        int inputHours = 0;
        //        int.TryParse(input.hours, out inputHours);
        //        hours = hours + inputHours;
        //        proj.totalhours = hours;
        //        await _projectRepository.UpdateAsync(proj);
        //    }
        //}
        public async Task DeleteProjectType(EntityDto input)
        {
            await _repositoryProjectsDetails.DeleteAsync(input.Id);

            var data = await _repositoryProjectsDetails.GetAsync(input.Id);
            decimal totalprice = 0;
            int hours = 0;
            var getdata = _repositoryProjectsDetails.GetAll().Where(x => x.ProjectId == data.ProjectId).ToList();
            if (getdata.Count > 0)
            {
                foreach (var item in getdata)
                {
                    int currenthours = 0;
                    if (item.Id != data.Id)
                    {
                        totalprice = item.Price + totalprice;
                        int.TryParse(item.hours, out currenthours);
                        hours = hours + currenthours;
                    }
                }

            }
            var proj = await _projectRepository.GetAsync(data.ProjectId);

            if (proj != null)
            {
                proj.Price = totalprice;
                proj.totalhours = hours;
                await _projectRepository.UpdateAsync(proj);
            }
        }

        public async Task UpdateProjectStatus(UpdateProjectStatusDto input)
        {
            if (input.UpdateStatusId != null && input.UpdateStatusId != 0)
            {
                //for (int i = 0; i < input.UpdateStatus.Length; i++)
                //{
                var data = _projectRepository.Get(input.Id);
                data.Status = input.UpdateStatusId;
                _projectRepository.Update(data);
                //}
            }
        }

        public async Task UpdateProjectPriority(UpdateProjectStatusDto input)
        {
            var data = _projectRepository.Get(input.Id);
            if (input.UpdatePrio != null && input.UpdatePrio != 0)
            {

                data.Priority = input.UpdatePrio;
            }
            else
            {
                data.Priority = null;
            }
            _projectRepository.Update(data);
        }

        //Generate Invoice request 

        public ListResultDto<ListInvoiceRequestDto> GetInvoiceRequest(int projectid)
        {

            var tasks = (from e1 in _projectRepository.GetAll()
                         join e2 in _invoiceRequestRepository.GetAll() on e1.Id equals e2.ProjectId
                         where (e2.ProjectId == projectid)
                         select new ListInvoiceRequestDto
                         {
                             Id = e2.Id,
                             Amount = e2.Amount,
                             Comment = e2.Comment,
                             CreationTime = e2.CreationTime,
                             Status = e2.Status,
                             ProjectName = e1.ProjectName,
                             ProjectId = e2.ProjectId
                         }).OrderByDescending(x => x.Id)
                     .ToList();
            return new ListResultDto<ListInvoiceRequestDto>(tasks.MapTo<List<ListInvoiceRequestDto>>());

            //return tasks;
        }

        public async Task CreateInvoiceRequest(InvoiceRequestDto input)
        {

            var Userlist = (from u in _userRepository.GetAll()
                            join ur in _userRoleRepository.GetAll()
                            on u.Id equals ur.UserId
                            join r in _roleRepository.GetAll()
                            on ur.RoleId equals r.Id
                            where (r.Name == "Accounts" || r.Name == "Admin") && u.IsActive == true
                            select u).ToList();

            invoicerequest Invreqobj = new invoicerequest();
            Invreqobj.TypeId = 2;
            Invreqobj.Comment = input.Comment;
            Invreqobj.Status = 0; // 0=pending,1=approved
            Invreqobj.Amount = input.Amount;
            Invreqobj.ProjectId = input.Id;
            var InvId = Invreqobj.MapTo<invoicerequest>();
            await _invoiceRequestRepository.InsertAsync((invoicerequest)Invreqobj);

            //get Project Name
            var projectname = _projectRepository.GetAll().Where(x => x.Id == input.Id).Select(x => x.ProjectName).FirstOrDefault();

            User[] users = Userlist.ToArray();   //all accountant and admin users

            #region Push Notification
            string message = "New invoice request generated for project " + projectname;
            string severity = "success";

            await _appNotifier.SendInvoiceRequestMessageAsync(
                users,
                message,
                severity.ToPascalCase(CultureInfo.InvariantCulture).ToEnum<NotificationSeverity>()
                );
            #endregion

            #region sendmail
            GetInvoiceRequestDto item = new GetInvoiceRequestDto();
            //item.FromEmail = _userRepository.GetAll().Where(x => x.Id == userId).Select(x => x.EmailAddress).FirstOrDefault();
            item.ProjectName = projectname;
            item.EmailTitle = "New Invoice Request is generated for Project " + item.ProjectName;
            item.Amount = input.Amount;
            item.Comment = input.Comment;
            foreach (var email in Userlist)
            {
                item.toEmail = email.EmailAddress;
                await _userEmailer.SendEmailForNewInvoiceRequest(item);
            }
            #endregion

        }

        public PagedResultDto<ListInvoiceRequestDto> GetInvoicerequestListByPrject(GetInvoiceInput input)
        {
            try
            {
                var tasks = (from e1 in _projectRepository.GetAll()
                             join e2 in _invoiceRequestRepository.GetAll() on e1.Id equals e2.ProjectId
                             join e3 in _userRepository.GetAll() on e2.CreatorUserId equals e3.Id
                             join e4 in _clientRepository.GetAll() on e1.ClientId equals e4.Id
                             join e5 in _userRepository.GetAll() on e1.Marketing_LeaderId equals (int)e5.Id
                             where (e2.TypeId == 2)
                             select new ListInvoiceRequestDto
                             {
                                 Id = e2.Id,
                                 ProjectName = e1.ProjectName,
                                 Amount = e2.Amount,
                                 Comment = e2.Comment,
                                 CreatorName = e3.Name,
                                 CreationTime = e2.CreationTime,
                                 Status = e2.Status,
                                 ClientName = e4.ClientName,
                                 MarketingPerson = e5.Name,
                                 TypeId = e2.TypeId

                             })
                             .WhereIf(input.Status.HasValue, x => x.Status == input.Status)

                             .WhereIf(!input.ProjectName.IsNullOrEmpty() && input.ProjectName != "", p => p.ProjectName.ToLower().Contains(input.ProjectName.ToLower()));


                //.OrderByDescending(p => p.Id).ToList();

                var totalcount = tasks.Count();
                //var servicePageBy = tasks.AsQueryable().PageBy(input);
                var servicePageBy = tasks.OrderBy(input.Sorting).PageBy(input).ToList();

                return new PagedResultDto<ListInvoiceRequestDto>(totalcount, servicePageBy.MapTo<List<ListInvoiceRequestDto>>());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public PagedResultDto<ListInvoiceRequestDto> GetInvoicerequestListByService(GetInvoiceInput input)
        {
            try
            {
                var domainname = _manageserviceRepository.GetAll().Select(x => x.DomainName).ToList().Distinct();


                var tasks = (from e1 in _invoiceRequestRepository.GetAll()
                             join e2 in _manageserviceRepository.GetAll() on e1.ServiceId equals e2.Id

                             join e3 in _userRepository.GetAll() on e1.CreatorUserId equals e3.Id

                             join e4 in _clientRepository.GetAll() on e2.ClientId equals e4.Id
                              into distclient
                             from e4 in distclient.DefaultIfEmpty()
                             join e5 in _userRepository.GetAll() on e2.EmployeeId equals (int)e5.Id
                              into distu
                             from e5 in distu.DefaultIfEmpty()
                             join e6 in _serviceRepository.GetAll() on e2.ServiceId equals e6.Id
                             into dists
                             from e6 in dists.DefaultIfEmpty()
                             join e7 in _serverRepository.GetAll() on e2.ServerType equals e7.Id
                          into distserver
                             from e7 in distserver.DefaultIfEmpty()
                             join e8 in _typenameRepository.GetAll() on e2.TypeName equals e8.Id
                               into distrtype
                             from e8 in distrtype.DefaultIfEmpty()
                             join e9 in _serviceRequestRepository.GetAll() on e1.ServiceReqId equals e9.Id
                               into distst
                             from e9 in distst.DefaultIfEmpty()

                             where (e1.TypeId == 1)
                             select new ListInvoiceRequestDto
                             {
                                 Id = e1.Id,
                                 DomainName = e2.DomainName,
                                 Amount = e1.Amount,
                                 Comment = e1.Comment,
                                 InvoiceNote = e1.InvoiceNote,
                                 CreatorName = e3.Name,
                                 CreationTime = e1.CreationTime,
                                 Status = e1.Status,
                                 ClientName = e4.ClientName,
                                 MarketingPerson = e5.Name + " " + e5.Surname,
                                 ServiceName = e6.Name,
                                 TypeId = e1.TypeId,
                                 HostingSpace = e2.HostingSpace,
                                 TypeName = e8.Name,
                                 NoOfEmail = e2.NoOfEmail,
                                 ServerName = e7.Name,
                                 Typeofssl = e2.Typeofssl,
                                 Title = e2.Title,
                                 ActionName = distst.OrderByDescending(x => x.Id).Select(x => x.ActionName).FirstOrDefault(),
                                 Credits = e2.Credits,
                                 DatabaseSpace = e2.DatabaseSpace,
                                 Period = e1.Period,
                             })
                          .WhereIf(input.Status.HasValue, x => x.Status == input.Status)

                          .WhereIf(!input.DomainName.IsNullOrEmpty() && input.DomainName != "", p => p.DomainName.ToLower().Contains(input.DomainName.ToLower())).Distinct();
                //.OrderByDescending(p => p.CreationTime).ToList();

                var totalcount = tasks.Count();
                //var servicePageBy = tasks.AsQueryable().PageBy(input);
                var servicePageBy = tasks.OrderBy(input.Sorting).PageBy(input).ToList();

                return new PagedResultDto<ListInvoiceRequestDto>(totalcount, servicePageBy.MapTo<List<ListInvoiceRequestDto>>());
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task Updatetocancelservice(int id)
        {
            try
            {
                var per = await _invoiceRequestRepository.FirstOrDefaultAsync(id);

                per.Status = 2;
                //per.CancelDate = DateTime.Now;
                //per.RenewalDate = DateTime.Now;

                await _invoiceRequestRepository.UpdateAsync(per);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task Enabledisable(int id)
        {
            var per = _projectRepository.Get(id);
            if (per.IsEnable == 1)
            {
                per.IsEnable = 0;
            }
            else
            {
                per.IsEnable = 1;
            }
            await _projectRepository.UpdateAsync(per);
        }


        public PagedResultDto<ProjectDto> GetAllProjectList(GetProjectDto input)
        {
            bool flag = false;

            try
            {
                long curId = (int)_abpSession.UserId;
                var rolelist = (from u in _userRepository.GetAll()
                                join ur in _userRoleRepository.GetAll()
                                on u.Id equals ur.UserId
                                join r in _roleRepository.GetAll()
                                on ur.RoleId equals r.Id
                                where u.Id == curId
                                select r).ToList();
                if (rolelist.Any(x => x.Name.ToLower() == "admin" || x.Name.ToLower() == "marketing leader"))
                {
                    flag = true;
                }

                var User_List = _userRepository.GetAll();
                // var uid = (int)User_List.Where(u => u.Id == _abpSession.UserId).Select(t => t.Id).FirstOrDefault();
                var username = (from user in User_List
                                join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                                from ur in urJoined.DefaultIfEmpty()
                                join us in _roleRepository.GetAll() on ur.RoleId equals us.Id into usJoined
                                from us in usJoined.DefaultIfEmpty()
                                where (us != null && user.Id == _abpSession.UserId && us.Name == "Marketing Leader")
                                select (us.DisplayName)).FirstOrDefault();


                // var projrepo = _invoiceRequestRepository.GetAll();
                var actualhours = _timesheetRepository.GetAll()
                            .GroupBy(a => a.ProjectId)
                            .Select(a => new { Hours = a.Sum(b => b.Hours), ProjectId = a.Key })
                            .OrderByDescending(a => a.Hours)
                            .ToList();

                var cc = (from p in _projectRepository.GetAll()
                          join Pro in _productionRepository.GetAll() on p.Id equals Pro.Projectid into productiondata
                          join Invreq in _invoiceRequestRepository.GetAll() on p.Id equals Invreq.ProjectId into projrepo
                          join c in _clientRepository.GetAll()
                          on p.ClientId equals c.Id into clientrepo
                          from ud in clientrepo.DefaultIfEmpty()

                              //join t in _timesheetRepository.GetAll()
                              //on p.Id equals t.ProjectId
                          join ur in _userRepository.GetAll()
                          on (Int32)p.Marketing_LeaderId equals ur.Id into userd
                          from ur in userd.DefaultIfEmpty()
                          join s in _projectStatusRepository.GetAll()
                          on p.Status equals s.Id
                          select new ProjectDto
                          {
                              Id = p.Id,
                              ProjectName = p.ProjectName,
                              Description = p.Description,
                              StartDate = p.StartDate,
                              EndDate = p.EndDate,
                              Price = p.Price,
                              ClientName = ud.ClientName,
                              Marketing_LeaderId = p.Marketing_LeaderId.HasValue ? p.Marketing_LeaderId.Value : 0,
                              MarketingLeadName = ur.Name + " " + ur.Surname,
                              ClientId = p.ClientId,
                              ProjectStatusId = p.Status,
                              ProjectStatus = s.Status,
                              totalhours = p.totalhours,
                              actualhours = 0,
                              username = username != null ? "Marketing Leader" : "",
                              Priority = p.Priority,
                              Invoiceamount = projrepo.Where(x => x.Status == 1).Sum(x => x.Amount),
                              pricesum = projrepo.Where(p => p.Status == 0 || p.Status == 1).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(p => p.Status == 10).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(t => t.ProjectId == p.Id && p.Status == 10).Select(t => t.Amount).Sum(),
                              PendingAmount = ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)) == null ? p.Price : ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)),
                              enableStatus = p.IsEnable,
                              typeshow = flag
                          })
                .WhereIf(!input.ProjectName.IsNullOrEmpty(), s => s.ProjectName.ToLower().Contains(input.ProjectName.ToLower()) /*|| s.ClientName.ToLower().Contains(input.ProjectName.ToLower()) || s.MarketingLeadName.ToLower().Contains(input.ProjectName.ToLower())*/)
                .WhereIf(input.ClientId.HasValue, s => s.ClientId == input.ClientId.Value)
               .WhereIf(input.ProjectId.HasValue, s => s.Id == input.ProjectId.Value)
               .WhereIf(input.MarketingleadId.HasValue, s => s.Marketing_LeaderId == input.MarketingleadId.Value)
                .WhereIf(input.ProjectStatusId.Length > 0, s => input.ProjectStatusId.Contains(s.ProjectStatusId));
                //var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
                var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
                foreach (var item in ccData)
                {
                    item.objProjectStatusList = GetStatusList();
                    item.actualhours = actualhours.Where(x => x.ProjectId == item.Id).Select(x => x.Hours).FirstOrDefault();
                    item.hourPercentage = item.totalhours > 0 ? ((decimal)actualhours.Where(s => s.ProjectId == item.Id).Select(s => s.Hours).DefaultIfEmpty(0).Sum()) * 100 / item.totalhours : 0;
                    item.username = username;

                    //foreach (var datahours in actualhours)
                    //{
                    //    if (datahours.ProjectId==item.Id)
                    //    {
                    //        item.hours = datahours.Hours.ToString();
                    //    }
                    //    else
                    //    {
                    //        item.hours = "0";
                    //    }
                    //}
                }
                var ccCount = cc.Count();

                return new PagedResultDto<ProjectDto>(ccCount, ccData.MapTo<List<ProjectDto>>());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        public PagedResultDto<ProjectDto> GetActiveProjectList(GetProjectDto input)
        {
            bool flag = false;

            try
            {
                long curId = (int)_abpSession.UserId;
                var rolelist = (from u in _userRepository.GetAll()
                                join ur in _userRoleRepository.GetAll()
                                on u.Id equals ur.UserId
                                join r in _roleRepository.GetAll()
                                on ur.RoleId equals r.Id
                                where u.Id == curId
                                select r).ToList();
                if (rolelist.Any(x => x.Name.ToLower() == "admin" || x.Name.ToLower() == "marketing leader"))
                {
                    flag = true;
                }

                var User_List = _userRepository.GetAll();
                // var uid = (int)User_List.Where(u => u.Id == _abpSession.UserId).Select(t => t.Id).FirstOrDefault();
                var username = (from user in User_List
                                join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                                from ur in urJoined.DefaultIfEmpty()
                                join us in _roleRepository.GetAll() on ur.RoleId equals us.Id into usJoined
                                from us in usJoined.DefaultIfEmpty()
                                where (us != null && user.Id == _abpSession.UserId && us.Name == "Marketing Leader")
                                select (us.DisplayName)).FirstOrDefault();


                // var projrepo = _invoiceRequestRepository.GetAll();
                var actualhours = _timesheetRepository.GetAll()
                            .GroupBy(a => a.ProjectId)
                            .Select(a => new { Hours = a.Sum(b => b.Hours), ProjectId = a.Key })
                            .OrderByDescending(a => a.Hours)
                            .ToList();

                var cc = (from p in _projectRepository.GetAll()
                          join Pro in _productionRepository.GetAll() on p.Id equals Pro.Projectid into productiondata
                          join Invreq in _invoiceRequestRepository.GetAll() on p.Id equals Invreq.ProjectId into projrepo
                          join c in _clientRepository.GetAll()
                          on p.ClientId equals c.Id into clientrepo
                          from ud in clientrepo.DefaultIfEmpty()

                              //join t in _timesheetRepository.GetAll()
                              //on p.Id equals t.ProjectId
                          join ur in _userRepository.GetAll()
                          on (Int32)p.Marketing_LeaderId equals ur.Id into userd
                          from ur in userd.DefaultIfEmpty()
                          join s in _projectStatusRepository.GetAll()
                          on p.Status equals s.Id
                          where p.Status == 4 || p.Status == 5 || p.Status == 6 || p.Status == 7 || p.Status == 10
                          select new ProjectDto
                          {
                              Id = p.Id,
                              ProjectName = p.ProjectName,
                              Description = p.Description,
                              StartDate = p.StartDate,
                              EndDate = p.EndDate,
                              Price = p.Price,
                              ClientName = ud.ClientName,
                              Marketing_LeaderId = p.Marketing_LeaderId.HasValue ? p.Marketing_LeaderId.Value : 0,
                              MarketingLeadName = ur.Name + " " + ur.Surname,
                              ClientId = p.ClientId,
                              ProjectStatusId = p.Status,
                              ProjectStatus = s.Status,
                              totalhours = p.totalhours,
                              actualhours = 0,
                              username = username != null ? "Marketing Leader" : "",
                              Priority = p.Priority,
                              Invoiceamount = projrepo.Where(x => x.Status == 1).Sum(x => x.Amount),
                              pricesum = projrepo.Where(p => p.Status == 0 || p.Status == 1).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(p => p.Status == 10).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(t => t.ProjectId == p.Id && p.Status == 10).Select(t => t.Amount).Sum(),
                              PendingAmount = ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)) == null ? p.Price : ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)),
                              enableStatus = p.IsEnable,
                              typeshow = flag
                          })
                .WhereIf(!input.ProjectName.IsNullOrEmpty(), s => s.ProjectName.ToLower().Contains(input.ProjectName.ToLower()) /*|| s.ClientName.ToLower().Contains(input.ProjectName.ToLower()) || s.MarketingLeadName.ToLower().Contains(input.ProjectName.ToLower())*/)
                .WhereIf(input.ClientId.HasValue, s => s.ClientId == input.ClientId.Value)
               .WhereIf(input.ProjectId.HasValue, s => s.Id == input.ProjectId.Value)
               .WhereIf(input.MarketingleadId.HasValue, s => s.Marketing_LeaderId == input.MarketingleadId.Value)
                .WhereIf(input.ProjectStatusId.Length > 0, s => input.ProjectStatusId.Contains(s.ProjectStatusId));
                //var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
                var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
                foreach (var item in ccData)
                {
                    item.objProjectStatusList = GetStatusList();
                    item.actualhours = actualhours.Where(x => x.ProjectId == item.Id).Select(x => x.Hours).FirstOrDefault();
                    item.hourPercentage = item.totalhours > 0 ? ((decimal)actualhours.Where(s => s.ProjectId == item.Id).Select(s => s.Hours).DefaultIfEmpty(0).Sum()) * 100 / item.totalhours : 0;
                    item.username = username;

                    //foreach (var datahours in actualhours)
                    //{
                    //    if (datahours.ProjectId==item.Id)
                    //    {
                    //        item.hours = datahours.Hours.ToString();
                    //    }
                    //    else
                    //    {
                    //        item.hours = "0";
                    //    }
                    //}
                }
                var ccCount = cc.Count();

                return new PagedResultDto<ProjectDto>(ccCount, ccData.MapTo<List<ProjectDto>>());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }



        public PagedResultDto<ProjectDto> GetAmcProjectList(GetProjectDto input)
        {
            try
            {
                var User_List = _userRepository.GetAll();
                // var uid = (int)User_List.Where(u => u.Id == _abpSession.UserId).Select(t => t.Id).FirstOrDefault();
                var username = (from user in User_List
                                join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                                from ur in urJoined.DefaultIfEmpty()
                                join us in _roleRepository.GetAll() on ur.RoleId equals us.Id into usJoined
                                from us in usJoined.DefaultIfEmpty()
                                where (us != null && user.Id == _abpSession.UserId)
                                select (us.DisplayName)).FirstOrDefault();


                // var projrepo = _invoiceRequestRepository.GetAll();
                var actualhours = _timesheetRepository.GetAll()
                            .GroupBy(a => a.ProjectId)
                            .Select(a => new { Hours = a.Sum(b => b.Hours), ProjectId = a.Key })
                            .OrderByDescending(a => a.Hours)
                            .ToList();

                var cc = (from p in _projectRepository.GetAll()
                          join Pro in _productionRepository.GetAll() on p.Id equals Pro.Projectid into productiondata
                          join Invreq in _invoiceRequestRepository.GetAll() on p.Id equals Invreq.ProjectId into projrepo
                          join c in _clientRepository.GetAll()
                          on p.ClientId equals c.Id

                          //join t in _timesheetRepository.GetAll()
                          //on p.Id equals t.ProjectId
                          join ud in _userRepository.GetAll()
                          on (Int32)p.Marketing_LeaderId equals ud.Id into userd
                          from ud in userd.DefaultIfEmpty()
                          join s in _projectStatusRepository.GetAll()
                          on p.Status equals s.Id
                          where p.Status == 9
                          select new ProjectDto
                          {
                              Id = p.Id,
                              ProjectName = p.ProjectName,
                              Description = p.Description,
                              StartDate = p.StartDate,
                              EndDate = p.EndDate,
                              Price = p.Price,
                              ClientName = c.ClientName,
                              Marketing_LeaderId = p.Marketing_LeaderId.HasValue ? p.Marketing_LeaderId.Value : 0,
                              MarketingLeadName = ud.Name + " " + ud.Surname,
                              ClientId = p.ClientId,
                              ProjectStatusId = p.Status,
                              ProjectStatus = s.Status,
                              totalhours = p.totalhours,
                              actualhours = 0,
                              username = "",
                              Priority = p.Priority,
                              Invoiceamount = projrepo.Where(x => x.Status == 1).Sum(x => x.Amount),
                              pricesum = projrepo.Where(p => p.Status == 0 || p.Status == 1).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(p => p.Status == 10).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(t => t.ProjectId == p.Id && p.Status == 10).Select(t => t.Amount).Sum(),
                              PendingAmount = ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)) == null ? p.Price : ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)),
                              enableStatus = p.IsEnable
                          })
                .WhereIf(!input.ProjectName.IsNullOrEmpty(), s => s.ProjectName.ToLower().Contains(input.ProjectName.ToLower()) /*|| s.ClientName.ToLower().Contains(input.ProjectName.ToLower()) || s.MarketingLeadName.ToLower().Contains(input.ProjectName.ToLower())*/)
                .WhereIf(input.ClientId.HasValue, s => s.ClientId == input.ClientId.Value)
               .WhereIf(input.ProjectId.HasValue, s => s.Id == input.ProjectId.Value)
               .WhereIf(input.MarketingleadId.HasValue, s => s.Marketing_LeaderId == input.MarketingleadId.Value)
                .WhereIf(input.ProjectStatusId.Length > 0, s => input.ProjectStatusId.Contains(s.ProjectStatusId));
                //var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
                var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
                foreach (var item in ccData)
                {
                    item.objProjectStatusList = GetStatusList();
                    item.actualhours = actualhours.Where(x => x.ProjectId == item.Id).Select(x => x.Hours).FirstOrDefault();
                    item.username = username;

                    //foreach (var datahours in actualhours)
                    //{
                    //    if (datahours.ProjectId==item.Id)
                    //    {
                    //        item.hours = datahours.Hours.ToString();
                    //    }
                    //    else
                    //    {
                    //        item.hours = "0";
                    //    }
                    //}
                }
                var ccCount = cc.Count();

                return new PagedResultDto<ProjectDto>(ccCount, ccData.MapTo<List<ProjectDto>>());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public List<ProjectDto> ExportGetCompletedProjectList(ImportGetProjectDto input)
        {
            try
            {
                var User_List = _userRepository.GetAll();
                // var uid = (int)User_List.Where(u => u.Id == _abpSession.UserId).Select(t => t.Id).FirstOrDefault();
                var username = (from user in User_List
                                join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                                from ur in urJoined.DefaultIfEmpty()
                                join us in _roleRepository.GetAll() on ur.RoleId equals us.Id into usJoined
                                from us in usJoined.DefaultIfEmpty()
                                where (us != null && user.Id == _abpSession.UserId)
                                select (us.DisplayName)).FirstOrDefault();


                // var projrepo = _invoiceRequestRepository.GetAll();
                var actualhours = _timesheetRepository.GetAll()
                            .GroupBy(a => a.ProjectId)
                            .Select(a => new { Hours = a.Sum(b => b.Hours), ProjectId = a.Key })
                            .OrderByDescending(a => a.Hours)
                            .ToList();

                var cc = (from p in _projectRepository.GetAll()
                          join Pro in _productionRepository.GetAll() on p.Id equals Pro.Projectid into productiondata
                          join Invreq in _invoiceRequestRepository.GetAll() on p.Id equals Invreq.ProjectId into projrepo
                          join c in _clientRepository.GetAll()
                          on p.ClientId equals c.Id

                          //join t in _timesheetRepository.GetAll()
                          //on p.Id equals t.ProjectId
                          join ud in _userRepository.GetAll()
                          on (Int32)p.Marketing_LeaderId equals ud.Id into userd
                          from ud in userd.DefaultIfEmpty()
                          join s in _projectStatusRepository.GetAll()
                          on p.Status equals s.Id
                          where p.Status == 2
                          select new ProjectDto
                          {
                              Id = p.Id,
                              ProjectName = p.ProjectName,
                              Description = p.Description,
                              StartDate = p.StartDate,
                              EndDate = p.EndDate,
                              Price = p.Price,
                              ClientName = c.ClientName,
                              Marketing_LeaderId = p.Marketing_LeaderId.HasValue ? p.Marketing_LeaderId.Value : 0,
                              MarketingLeadName = ud.Name + " " + ud.Surname,
                              ClientId = p.ClientId,
                              ProjectStatusId = p.Status,
                              ProjectStatus = s.Status,
                              totalhours = p.totalhours,
                              actualhours = 0,
                              username = "",
                              Priority = p.Priority,
                              Invoiceamount = projrepo.Where(x => x.Status == 1).Sum(x => x.Amount),
                              pricesum = projrepo.Where(p => p.Status == 0 || p.Status == 1).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(p => p.Status == 10).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(t => t.ProjectId == p.Id && p.Status == 10).Select(t => t.Amount).Sum(),
                              PendingAmount = ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)) == null ? p.Price : ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)),
                              enableStatus = p.IsEnable
                          })
                .WhereIf(!input.SearchBy.IsNullOrEmpty(), s => s.ProjectName.ToLower().Contains(input.SearchBy.ToLower()))
                .WhereIf(input.ClientId.HasValue, s => s.ClientId == input.ClientId.Value)
               .WhereIf(input.MarketingleadId.HasValue, s => s.Marketing_LeaderId == input.MarketingleadId.Value);
                var ccData = cc.OrderByDescending(s => s.Id)
                               .ToList(); ;
                foreach (var item in ccData)
                {
                    item.objProjectStatusList = GetStatusList();
                    item.actualhours = actualhours.Where(x => x.ProjectId == item.Id).Select(x => x.Hours).FirstOrDefault();
                    item.username = username;

                }

                return ccData;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        public PagedResultDto<ProjectDto> GetCompletedProjectList(GetProjectDto input)
        {
            try
            {
                var User_List = _userRepository.GetAll();
                // var uid = (int)User_List.Where(u => u.Id == _abpSession.UserId).Select(t => t.Id).FirstOrDefault();
                var username = (from user in User_List
                                join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                                from ur in urJoined.DefaultIfEmpty()
                                join us in _roleRepository.GetAll() on ur.RoleId equals us.Id into usJoined
                                from us in usJoined.DefaultIfEmpty()
                                where (us != null && user.Id == _abpSession.UserId)
                                select (us.DisplayName)).FirstOrDefault();


                // var projrepo = _invoiceRequestRepository.GetAll();
                var actualhours = _timesheetRepository.GetAll()
                            .GroupBy(a => a.ProjectId)
                            .Select(a => new { Hours = a.Sum(b => b.Hours), ProjectId = a.Key })
                            .OrderByDescending(a => a.Hours)
                            .ToList();

                var cc = (from p in _projectRepository.GetAll()
                          join Pro in _productionRepository.GetAll() on p.Id equals Pro.Projectid into productiondata
                          join Invreq in _invoiceRequestRepository.GetAll() on p.Id equals Invreq.ProjectId into projrepo
                          join c in _clientRepository.GetAll()
                          on p.ClientId equals c.Id

                          //join t in _timesheetRepository.GetAll()
                          //on p.Id equals t.ProjectId
                          join ud in _userRepository.GetAll()
                          on (Int32)p.Marketing_LeaderId equals ud.Id into userd
                          from ud in userd.DefaultIfEmpty()
                          join s in _projectStatusRepository.GetAll()
                          on p.Status equals s.Id
                          where p.Status == 2
                          select new ProjectDto
                          {
                              Id = p.Id,
                              ProjectName = p.ProjectName,
                              Description = p.Description,
                              StartDate = p.StartDate,
                              EndDate = p.EndDate,
                              Price = p.Price,
                              ClientName = c.ClientName,
                              Marketing_LeaderId = p.Marketing_LeaderId.HasValue ? p.Marketing_LeaderId.Value : 0,
                              MarketingLeadName = ud.Name + " " + ud.Surname,
                              ClientId = p.ClientId,
                              ProjectStatusId = p.Status,
                              ProjectStatus = s.Status,
                              totalhours = p.totalhours,
                              actualhours = 0,
                              username = "",
                              Priority = p.Priority,
                              Invoiceamount = projrepo.Where(x => x.Status == 1).Sum(x => x.Amount),
                              pricesum = projrepo.Where(p => p.Status == 0 || p.Status == 1).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(p => p.Status == 10).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(t => t.ProjectId == p.Id && p.Status == 10).Select(t => t.Amount).Sum(),
                              PendingAmount = ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)) == null ? p.Price : ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)),
                              enableStatus = p.IsEnable
                          })
                .WhereIf(!input.ProjectName.IsNullOrEmpty(), s => s.ProjectName.ToLower().Contains(input.ProjectName.ToLower()) /*|| s.ClientName.ToLower().Contains(input.ProjectName.ToLower()) || s.MarketingLeadName.ToLower().Contains(input.ProjectName.ToLower())*/)
                .WhereIf(input.ClientId.HasValue, s => s.ClientId == input.ClientId.Value)
               .WhereIf(input.ProjectId.HasValue, s => s.Id == input.ProjectId.Value)
               .WhereIf(input.MarketingleadId.HasValue, s => s.Marketing_LeaderId == input.MarketingleadId.Value)
                .WhereIf(input.ProjectStatusId.Length > 0, s => input.ProjectStatusId.Contains(s.ProjectStatusId));
                //var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
                var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
                foreach (var item in ccData)
                {
                    item.objProjectStatusList = GetStatusList();
                    item.actualhours = actualhours.Where(x => x.ProjectId == item.Id).Select(x => x.Hours).FirstOrDefault();
                    item.username = username;

                    //foreach (var datahours in actualhours)
                    //{
                    //    if (datahours.ProjectId==item.Id)
                    //    {
                    //        item.hours = datahours.Hours.ToString();
                    //    }
                    //    else
                    //    {
                    //        item.hours = "0";
                    //    }
                    //}
                }
                var ccCount = cc.Count();

                return new PagedResultDto<ProjectDto>(ccCount, ccData.MapTo<List<ProjectDto>>());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public PagedResultDto<ProjectDto> GetInvoiceCollectionProjectList(GetProjectDto input)
        {
            try
            {
                var User_List = _userRepository.GetAll();
                // var uid = (int)User_List.Where(u => u.Id == _abpSession.UserId).Select(t => t.Id).FirstOrDefault();
                var username = (from user in User_List
                                join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                                from ur in urJoined.DefaultIfEmpty()
                                join us in _roleRepository.GetAll() on ur.RoleId equals us.Id into usJoined
                                from us in usJoined.DefaultIfEmpty()
                                where (us != null && user.Id == _abpSession.UserId)
                                select (us.DisplayName)).FirstOrDefault();


                // var projrepo = _invoiceRequestRepository.GetAll();
                var actualhours = _timesheetRepository.GetAll()
                            .GroupBy(a => a.ProjectId)
                            .Select(a => new { Hours = a.Sum(b => b.Hours), ProjectId = a.Key })
                            .OrderByDescending(a => a.Hours)
                            .ToList();

                var cc = (from p in _projectRepository.GetAll()
                          join Pro in _productionRepository.GetAll() on p.Id equals Pro.Projectid into productiondata
                          join Invreq in _invoiceRequestRepository.GetAll() on p.Id equals Invreq.ProjectId into projrepo
                          join c in _clientRepository.GetAll()
                          on p.ClientId equals c.Id

                          //join t in _timesheetRepository.GetAll()
                          //on p.Id equals t.ProjectId
                          join ud in _userRepository.GetAll()
                          on (Int32)p.Marketing_LeaderId equals ud.Id into userd
                          from ud in userd.DefaultIfEmpty()
                          join s in _projectStatusRepository.GetAll()
                          on p.Status equals s.Id
                          where p.Status == 1
                          select new ProjectDto
                          {
                              Id = p.Id,
                              ProjectName = p.ProjectName,
                              Description = p.Description,
                              StartDate = p.StartDate,
                              EndDate = p.EndDate,
                              Price = p.Price,
                              ClientName = c.ClientName,
                              Marketing_LeaderId = p.Marketing_LeaderId.HasValue ? p.Marketing_LeaderId.Value : 0,
                              MarketingLeadName = ud.Name + " " + ud.Surname,
                              ClientId = p.ClientId,
                              ProjectStatusId = p.Status,
                              ProjectStatus = s.Status,
                              totalhours = p.totalhours,
                              actualhours = 0,
                              username = "",
                              Priority = p.Priority,
                              Invoiceamount = projrepo.Where(x => x.Status == 1).Sum(x => x.Amount),
                              pricesum = projrepo.Where(p => p.Status == 0 || p.Status == 1).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(p => p.Status == 10).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(t => t.ProjectId == p.Id && p.Status == 10).Select(t => t.Amount).Sum(),
                              PendingAmount = ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)) == null ? p.Price : ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)),
                              enableStatus = p.IsEnable
                          })
                .WhereIf(!input.ProjectName.IsNullOrEmpty(), s => s.ProjectName.ToLower().Contains(input.ProjectName.ToLower()) /*|| s.ClientName.ToLower().Contains(input.ProjectName.ToLower()) || s.MarketingLeadName.ToLower().Contains(input.ProjectName.ToLower())*/)
                .WhereIf(input.ClientId.HasValue, s => s.ClientId == input.ClientId.Value)
               .WhereIf(input.ProjectId.HasValue, s => s.Id == input.ProjectId.Value)
               .WhereIf(input.MarketingleadId.HasValue, s => s.Marketing_LeaderId == input.MarketingleadId.Value)
                .WhereIf(input.ProjectStatusId.Length > 0, s => input.ProjectStatusId.Contains(s.ProjectStatusId));
                //var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
                var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
                foreach (var item in ccData)
                {
                    item.objProjectStatusList = GetStatusList();
                    item.actualhours = actualhours.Where(x => x.ProjectId == item.Id).Select(x => x.Hours).FirstOrDefault();
                    item.hourPercentage = item.totalhours > 0 ? ((decimal)actualhours.Where(s => s.ProjectId == item.Id).Select(s => s.Hours).DefaultIfEmpty(0).Sum()) * 100 / item.totalhours : 0;
                    item.username = username;
                }
                var ccCount = cc.Count();

                return new PagedResultDto<ProjectDto>(ccCount, ccData.MapTo<List<ProjectDto>>());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public PagedResultDto<ProjectDto> GetOnGoingProjectList(GetProjectDto input)
        {
            try
            {
                var User_List = _userRepository.GetAll();
                // var uid = (int)User_List.Where(u => u.Id == _abpSession.UserId).Select(t => t.Id).FirstOrDefault();
                var username = (from user in User_List
                                join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                                from ur in urJoined.DefaultIfEmpty()
                                join us in _roleRepository.GetAll() on ur.RoleId equals us.Id into usJoined
                                from us in usJoined.DefaultIfEmpty()
                                where (us != null && user.Id == _abpSession.UserId)
                                select (us.DisplayName)).FirstOrDefault();


                // var projrepo = _invoiceRequestRepository.GetAll();
                var actualhours = _timesheetRepository.GetAll()
                            .GroupBy(a => a.ProjectId)
                            .Select(a => new { Hours = a.Sum(b => b.Hours), ProjectId = a.Key })
                            .OrderByDescending(a => a.Hours)
                            .ToList();

                var cc = (from p in _projectRepository.GetAll()
                          join Pro in _productionRepository.GetAll() on p.Id equals Pro.Projectid into productiondata
                          join Invreq in _invoiceRequestRepository.GetAll() on p.Id equals Invreq.ProjectId into projrepo
                          join c in _clientRepository.GetAll()
                          on p.ClientId equals c.Id

                          //join t in _timesheetRepository.GetAll()
                          //on p.Id equals t.ProjectId
                          join ud in _userRepository.GetAll()
                          on (Int32)p.Marketing_LeaderId equals ud.Id into userd
                          from ud in userd.DefaultIfEmpty()
                          join s in _projectStatusRepository.GetAll()
                          on p.Status equals s.Id
                          where p.Status == 8
                          select new ProjectDto
                          {
                              Id = p.Id,
                              ProjectName = p.ProjectName,
                              Description = p.Description,
                              StartDate = p.StartDate,
                              EndDate = p.EndDate,
                              Price = p.Price,
                              ClientName = c.ClientName,
                              Marketing_LeaderId = p.Marketing_LeaderId.HasValue ? p.Marketing_LeaderId.Value : 0,
                              MarketingLeadName = ud.Name + " " + ud.Surname,
                              ClientId = p.ClientId,
                              ProjectStatusId = p.Status,
                              ProjectStatus = s.Status,
                              totalhours = p.totalhours,
                              actualhours = 0,
                              username = "",
                              Priority = p.Priority,
                              Invoiceamount = projrepo.Where(x => x.Status == 1).Sum(x => x.Amount),
                              pricesum = projrepo.Where(p => p.Status == 0 || p.Status == 1).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(p => p.Status == 10).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(t => t.ProjectId == p.Id && p.Status == 10).Select(t => t.Amount).Sum(),
                              PendingAmount = ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)) == null ? p.Price : ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)),
                              enableStatus = p.IsEnable
                          })
                .WhereIf(!input.ProjectName.IsNullOrEmpty(), s => s.ProjectName.ToLower().Contains(input.ProjectName.ToLower()) /*|| s.ClientName.ToLower().Contains(input.ProjectName.ToLower()) || s.MarketingLeadName.ToLower().Contains(input.ProjectName.ToLower())*/)
                .WhereIf(input.ClientId.HasValue, s => s.ClientId == input.ClientId.Value)
               .WhereIf(input.ProjectId.HasValue, s => s.Id == input.ProjectId.Value)
               .WhereIf(input.MarketingleadId.HasValue, s => s.Marketing_LeaderId == input.MarketingleadId.Value)
                .WhereIf(input.ProjectStatusId.Length > 0, s => input.ProjectStatusId.Contains(s.ProjectStatusId));
                //var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
                var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
                foreach (var item in ccData)
                {
                    item.objProjectStatusList = GetStatusList();
                    item.actualhours = actualhours.Where(x => x.ProjectId == item.Id).Select(x => x.Hours).FirstOrDefault();
                    item.username = username;

                    //foreach (var datahours in actualhours)
                    //{
                    //    if (datahours.ProjectId==item.Id)
                    //    {
                    //        item.hours = datahours.Hours.ToString();
                    //    }
                    //    else
                    //    {
                    //        item.hours = "0";
                    //    }
                    //}
                }
                var ccCount = cc.Count();

                return new PagedResultDto<ProjectDto>(ccCount, ccData.MapTo<List<ProjectDto>>());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public List<ProjectDto> ExportGetHoldProjectList(ImportGetProjectDto input)
        {
            try
            {
                var User_List = _userRepository.GetAll();
                // var uid = (int)User_List.Where(u => u.Id == _abpSession.UserId).Select(t => t.Id).FirstOrDefault();
                var username = (from user in User_List
                                join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                                from ur in urJoined.DefaultIfEmpty()
                                join us in _roleRepository.GetAll() on ur.RoleId equals us.Id into usJoined
                                from us in usJoined.DefaultIfEmpty()
                                where (us != null && user.Id == _abpSession.UserId)
                                select (us.DisplayName)).FirstOrDefault();


                // var projrepo = _invoiceRequestRepository.GetAll();
                var actualhours = _timesheetRepository.GetAll()
                            .GroupBy(a => a.ProjectId)
                            .Select(a => new { Hours = a.Sum(b => b.Hours), ProjectId = a.Key })
                            .OrderByDescending(a => a.Hours)
                            .ToList();

                var cc = (from p in _projectRepository.GetAll()
                          join Pro in _productionRepository.GetAll() on p.Id equals Pro.Projectid into productiondata
                          join Invreq in _invoiceRequestRepository.GetAll() on p.Id equals Invreq.ProjectId into projrepo
                          join c in _clientRepository.GetAll()
                          on p.ClientId equals c.Id

                          //join t in _timesheetRepository.GetAll()
                          //on p.Id equals t.ProjectId
                          join ud in _userRepository.GetAll()
                          on (Int32)p.Marketing_LeaderId equals ud.Id into userd
                          from ud in userd.DefaultIfEmpty()
                          join s in _projectStatusRepository.GetAll()
                          on p.Status equals s.Id
                          where p.Status == 3
                          select new ProjectDto
                          {
                              Id = p.Id,
                              ProjectName = p.ProjectName,
                              Description = p.Description,
                              StartDate = p.StartDate,
                              EndDate = p.EndDate,
                              Price = p.Price,
                              ClientName = c.ClientName,
                              Marketing_LeaderId = p.Marketing_LeaderId.HasValue ? p.Marketing_LeaderId.Value : 0,
                              MarketingLeadName = ud.Name + " " + ud.Surname,
                              ClientId = p.ClientId,
                              ProjectStatusId = p.Status,
                              ProjectStatus = s.Status,
                              totalhours = p.totalhours,
                              actualhours = 0,
                              username = "",
                              Priority = p.Priority,
                              Invoiceamount = projrepo.Where(x => x.Status == 1).Sum(x => x.Amount),
                              pricesum = projrepo.Where(p => p.Status == 0 || p.Status == 1).Select(x => x.Amount).Sum(),
                              PendingAmount = ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)) == null ? p.Price : ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)),
                              enableStatus = p.IsEnable
                          })
                .WhereIf(!input.SearchBy.IsNullOrEmpty(), s => s.ProjectName.ToLower().Contains(input.SearchBy.ToLower()))
                .WhereIf(input.ClientId.HasValue, s => s.ClientId == input.ClientId.Value)
               .WhereIf(input.MarketingleadId.HasValue, s => s.Marketing_LeaderId == input.MarketingleadId.Value);
                var ccData = cc.OrderByDescending(x => x.Id).ToList();
                foreach (var item in ccData)
                {
                    item.objProjectStatusList = GetStatusList();
                    item.actualhours = actualhours.Where(x => x.ProjectId == item.Id).Select(x => x.Hours).FirstOrDefault();
                    item.username = username;

                }

                return ccData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public PagedResultDto<ProjectDto> GetOnHoldProjectList(GetProjectDto input)
        {
            try
            {
                var User_List = _userRepository.GetAll();
                // var uid = (int)User_List.Where(u => u.Id == _abpSession.UserId).Select(t => t.Id).FirstOrDefault();
                var username = (from user in User_List
                                join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                                from ur in urJoined.DefaultIfEmpty()
                                join us in _roleRepository.GetAll() on ur.RoleId equals us.Id into usJoined
                                from us in usJoined.DefaultIfEmpty()
                                where (us != null && user.Id == _abpSession.UserId)
                                select (us.DisplayName)).FirstOrDefault();


                // var projrepo = _invoiceRequestRepository.GetAll();
                var actualhours = _timesheetRepository.GetAll()
                            .GroupBy(a => a.ProjectId)
                            .Select(a => new { Hours = a.Sum(b => b.Hours), ProjectId = a.Key })
                            .OrderByDescending(a => a.Hours)
                            .ToList();

                var cc = (from p in _projectRepository.GetAll()
                          join Pro in _productionRepository.GetAll() on p.Id equals Pro.Projectid into productiondata
                          join Invreq in _invoiceRequestRepository.GetAll() on p.Id equals Invreq.ProjectId into projrepo
                          join c in _clientRepository.GetAll()
                          on p.ClientId equals c.Id

                          //join t in _timesheetRepository.GetAll()
                          //on p.Id equals t.ProjectId
                          join ud in _userRepository.GetAll()
                          on (Int32)p.Marketing_LeaderId equals ud.Id into userd
                          from ud in userd.DefaultIfEmpty()
                          join s in _projectStatusRepository.GetAll()
                          on p.Status equals s.Id
                          where p.Status == 3
                          select new ProjectDto
                          {
                              Id = p.Id,
                              ProjectName = p.ProjectName,
                              Description = p.Description,
                              StartDate = p.StartDate,
                              EndDate = p.EndDate,
                              Price = p.Price,
                              ClientName = c.ClientName,
                              Marketing_LeaderId = p.Marketing_LeaderId.HasValue ? p.Marketing_LeaderId.Value : 0,
                              MarketingLeadName = ud.Name + " " + ud.Surname,
                              ClientId = p.ClientId,
                              ProjectStatusId = p.Status,
                              ProjectStatus = s.Status,
                              totalhours = p.totalhours,
                              actualhours = 0,
                              username = "",
                              Priority = p.Priority,
                              Invoiceamount = projrepo.Where(x => x.Status == 1).Sum(x => x.Amount),
                              pricesum = projrepo.Where(p => p.Status == 0 || p.Status == 1).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(p => p.Status == 10).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(t => t.ProjectId == p.Id && p.Status == 10).Select(t => t.Amount).Sum(),
                              PendingAmount = ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)) == null ? p.Price : ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)),
                              enableStatus = p.IsEnable
                          })
                .WhereIf(!input.ProjectName.IsNullOrEmpty(), s => s.ProjectName.ToLower().Contains(input.ProjectName.ToLower()) /*|| s.ClientName.ToLower().Contains(input.ProjectName.ToLower()) || s.MarketingLeadName.ToLower().Contains(input.ProjectName.ToLower())*/)
                .WhereIf(input.ClientId.HasValue, s => s.ClientId == input.ClientId.Value)
               .WhereIf(input.ProjectId.HasValue, s => s.Id == input.ProjectId.Value)
               .WhereIf(input.MarketingleadId.HasValue, s => s.Marketing_LeaderId == input.MarketingleadId.Value)
                .WhereIf(input.ProjectStatusId.Length > 0, s => input.ProjectStatusId.Contains(s.ProjectStatusId));
                //var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
                var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
                foreach (var item in ccData)
                {
                    item.objProjectStatusList = GetStatusList();
                    item.actualhours = actualhours.Where(x => x.ProjectId == item.Id).Select(x => x.Hours).FirstOrDefault();
                    item.username = username;

                    //foreach (var datahours in actualhours)
                    //{
                    //    if (datahours.ProjectId==item.Id)
                    //    {
                    //        item.hours = datahours.Hours.ToString();
                    //    }
                    //    else
                    //    {
                    //        item.hours = "0";
                    //    }
                    //}
                }
                var ccCount = cc.Count();

                return new PagedResultDto<ProjectDto>(ccCount, ccData.MapTo<List<ProjectDto>>());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public async Task<ListResultDto<GetStatusDto>> GetActiveProjectStatus()
        {
            var enumData = (from s in _projectStatusRepository.GetAll()
                            select new GetStatusDto
                            {
                                Id = s.Id,
                                Status = s.Status
                            }).Where(x => x.Id == 4 || x.Id == 5 || x.Id == 6 || x.Id == 7 || x.Id == 10).OrderBy(x => x.Status).ToList();

            //return new ListResultDto<GetStatusDto>(ObjectMapper.Map<List<GetStatusDto>>(enumData));
            return new ListResultDto<GetStatusDto>(enumData.MapTo<List<GetStatusDto>>());
        }

        public async Task<ListResultDto<GetStatusDto>> GetAmcProjectStatus()
        {
            var enumData = (from s in _projectStatusRepository.GetAll()
                            select new GetStatusDto
                            {
                                Id = s.Id,
                                Status = s.Status
                            }).Where(x => x.Id == 9).OrderBy(x => x.Status).ToList();

            //return new ListResultDto<GetStatusDto>(ObjectMapper.Map<List<GetStatusDto>>(enumData));
            return new ListResultDto<GetStatusDto>(enumData.MapTo<List<GetStatusDto>>());
        }

        public async Task<ListResultDto<GetStatusDto>> GetCompletedProjectStatus()
        {
            var enumData = (from s in _projectStatusRepository.GetAll()
                            select new GetStatusDto
                            {
                                Id = s.Id,
                                Status = s.Status
                            }).Where(x => x.Id == 2).OrderBy(x => x.Status).ToList();

            //return new ListResultDto<GetStatusDto>(ObjectMapper.Map<List<GetStatusDto>>(enumData));
            return new ListResultDto<GetStatusDto>(enumData.MapTo<List<GetStatusDto>>());
        }

        public async Task<ListResultDto<GetStatusDto>> GetInvoiceCollectionProjectStatus()
        {
            var enumData = (from s in _projectStatusRepository.GetAll()
                            select new GetStatusDto
                            {
                                Id = s.Id,
                                Status = s.Status
                            }).Where(x => x.Id == 1).OrderBy(x => x.Status).ToList();

            //return new ListResultDto<GetStatusDto>(ObjectMapper.Map<List<GetStatusDto>>(enumData));
            return new ListResultDto<GetStatusDto>(enumData.MapTo<List<GetStatusDto>>());
        }

        public async Task<ListResultDto<GetStatusDto>> GetOnGoingProjectStatus()
        {
            var enumData = (from s in _projectStatusRepository.GetAll()
                            select new GetStatusDto
                            {
                                Id = s.Id,
                                Status = s.Status
                            }).Where(x => x.Id == 8).OrderBy(x => x.Status).ToList();

            //return new ListResultDto<GetStatusDto>(ObjectMapper.Map<List<GetStatusDto>>(enumData));
            return new ListResultDto<GetStatusDto>(enumData.MapTo<List<GetStatusDto>>());
        }

        public async Task<ListResultDto<GetStatusDto>> GetOnHoldStatus()
        {
            var enumData = (from s in _projectStatusRepository.GetAll()
                            select new GetStatusDto
                            {
                                Id = s.Id,
                                Status = s.Status
                            }).Where(x => x.Id == 3).OrderBy(x => x.Status).ToList();

            //return new ListResultDto<GetStatusDto>(ObjectMapper.Map<List<GetStatusDto>>(enumData));
            return new ListResultDto<GetStatusDto>(enumData.MapTo<List<GetStatusDto>>());
        }
        public List<ProjectDto> ExportGetProjectsWithoutClientList(ImportGetProjectDto input)
        {
            bool flag = false;

            try
            {
                long curId = (int)_abpSession.UserId;
                var rolelist = (from u in _userRepository.GetAll()
                                join ur in _userRoleRepository.GetAll()
                                on u.Id equals ur.UserId
                                join r in _roleRepository.GetAll()
                                on ur.RoleId equals r.Id
                                where u.Id == curId
                                select r).ToList();
                if (rolelist.Any(x => x.Name.ToLower() == "admin" || x.Name.ToLower() == "marketing leader"))
                {
                    flag = true;
                }

                var User_List = _userRepository.GetAll();
                // var uid = (int)User_List.Where(u => u.Id == _abpSession.UserId).Select(t => t.Id).FirstOrDefault();
                var username = (from user in User_List
                                join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                                from ur in urJoined.DefaultIfEmpty()
                                join us in _roleRepository.GetAll() on ur.RoleId equals us.Id into usJoined
                                from us in usJoined.DefaultIfEmpty()
                                where (us != null && user.Id == _abpSession.UserId && us.Name == "Marketing Leader")
                                select (us.DisplayName)).FirstOrDefault();


                // var projrepo = _invoiceRequestRepository.GetAll();
                var actualhours = _timesheetRepository.GetAll()
                            .GroupBy(a => a.ProjectId)
                            .Select(a => new { Hours = a.Sum(b => b.Hours), ProjectId = a.Key })
                            .OrderByDescending(a => a.Hours)
                            .ToList();

                var cc = (from p in _projectRepository.GetAll()
                          join Pro in _productionRepository.GetAll() on p.Id equals Pro.Projectid into productiondata
                          join Invreq in _invoiceRequestRepository.GetAll() on p.Id equals Invreq.ProjectId into projrepo
                          join c in _clientRepository.GetAll()
                          on p.ClientId equals c.Id into clientrepo
                          from ud in clientrepo.DefaultIfEmpty()

                              //join t in _timesheetRepository.GetAll()
                              //on p.Id equals t.ProjectId
                          join ur in _userRepository.GetAll()
                          on (Int32)p.Marketing_LeaderId equals ur.Id into userd
                          from ur in userd.DefaultIfEmpty()
                          join s in _projectStatusRepository.GetAll()
                          on p.Status equals s.Id
                          where (p.Status == 4 || p.Status == 5 || p.Status == 6 || p.Status == 7 || p.Status == 10) && p.ClientId == 0
                          select new ProjectDto
                          {
                              Id = p.Id,
                              ProjectName = p.ProjectName,
                              Description = p.Description,
                              StartDate = p.StartDate,
                              EndDate = p.EndDate,
                              Price = p.Price,
                              ClientName = ud.ClientName,
                              Marketing_LeaderId = p.Marketing_LeaderId.HasValue ? p.Marketing_LeaderId.Value : 0,
                              MarketingLeadName = ur.Name + " " + ur.Surname,
                              ClientId = p.ClientId,
                              ProjectStatusId = p.Status,
                              ProjectStatus = s.Status,
                              totalhours = p.totalhours,
                              actualhours = 0,
                              username = username != null ? "Marketing Leader" : "",
                              Priority = p.Priority,
                              Invoiceamount = projrepo.Where(x => x.Status == 1).Sum(x => x.Amount),
                              pricesum = projrepo.Where(p => p.Status == 0 || p.Status == 1).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(p => p.Status == 10).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(t => t.ProjectId == p.Id && p.Status == 10).Select(t => t.Amount).Sum(),
                              PendingAmount = ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)) == null ? p.Price : ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)),
                              enableStatus = p.IsEnable,
                              typeshow = flag
                          })
                .WhereIf(!input.SearchBy.IsNullOrEmpty(), s => s.ProjectName.ToLower().Contains(input.SearchBy.ToLower()))
                .WhereIf(input.ClientId.HasValue, s => s.ClientId == input.ClientId.Value)
               .WhereIf(input.MarketingleadId.HasValue, s => s.Marketing_LeaderId == input.MarketingleadId.Value)
                .WhereIf(input.ProjectStatusId != null, s => input.ProjectStatusId.Contains(s.ProjectStatusId));

                var ccData = cc.OrderByDescending(x => x.Id).ToList();
                foreach (var item in ccData)
                {
                    item.objProjectStatusList = GetStatusList();
                    item.actualhours = actualhours.Where(x => x.ProjectId == item.Id).Select(x => x.Hours).FirstOrDefault();
                    item.hourPercentage = item.totalhours > 0 ? ((decimal)actualhours.Where(s => s.ProjectId == item.Id).Select(s => s.Hours).DefaultIfEmpty(0).Sum()) * 100 / item.totalhours : 0;
                    item.username = username;

                }

                return ccData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public PagedResultDto<ProjectDto> GetProjectsWithoutClientList(GetProjectDto input)
        {
            bool flag = false;

            try
            {
                long curId = (int)_abpSession.UserId;
                var rolelist = (from u in _userRepository.GetAll()
                                join ur in _userRoleRepository.GetAll()
                                on u.Id equals ur.UserId
                                join r in _roleRepository.GetAll()
                                on ur.RoleId equals r.Id
                                where u.Id == curId
                                select r).ToList();
                if (rolelist.Any(x => x.Name.ToLower() == "admin" || x.Name.ToLower() == "marketing leader"))
                {
                    flag = true;
                }

                var User_List = _userRepository.GetAll();
                // var uid = (int)User_List.Where(u => u.Id == _abpSession.UserId).Select(t => t.Id).FirstOrDefault();
                var username = (from user in User_List
                                join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                                from ur in urJoined.DefaultIfEmpty()
                                join us in _roleRepository.GetAll() on ur.RoleId equals us.Id into usJoined
                                from us in usJoined.DefaultIfEmpty()
                                where (us != null && user.Id == _abpSession.UserId && us.Name == "Marketing Leader")
                                select (us.DisplayName)).FirstOrDefault();


                // var projrepo = _invoiceRequestRepository.GetAll();
                var actualhours = _timesheetRepository.GetAll()
                            .GroupBy(a => a.ProjectId)
                            .Select(a => new { Hours = a.Sum(b => b.Hours), ProjectId = a.Key })
                            .OrderByDescending(a => a.Hours)
                            .ToList();

                var cc = (from p in _projectRepository.GetAll()
                          join Pro in _productionRepository.GetAll() on p.Id equals Pro.Projectid into productiondata
                          join Invreq in _invoiceRequestRepository.GetAll() on p.Id equals Invreq.ProjectId into projrepo
                          join c in _clientRepository.GetAll()
                          on p.ClientId equals c.Id into clientrepo
                          from ud in clientrepo.DefaultIfEmpty()

                              //join t in _timesheetRepository.GetAll()
                              //on p.Id equals t.ProjectId
                          join ur in _userRepository.GetAll()
                          on (Int32)p.Marketing_LeaderId equals ur.Id into userd
                          from ur in userd.DefaultIfEmpty()
                          join s in _projectStatusRepository.GetAll()
                          on p.Status equals s.Id
                          where (p.Status == 4 || p.Status == 5 || p.Status == 6 || p.Status == 7 || p.Status == 10) && p.ClientId == 0
                          select new ProjectDto
                          {
                              Id = p.Id,
                              ProjectName = p.ProjectName,
                              Description = p.Description,
                              StartDate = p.StartDate,
                              EndDate = p.EndDate,
                              Price = p.Price,
                              ClientName = ud.ClientName,
                              Marketing_LeaderId = p.Marketing_LeaderId.HasValue ? p.Marketing_LeaderId.Value : 0,
                              MarketingLeadName = ur.Name + " " + ur.Surname,
                              ClientId = p.ClientId,
                              ProjectStatusId = p.Status,
                              ProjectStatus = s.Status,
                              totalhours = p.totalhours,
                              actualhours = 0,
                              username = username != null ? "Marketing Leader" : "",
                              Priority = p.Priority,
                              Invoiceamount = projrepo.Where(x => x.Status == 1).Sum(x => x.Amount),
                              pricesum = projrepo.Where(p => p.Status == 0 || p.Status == 1).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(p => p.Status == 10).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(t => t.ProjectId == p.Id && p.Status == 10).Select(t => t.Amount).Sum(),
                              PendingAmount = ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)) == null ? p.Price : ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)),
                              enableStatus = p.IsEnable,
                              typeshow = flag
                          })
                .WhereIf(!input.ProjectName.IsNullOrEmpty(), s => s.ProjectName.ToLower().Contains(input.ProjectName.ToLower()) /*|| s.ClientName.ToLower().Contains(input.ProjectName.ToLower()) || s.MarketingLeadName.ToLower().Contains(input.ProjectName.ToLower())*/)
                .WhereIf(input.ClientId.HasValue, s => s.ClientId == input.ClientId.Value)
               .WhereIf(input.ProjectId.HasValue, s => s.Id == input.ProjectId.Value)
               .WhereIf(input.MarketingleadId.HasValue, s => s.Marketing_LeaderId == input.MarketingleadId.Value)
                .WhereIf(input.ProjectStatusId.Length > 0, s => input.ProjectStatusId.Contains(s.ProjectStatusId));

                var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
                foreach (var item in ccData)
                {
                    item.objProjectStatusList = GetStatusList();
                    item.actualhours = actualhours.Where(x => x.ProjectId == item.Id).Select(x => x.Hours).FirstOrDefault();
                    item.hourPercentage = item.totalhours > 0 ? ((decimal)actualhours.Where(s => s.ProjectId == item.Id).Select(s => s.Hours).DefaultIfEmpty(0).Sum()) * 100 / item.totalhours : 0;
                    item.username = username;

                }
                var ccCount = cc.Count();

                return new PagedResultDto<ProjectDto>(ccCount, ccData.MapTo<List<ProjectDto>>());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public async Task DeleteInvoiceRequest(EntityDto input)
        {
            await _invoiceRequestRepository.DeleteAsync(input.Id);
        }

        public List<ProjectDto> GetAllProjectListData(ImportGetProjectDto input)
        {
            bool flag = false;

            try
            {
                long curId = (int)_abpSession.UserId;
                var rolelist = (from u in _userRepository.GetAll()
                                join ur in _userRoleRepository.GetAll()
                                on u.Id equals ur.UserId
                                join r in _roleRepository.GetAll()
                                on ur.RoleId equals r.Id
                                where u.Id == curId
                                select r).ToList();
                if (rolelist.Any(x => x.Name.ToLower() == "admin" || x.Name.ToLower() == "marketing leader"))
                {
                    flag = true;
                }

                var User_List = _userRepository.GetAll();
                // var uid = (int)User_List.Where(u => u.Id == _abpSession.UserId).Select(t => t.Id).FirstOrDefault();
                var username = (from user in User_List
                                join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                                from ur in urJoined.DefaultIfEmpty()
                                join us in _roleRepository.GetAll() on ur.RoleId equals us.Id into usJoined
                                from us in usJoined.DefaultIfEmpty()
                                where (us != null && user.Id == _abpSession.UserId && us.Name == "Marketing Leader")
                                select (us.DisplayName)).FirstOrDefault();



                var actualhours = _timesheetRepository.GetAll()
                            .GroupBy(a => a.ProjectId)
                            .Select(a => new { Hours = a.Sum(b => b.Hours), ProjectId = a.Key })
                            .OrderByDescending(a => a.Hours)
                            .ToList();

                var cc = (from p in _projectRepository.GetAll()
                          join Pro in _productionRepository.GetAll() on p.Id equals Pro.Projectid into productiondata
                          join Invreq in _invoiceRequestRepository.GetAll() on p.Id equals Invreq.ProjectId into projrepo
                          join c in _clientRepository.GetAll()
                          on p.ClientId equals c.Id into clientrepo
                          from ud in clientrepo.DefaultIfEmpty()

                          join ur in _userRepository.GetAll()
                          on (Int32)p.Marketing_LeaderId equals ur.Id into userd
                          from ur in userd.DefaultIfEmpty()
                          join s in _projectStatusRepository.GetAll()
                          on p.Status equals s.Id
                          select new ProjectDto
                          {
                              Id = p.Id,
                              ProjectName = p.ProjectName,
                              Description = p.Description,
                              StartDate = p.StartDate,
                              EndDate = p.EndDate,
                              Price = p.Price,
                              ClientName = ud.ClientName,
                              Marketing_LeaderId = p.Marketing_LeaderId.HasValue ? p.Marketing_LeaderId.Value : 0,
                              MarketingLeadName = ur.Name + " " + ur.Surname,
                              ClientId = p.ClientId,
                              ProjectStatusId = p.Status,
                              ProjectStatus = s.Status,
                              totalhours = p.totalhours,
                              actualhours = 0,
                              username = username != null ? "Marketing Leader" : "",
                              Priority = p.Priority,
                              Invoiceamount = projrepo.Where(x => x.Status == 1).Sum(x => x.Amount),
                              pricesum = projrepo.Where(p => p.Status == 0 || p.Status == 1).Select(x => x.Amount).Sum(),

                              PendingAmount = ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)) == null ? p.Price : ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)),
                              enableStatus = p.IsEnable,
                              typeshow = flag
                          })
                .WhereIf(!input.SearchBy.IsNullOrEmpty(), s => s.ProjectName.ToLower().Contains(input.SearchBy.ToLower()))
                .WhereIf(input.ClientId.HasValue, s => s.ClientId == input.ClientId.Value)
               .WhereIf(input.MarketingleadId.HasValue, s => s.Marketing_LeaderId == input.MarketingleadId.Value)
                .WhereIf(input.ProjectStatusId != null, s => input.ProjectStatusId.Contains(s.ProjectStatusId));
                var ccData = cc.OrderByDescending(x => x.Id).ToList();
                foreach (var item in ccData)
                {
                    item.objProjectStatusList = GetStatusList();
                    item.actualhours = actualhours.Where(x => x.ProjectId == item.Id).Select(x => x.Hours).FirstOrDefault();
                    item.hourPercentage = item.totalhours > 0 ? ((decimal)actualhours.Where(s => s.ProjectId == item.Id).Select(s => s.Hours).DefaultIfEmpty(0).Sum()) * 100 / item.totalhours : 0;
                    item.username = username;

                }
                return ccData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<ProjectDto> GetActiveProjectListData(ImportGetProjectDto input)
        {
            bool flag = false;

            try
            {
                long curId = (int)_abpSession.UserId;
                var rolelist = (from u in _userRepository.GetAll()
                                join ur in _userRoleRepository.GetAll()
                                on u.Id equals ur.UserId
                                join r in _roleRepository.GetAll()
                                on ur.RoleId equals r.Id
                                where u.Id == curId
                                select r).ToList();
                if (rolelist.Any(x => x.Name.ToLower() == "admin" || x.Name.ToLower() == "marketing leader"))
                {
                    flag = true;
                }

                var User_List = _userRepository.GetAll();
                // var uid = (int)User_List.Where(u => u.Id == _abpSession.UserId).Select(t => t.Id).FirstOrDefault();
                var username = (from user in User_List
                                join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                                from ur in urJoined.DefaultIfEmpty()
                                join us in _roleRepository.GetAll() on ur.RoleId equals us.Id into usJoined
                                from us in usJoined.DefaultIfEmpty()
                                where (us != null && user.Id == _abpSession.UserId && us.Name == "Marketing Leader")
                                select (us.DisplayName)).FirstOrDefault();



                var actualhours = _timesheetRepository.GetAll()
                            .GroupBy(a => a.ProjectId)
                            .Select(a => new { Hours = a.Sum(b => b.Hours), ProjectId = a.Key })
                            .OrderByDescending(a => a.Hours)
                            .ToList();

                var cc = (from p in _projectRepository.GetAll()
                          join Pro in _productionRepository.GetAll() on p.Id equals Pro.Projectid into productiondata
                          join Invreq in _invoiceRequestRepository.GetAll() on p.Id equals Invreq.ProjectId into projrepo
                          join c in _clientRepository.GetAll()
                          on p.ClientId equals c.Id into clientrepo
                          from ud in clientrepo.DefaultIfEmpty()

                          join ur in _userRepository.GetAll()
                          on (Int32)p.Marketing_LeaderId equals ur.Id into userd
                          from ur in userd.DefaultIfEmpty()
                          join s in _projectStatusRepository.GetAll()
                          on p.Status equals s.Id
                          where p.Status == 4 || p.Status == 5 || p.Status == 6 || p.Status == 7 || p.Status == 10
                          select new ProjectDto
                          {
                              Id = p.Id,
                              ProjectName = p.ProjectName,
                              Description = p.Description,
                              StartDate = p.StartDate,
                              EndDate = p.EndDate,
                              Price = p.Price,
                              ClientName = ud.ClientName,
                              Marketing_LeaderId = p.Marketing_LeaderId.HasValue ? p.Marketing_LeaderId.Value : 0,
                              MarketingLeadName = ur.Name + " " + ur.Surname,
                              ClientId = p.ClientId,
                              ProjectStatusId = p.Status,
                              ProjectStatus = s.Status,
                              totalhours = p.totalhours,
                              actualhours = 0,
                              username = username != null ? "Marketing Leader" : "",
                              Priority = p.Priority,
                              Invoiceamount = projrepo.Where(x => x.Status == 1).Sum(x => x.Amount),
                              pricesum = projrepo.Where(p => p.Status == 0 || p.Status == 1).Select(x => x.Amount).Sum(),

                              PendingAmount = ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)) == null ? p.Price : ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)),
                              enableStatus = p.IsEnable,
                              typeshow = flag
                          })
                .WhereIf(!input.SearchBy.IsNullOrEmpty(), s => s.ProjectName.ToLower().Contains(input.SearchBy.ToLower()))
                .WhereIf(input.ClientId.HasValue, s => s.ClientId == input.ClientId.Value)
               .WhereIf(input.MarketingleadId.HasValue, s => s.Marketing_LeaderId == input.MarketingleadId.Value)
                .WhereIf(input.ProjectStatusId != null, s => input.ProjectStatusId.Contains(s.ProjectStatusId));
                var ccData = cc.OrderByDescending(x => x.Id).ToList();
                foreach (var item in ccData)
                {
                    item.objProjectStatusList = GetStatusList();
                    item.actualhours = actualhours.Where(x => x.ProjectId == item.Id).Select(x => x.Hours).FirstOrDefault();
                    item.hourPercentage = item.totalhours > 0 ? ((decimal)actualhours.Where(s => s.ProjectId == item.Id).Select(s => s.Hours).DefaultIfEmpty(0).Sum()) * 100 / item.totalhours : 0;
                    item.username = username;

                }
                return ccData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<ProjectDto> GetAmcProjectListAMC(ImportGetProjectDto input)
        {
            try
            {
                var User_List = _userRepository.GetAll();
                // var uid = (int)User_List.Where(u => u.Id == _abpSession.UserId).Select(t => t.Id).FirstOrDefault();
                var username = (from user in User_List
                                join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                                from ur in urJoined.DefaultIfEmpty()
                                join us in _roleRepository.GetAll() on ur.RoleId equals us.Id into usJoined
                                from us in usJoined.DefaultIfEmpty()
                                where (us != null && user.Id == _abpSession.UserId)
                                select (us.DisplayName)).FirstOrDefault();


                // var projrepo = _invoiceRequestRepository.GetAll();
                var actualhours = _timesheetRepository.GetAll()
                            .GroupBy(a => a.ProjectId)
                            .Select(a => new { Hours = a.Sum(b => b.Hours), ProjectId = a.Key })
                            .OrderByDescending(a => a.Hours)
                            .ToList();

                var cc = (from p in _projectRepository.GetAll()
                          join Pro in _productionRepository.GetAll() on p.Id equals Pro.Projectid into productiondata
                          join Invreq in _invoiceRequestRepository.GetAll() on p.Id equals Invreq.ProjectId into projrepo
                          join c in _clientRepository.GetAll()
                          on p.ClientId equals c.Id

                          //join t in _timesheetRepository.GetAll()
                          //on p.Id equals t.ProjectId
                          join ud in _userRepository.GetAll()
                          on (Int32)p.Marketing_LeaderId equals ud.Id into userd
                          from ud in userd.DefaultIfEmpty()
                          join s in _projectStatusRepository.GetAll()
                          on p.Status equals s.Id
                          where p.Status == 9
                          select new ProjectDto
                          {
                              Id = p.Id,
                              ProjectName = p.ProjectName,
                              Description = p.Description,
                              StartDate = p.StartDate,
                              EndDate = p.EndDate,
                              Price = p.Price,
                              ClientName = c.ClientName,
                              Marketing_LeaderId = p.Marketing_LeaderId.HasValue ? p.Marketing_LeaderId.Value : 0,
                              MarketingLeadName = ud.Name + " " + ud.Surname,
                              ClientId = p.ClientId,
                              ProjectStatusId = p.Status,
                              ProjectStatus = s.Status,
                              totalhours = p.totalhours,
                              actualhours = 0,
                              username = "",
                              Priority = p.Priority,
                              Invoiceamount = projrepo.Where(x => x.Status == 1).Sum(x => x.Amount),
                              pricesum = projrepo.Where(p => p.Status == 0 || p.Status == 1).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(p => p.Status == 10).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(t => t.ProjectId == p.Id && p.Status == 10).Select(t => t.Amount).Sum(),
                              PendingAmount = ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)) == null ? p.Price : ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)),
                              enableStatus = p.IsEnable
                          })
                .WhereIf(!input.SearchBy.IsNullOrEmpty(), s => s.ProjectName.ToLower().Contains(input.SearchBy.ToLower()))
                .WhereIf(input.ClientId.HasValue, s => s.ClientId == input.ClientId.Value)
               .WhereIf(input.MarketingleadId.HasValue, s => s.Marketing_LeaderId == input.MarketingleadId.Value);

                var ccData = cc.OrderBy(x => x.Id).ToList();
                foreach (var item in ccData)
                {
                    item.objProjectStatusList = GetStatusList();
                    item.actualhours = actualhours.Where(x => x.ProjectId == item.Id).Select(x => x.Hours).FirstOrDefault();
                    item.username = username;


                }
                var ccCount = cc.Count();
                return ccData;


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<ProjectDto> GetOnGoingProjectListOnGoing(ImportGetProjectDto input)
        {
            try
            {
                var User_List = _userRepository.GetAll();
                // var uid = (int)User_List.Where(u => u.Id == _abpSession.UserId).Select(t => t.Id).FirstOrDefault();
                var username = (from user in User_List
                                join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                                from ur in urJoined.DefaultIfEmpty()
                                join us in _roleRepository.GetAll() on ur.RoleId equals us.Id into usJoined
                                from us in usJoined.DefaultIfEmpty()
                                where (us != null && user.Id == _abpSession.UserId)
                                select (us.DisplayName)).FirstOrDefault();


                // var projrepo = _invoiceRequestRepository.GetAll();
                var actualhours = _timesheetRepository.GetAll()
                            .GroupBy(a => a.ProjectId)
                            .Select(a => new { Hours = a.Sum(b => b.Hours), ProjectId = a.Key })
                            .OrderByDescending(a => a.Hours)
                            .ToList();

                var cc = (from p in _projectRepository.GetAll()
                          join Pro in _productionRepository.GetAll() on p.Id equals Pro.Projectid into productiondata
                          join Invreq in _invoiceRequestRepository.GetAll() on p.Id equals Invreq.ProjectId into projrepo
                          join c in _clientRepository.GetAll()
                          on p.ClientId equals c.Id

                          //join t in _timesheetRepository.GetAll()
                          //on p.Id equals t.ProjectId
                          join ud in _userRepository.GetAll()
                          on (Int32)p.Marketing_LeaderId equals ud.Id into userd
                          from ud in userd.DefaultIfEmpty()
                          join s in _projectStatusRepository.GetAll()
                          on p.Status equals s.Id
                          where p.Status == 8
                          select new ProjectDto
                          {
                              Id = p.Id,
                              ProjectName = p.ProjectName,
                              Description = p.Description,
                              StartDate = p.StartDate,
                              EndDate = p.EndDate,
                              Price = p.Price,
                              ClientName = c.ClientName,
                              Marketing_LeaderId = p.Marketing_LeaderId.HasValue ? p.Marketing_LeaderId.Value : 0,
                              MarketingLeadName = ud.Name + " " + ud.Surname,
                              ClientId = p.ClientId,
                              ProjectStatusId = p.Status,
                              ProjectStatus = s.Status,
                              totalhours = p.totalhours,
                              actualhours = 0,
                              username = "",
                              Priority = p.Priority,
                              Invoiceamount = projrepo.Where(x => x.Status == 1).Sum(x => x.Amount),
                              pricesum = projrepo.Where(p => p.Status == 0 || p.Status == 1).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(p => p.Status == 10).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(t => t.ProjectId == p.Id && p.Status == 10).Select(t => t.Amount).Sum(),
                              PendingAmount = ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)) == null ? p.Price : ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)),
                              enableStatus = p.IsEnable
                          })
                .WhereIf(!input.SearchBy.IsNullOrEmpty(), s => s.ProjectName.ToLower().Contains(input.SearchBy.ToLower()))
                .WhereIf(input.ClientId.HasValue, s => s.ClientId == input.ClientId.Value)
               .WhereIf(input.MarketingleadId.HasValue, s => s.Marketing_LeaderId == input.MarketingleadId.Value);

                var ccData = cc.OrderBy(x => x.Id).ToList();
                foreach (var item in ccData)
                {
                    item.objProjectStatusList = GetStatusList();
                    item.actualhours = actualhours.Where(x => x.ProjectId == item.Id).Select(x => x.Hours).FirstOrDefault();
                    item.username = username;

                    //foreach (var datahours in actualhours)
                    //{
                    //    if (datahours.ProjectId==item.Id)
                    //    {
                    //        item.hours = datahours.Hours.ToString();
                    //    }
                    //    else
                    //    {
                    //        item.hours = "0";
                    //    }
                    //}
                }
                var ccCount = cc.Count();

                return ccData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<ProjectDto> GetInvoiceCollectionProjectListINVOICE(ImportGetProjectDto input)
        {
            try
            {
                var User_List = _userRepository.GetAll();
                // var uid = (int)User_List.Where(u => u.Id == _abpSession.UserId).Select(t => t.Id).FirstOrDefault();
                var username = (from user in User_List
                                join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                                from ur in urJoined.DefaultIfEmpty()
                                join us in _roleRepository.GetAll() on ur.RoleId equals us.Id into usJoined
                                from us in usJoined.DefaultIfEmpty()
                                where (us != null && user.Id == _abpSession.UserId)
                                select (us.DisplayName)).FirstOrDefault();


                // var projrepo = _invoiceRequestRepository.GetAll();
                var actualhours = _timesheetRepository.GetAll()
                            .GroupBy(a => a.ProjectId)
                            .Select(a => new { Hours = a.Sum(b => b.Hours), ProjectId = a.Key })
                            .OrderByDescending(a => a.Hours)
                            .ToList();

                var cc = (from p in _projectRepository.GetAll()
                          join Pro in _productionRepository.GetAll() on p.Id equals Pro.Projectid into productiondata
                          join Invreq in _invoiceRequestRepository.GetAll() on p.Id equals Invreq.ProjectId into projrepo
                          join c in _clientRepository.GetAll()
                          on p.ClientId equals c.Id

                          //join t in _timesheetRepository.GetAll()
                          //on p.Id equals t.ProjectId
                          join ud in _userRepository.GetAll()
                          on (Int32)p.Marketing_LeaderId equals ud.Id into userd
                          from ud in userd.DefaultIfEmpty()
                          join s in _projectStatusRepository.GetAll()
                          on p.Status equals s.Id
                          where p.Status == 1
                          select new ProjectDto
                          {
                              Id = p.Id,
                              ProjectName = p.ProjectName,
                              Description = p.Description,
                              StartDate = p.StartDate,
                              EndDate = p.EndDate,
                              Price = p.Price,
                              ClientName = c.ClientName,
                              Marketing_LeaderId = p.Marketing_LeaderId.HasValue ? p.Marketing_LeaderId.Value : 0,
                              MarketingLeadName = ud.Name + " " + ud.Surname,
                              ClientId = p.ClientId,
                              ProjectStatusId = p.Status,
                              ProjectStatus = s.Status,
                              totalhours = p.totalhours,
                              actualhours = 0,
                              username = "",
                              Priority = p.Priority,
                              Invoiceamount = projrepo.Where(x => x.Status == 1).Sum(x => x.Amount),
                              pricesum = projrepo.Where(p => p.Status == 0 || p.Status == 1).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(p => p.Status == 10).Select(x => x.Amount).Sum(),
                              //pricesum = projrepo.Where(t => t.ProjectId == p.Id && p.Status == 10).Select(t => t.Amount).Sum(),
                              PendingAmount = ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)) == null ? p.Price : ((p.Price) - projrepo.Where(x => x.Status == 1).Sum(x => x.Amount)),
                              enableStatus = p.IsEnable
                          })
                .WhereIf(!input.SearchBy.IsNullOrEmpty(), s => s.ProjectName.ToLower().Contains(input.SearchBy.ToLower()))
                .WhereIf(input.ClientId.HasValue, s => s.ClientId == input.ClientId.Value)
               .WhereIf(input.MarketingleadId.HasValue, s => s.Marketing_LeaderId == input.MarketingleadId.Value);
                // .WhereIf(!input.ProjectName.IsNullOrEmpty(), s => s.ProjectName.ToLower().Contains(input.ProjectName.ToLower()) /*|| s.ClientName.ToLower().Contains(input.ProjectName.ToLower()) || s.MarketingLeadName.ToLower().Contains(input.ProjectName.ToLower())*/)
                // .WhereIf(input.ClientId.HasValue, s => s.ClientId == input.ClientId.Value)
                //.WhereIf(input.ProjectId.HasValue, s => s.Id == input.ProjectId.Value)
                //.WhereIf(input.MarketingleadId.HasValue, s => s.Marketing_LeaderId == input.MarketingleadId.Value)
                // .WhereIf(input.ProjectStatusId.Length > 0, s => input.ProjectStatusId.Contains(s.ProjectStatusId));
                //var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
                var ccData = cc.OrderBy(c => c.Id).ToList();
                foreach (var item in ccData)
                {
                    item.objProjectStatusList = GetStatusList();
                    item.actualhours = actualhours.Where(x => x.ProjectId == item.Id).Select(x => x.Hours).FirstOrDefault();
                    item.hourPercentage = item.totalhours > 0 ? ((decimal)actualhours.Where(s => s.ProjectId == item.Id).Select(s => s.Hours).DefaultIfEmpty(0).Sum()) * 100 / item.totalhours : 0;
                    item.username = username;
                }
                var ccCount = cc.Count();

                return ccData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }

}

