using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using AbpProjects.Opportunities;
using AbpProjects.Project.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using System.Data;
using Abp.AutoMapper;
using AbpProjects.CallCategories;
using AbpProjects.OpportunityAppServices.Dto;
using AbpProjects.ProjectType;
using AbpProjects.Authorization.Users;
using AbpProjects.Authorization.Roles;
using Abp.Authorization.Users;
using AutoMapper;
using Abp.Extensions;
using Abp.Runtime.Session;
using AbpProjects.BulkData.Dto;
using AbpProjects.BulkData;
using AbpProjects.EntityFramework;
using AbpProjects.TimeSheet.Dto;
using AbpProjects.Dasboard.Dto;
using AbpProjects.LeaveApplication.Dto;
using AbpProjects.Company;
using Abp.Authorization;
using AbpProjects.VPS.Dto;
using Newtonsoft.Json;

namespace AbpProjects.OpportunityAppServices
{
    [AbpAuthorize]
    public class OpportunityService : AbpProjectsApplicationModule, IOpportunityService
    {
        private readonly IRepository<Opportunity> _OpportunityRepository;
        private readonly IRepository<InterestedOpportunity> _InterestedOpportunityRepository;
        private readonly IRepository<OpportunityFollowUp> _OpportunityFollowUpRepository;
        private readonly IRepository<FollowupIntrest> _FollowupIntrestRepository;
        private readonly IRepository<CallCategory> _CallCategoriesRepository;
        private readonly IRepository<projecttype> _projecttypeRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        private readonly IRepository<company> _companyRepository;
        private readonly IRepository<Followuptype> _FollowuptypeRepository;
        public OpportunityService
            (
            IRepository<Opportunity> opportunityRepository,
            IRepository<CallCategory> CallCategoriesRepository,
            IRepository<projecttype> projecttypeRepository,
            IRepository<User, long> userRepository,
            IRepository<Role> roleRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<InterestedOpportunity> InterestedOpportunityRepository,
            IAbpSession abpSession,
            UserManager userManager,
            IRepository<OpportunityFollowUp> OpportunityFollowUpRepository,
            IRepository<FollowupIntrest> FollowupIntrestRepository,
            IRepository<company> companyRepository, IRepository<Followuptype> FollowuptypeRepository
            )
        {
            _OpportunityRepository = opportunityRepository;
            _CallCategoriesRepository = CallCategoriesRepository;
            _projecttypeRepository = projecttypeRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _InterestedOpportunityRepository = InterestedOpportunityRepository;
            _abpSession = abpSession;
            _userManager = userManager;
            _OpportunityFollowUpRepository = OpportunityFollowUpRepository;
            _FollowupIntrestRepository = FollowupIntrestRepository;
            _companyRepository = companyRepository;
            _FollowuptypeRepository = FollowuptypeRepository;
        }

        public async Task<ListResultDto<CallCategoryDto>> GetCallCategoryMyOpp()
        {
            var enumData = (from s in _CallCategoriesRepository.GetAll()
                            select new CallCategoryDto
                            {
                                Id = s.Id,
                                Name = s.Name
                            }).Where(x => x.Id == 7 || x.Id == 8).OrderBy(x => x.Name).ToList();

            return new ListResultDto<CallCategoryDto>(enumData.MapTo<List<CallCategoryDto>>());
        }
        public async Task<ListResultDto<CallCategoryDto>> GetCallCategoryGen()
        {
            var enumData = (from s in _CallCategoriesRepository.GetAll()
                            select new CallCategoryDto
                            {
                                Id = s.Id,
                                Name = s.Name
                            }).Where(x => x.Id == 7).OrderBy(x => x.Name).ToList();

            return new ListResultDto<CallCategoryDto>(enumData.MapTo<List<CallCategoryDto>>());
        }

        public async Task<ListResultDto<CallCategoryDto>> GetCallCategoryOpportunity()
        {
            var enumData = (from s in _CallCategoriesRepository.GetAll()
                            select new CallCategoryDto
                            {
                                Id = s.Id,
                                Name = s.Name
                            }).Where(x => x.Id == 7 || x.Id == 8 || x.Id == 9 || x.Id == 10).OrderBy(x => x.Name).ToList();

            return new ListResultDto<CallCategoryDto>(enumData.MapTo<List<CallCategoryDto>>());
        }

        public async Task<ListResultDto<CallCategoryDto>> GetCallCategoryInq()
        {
            var enumData = (from s in _CallCategoriesRepository.GetAll()
                            select new CallCategoryDto
                            {
                                Id = s.Id,
                                Name = s.Name
                            }).Where(x => x.Id != 7 && x.Id != 8).OrderBy(x => x.Name).ToList();

            return new ListResultDto<CallCategoryDto>(enumData.MapTo<List<CallCategoryDto>>());
        }

        public async Task<ListResultDto<CallCategoryDto>> GetCallCategory()
        {
            var enumData = (from s in _CallCategoriesRepository.GetAll()
                            select new CallCategoryDto
                            {
                                Id = s.Id,
                                Name = s.Name
                            }).OrderBy(x => x.Name).ToList();

            return new ListResultDto<CallCategoryDto>(enumData.MapTo<List<CallCategoryDto>>());
        }

        public async Task<ListResultDto<GetProjectTypeDto>> GetProjectType()
        {
            var enumData = (from s in _projecttypeRepository.GetAll()
                            select new GetProjectTypeDto
                            {
                                Id = s.Id,
                                Name = s.ProjectTypeName
                            }).OrderBy(x => x.Name).ToList();

            return new ListResultDto<GetProjectTypeDto>(enumData.MapTo<List<GetProjectTypeDto>>());
        }


        public async Task<ListResultDto<GetMarketingUserDto>> GetMarketingUsers()
        {
          
            int curId = (int)_abpSession.UserId;
            bool checkUser = await _userManager.IsInRoleAsync(curId, "DubaiManager");
            if(checkUser)
            {
                var marketingName = (from u in _userRepository.GetAll()
                                     join ur in _userRoleRepository.GetAll()
                                     on u.Id equals ur.UserId
                                     join r in _roleRepository.GetAll()
                                     on ur.RoleId equals r.Id
                                     where (r.DisplayName == "Marketing Leader" || r.DisplayName == "TeleMarketing" || r.DisplayName == "Operating Leader") && u.CompanyId == 4
                                     select new GetMarketingUserDto
                                     {
                                         Id = (int)u.Id,
                                         Name = u.Name + " " + u.Surname
                                     }).Distinct().OrderBy(x => x.Name).ToList();
                marketingName.Add(new GetMarketingUserDto
                {
                    Id = (int)_userManager.FindByNameAsync("Nitin").Result.Id,
                    Name = _userManager.FindByNameAsync("Nitin").Result.Name+ " "+ _userManager.FindByNameAsync("Nitin").Result.Surname
                });
                marketingName.Add(new GetMarketingUserDto
                {
                    Id = (int)_userManager.FindByNameAsync("Jitu").Result.Id,
                    Name = _userManager.FindByNameAsync("Jitu").Result.Name + " " + _userManager.FindByNameAsync("Jitu").Result.Surname
                });

                marketingName= marketingName.OrderBy(x => x.Name).ToList();
                return new ListResultDto<GetMarketingUserDto>(marketingName.MapTo<List<GetMarketingUserDto>>());
            }
            else
            {
                var marketingName = (from u in _userRepository.GetAll()
                                     join ur in _userRoleRepository.GetAll()
                                     on u.Id equals ur.UserId
                                     join r in _roleRepository.GetAll()
                                     on ur.RoleId equals r.Id
                                     where r.DisplayName == "Marketing Leader" || r.DisplayName == "TeleMarketing" || r.DisplayName == "Operating Leader"
                                     select new GetMarketingUserDto
                                     {
                                         Id = (int)u.Id,
                                         Name = u.Name + " " + u.Surname
                                     }).Distinct().OrderBy(x => x.Name).ToList();
                return new ListResultDto<GetMarketingUserDto>(marketingName.MapTo<List<GetMarketingUserDto>>());
            }
            
        }
        public async Task<PagedResultDto<GetOpportunityDto>> MyOpportunityList(GetOpportunityInputDto input)
        {
            try
            {
                var companydata = _companyRepository.GetAll();
                int curId = (int)_abpSession.UserId;
                bool checkUser = await _userManager.IsInRoleAsync(curId, "Admin");
                var of = _OpportunityFollowUpRepository.GetAll();

                var cc = (from p in _OpportunityRepository.GetAll()
                          join c in _CallCategoriesRepository.GetAll()
                          on p.CalllCategoryId equals c.Id
                          join u in _userRepository.GetAll() on p.CreatorUserId equals u.Id
                          //join io in _InterestedOpportunityRepository.GetAll() on p.Id equals io.Opportunityid
                          //join pr in _projecttypeRepository.GetAll() on io.projectypeid equals pr.Id
                          //group pr by new { p, c } into g   //{p,u,c}
                          select new GetOpportunityDto
                          {
                              Id = p.Id,
                              CompanyName = p.CompanyName,
                              CalllCategoryId = p.CalllCategoryId,
                              AssignUserId = p.AssignUserId,
                              CallCategoryName = c.Name,
                              Comment = p.Comment,
                              PersonName = p.PersonName,
                              EmailId = p.EmailId,
                              MobileNumber = p.MobileNumber,
                              CreateUser = (int)p.CreatorUserId,
                              CreatedOn = p.CreationTime,
                              OpportunityOwner = p.OpportunityOwner,
                              //AssignUserName = u.Name + " " + u.Surname,
                              UploaderName = u.Name + " " + u.Surname,
                              //FollowUpCount = of.Where(x=>x.opporutnityid == p.Id).Select(x => x.Id).DefaultIfEmpty(0).Count(),
                              FollowUpCount = of.Where(x => x.opporutnityid == p.Id).Count(),
                              BeneficiaryCompanyId = p.BeneficiaryCompanyId,
                              BeneficiaryCompany = companydata.Where(x => x.Id == p.BeneficiaryCompanyId).Select(x => x.Beneficial_Company_Name).FirstOrDefault()
                          })
                          .Where(x => (x.CallCategoryName == "Opportunity Dead" || x.CallCategoryName == "Opportunity" || x.CallCategoryName == "Inquiry" || x.CallCategoryName == "Tele Caller") && (x.OpportunityOwner == curId || x.AssignUserId == curId))
                          .WhereIf(!input.CompanyName.IsNullOrEmpty(), p => p.CompanyName.ToLower().Contains(input.CompanyName.ToLower()))
                          .WhereIf(input.CalllCategoryId.HasValue, p => p.CalllCategoryId == input.CalllCategoryId)
                          .WhereIf(!input.PersonName.IsNullOrEmpty(), p => p.PersonName.ToLower().Contains(input.PersonName.ToLower()))
                          .WhereIf(!input.MobileNumber.IsNullOrEmpty(), p => p.MobileNumber.Contains(input.MobileNumber));
                          //.WhereIf(checkUser == false, p => p.CreateUser == curId)
                          ;

                var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
                var ccCount = cc.Count();
                return new PagedResultDto<GetOpportunityDto>(ccCount, ccData.MapTo<List<GetOpportunityDto>>());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        public async Task<PagedResultDto<GetOpportunityDto>> GeneralOpportunityList(GetOpportunityInputDto input)
        {
            try
            {
                var companydata = _companyRepository.GetAll();
                int curId = (int)_abpSession.UserId;
               
                bool checkUser = await _userManager.IsInRoleAsync(curId, "Admin");
                bool marketingUSer = checkUser;
                if (!checkUser)
                {
                    checkUser = await _userManager.IsInRoleAsync(curId, "Operating Leader");
                    marketingUSer = checkUser;
                }
                if(!marketingUSer)
                {
                    marketingUSer = await _userManager.IsInRoleAsync(curId, "Marketing Leader");
                }
                var of = _OpportunityFollowUpRepository.GetAll();

                var cc = (from p in _OpportunityRepository.GetAll()
                          join c in _CallCategoriesRepository.GetAll()
                          on p.CalllCategoryId equals c.Id
                          join b in _userRepository.GetAll() on p.AssignUserId equals b.Id
                          into ab
                          from b in ab.DefaultIfEmpty()
                          join u in _userRepository.GetAll() on p.CreatorUserId equals u.Id

                          //join io in _InterestedOpportunityRepository.GetAll() on p.Id equals io.Opportunityid
                          //join pr in _projecttypeRepository.GetAll() on io.projectypeid equals pr.Id
                          //group pr by new { p, c } into g   //{p,u,c}
                          select new GetOpportunityDto
                          {
                              Id = p.Id,
                              CompanyName = p.CompanyName,
                              CalllCategoryId = p.CalllCategoryId,

                              CallCategoryName = c.Name,
                              Comment = p.Comment,
                              PersonName = p.PersonName,
                              EmailId = p.EmailId,
                              MobileNumber = p.MobileNumber,
                              CreateUser = (int)p.CreatorUserId,
                              CreatedOn = p.CreationTime,
                              OpportunityOwner = p.OpportunityOwner,
                              AssignUserId = p.AssignUserId,
                              AssignUserName = b.Name + " " + b.Surname,
                              UploaderName = u.Name + " " + u.Surname,
                              //FollowUpCount = of.Where(x => x.opporutnityid == p.Id).Select(x => x.Id).DefaultIfEmpty(0).Count(),
                              FollowUpCount = of.Where(x => x.opporutnityid == p.Id).Count(),
                              BeneficiaryCompanyId = p.BeneficiaryCompanyId,
                              BeneficiaryCompany = companydata.Where(x=>x.Id == p.BeneficiaryCompanyId).Select(x=>x.Beneficial_Company_Name).FirstOrDefault(),
                          })
                          .Where(x => (x.CallCategoryName == "Opportunity" || x.CallCategoryName == "Tele Caller"))
                          .WhereIf(!input.CompanyName.IsNullOrEmpty(), p => p.CompanyName.ToLower().Contains(input.CompanyName.ToLower()))
                          //.WhereIf(input.CalllCategoryId.HasValue, p => p.CalllCategoryId == input.CalllCategoryId)
                          .WhereIf(checkUser == false, p => p.CreateUser == curId)
                          .WhereIf(marketingUSer == false, p => p.OpportunityOwner == 0)
                          .WhereIf(!input.PersonName.IsNullOrEmpty(), p => p.PersonName.ToLower().Contains(input.PersonName.ToLower()))
                          .WhereIf(!input.MobileNumber.IsNullOrEmpty(), p => p.MobileNumber.Contains(input.MobileNumber));

                var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
                var ccCount = cc.Count();
                return new PagedResultDto<GetOpportunityDto>(ccCount, ccData.MapTo<List<GetOpportunityDto>>());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PagedResultDto<GetOpportunityDto>> GetOpportunityList(GetOpportunityInputDto input)
        {
            try
            {
                var followupdata = _OpportunityFollowUpRepository.GetAll();
                var companydata = _companyRepository.GetAll();

                int curId = (int)_abpSession.UserId;
                bool checkUser = await _userManager.IsInRoleAsync(curId, "Admin");
                var cc = (from p in _OpportunityRepository.GetAll().Where(x => x.AssignUserId == curId)
                          join c in _CallCategoriesRepository.GetAll().Where(x => x.Name != "Opportunity Dead" && x.Name != "Opportunity")
                          on p.CalllCategoryId equals c.Id
                          join io in _InterestedOpportunityRepository.GetAll() on p.Id equals io.Opportunityid
                          join pr in _projecttypeRepository.GetAll() on io.projectypeid equals pr.Id
                          group pr by new { p, c } into g   //{p,u,c}
                          select new GetOpportunityDto
                          {
                              Id = g.Key.p.Id,
                              CompanyName = g.Key.p.CompanyName,
                              CalllCategoryId = g.Key.p.CalllCategoryId,

                              CallCategoryName = g.Key.c.Name,
                              Comment = g.Key.p.Comment,
                              PersonName = g.Key.p.PersonName,
                              EmailId = g.Key.p.EmailId,
                              MobileNumber = g.Key.p.MobileNumber,
                              ProjectType_Name = g.Select(y => y.ProjectTypeName).ToList(),
                              CreateUser = (int)g.Key.p.CreatorUserId,
                              CreatedOn = g.Key.p.CreationTime,
                              ProjectValue = g.Key.p.ProjectValue,
                              ExpectedClosingDate = g.Key.p.CalllCategoryId == 6 ? followupdata.Where(x => x.opporutnityid == g.Key.p.Id).OrderByDescending(x => x.Id).Select(x => x.expectedclosingdate).FirstOrDefault() : null,
                              Reason = followupdata.Where(x => x.opporutnityid == g.Key.p.Id).OrderByDescending(x => x.Id).Select(x => x.Comment).FirstOrDefault(),
                              ClosedAmount = g.Key.p.ProjectValue,
                              BeneficiaryCompany = companydata.Where(x => x.Id == g.Key.p.BeneficiaryCompanyId).Select(x => x.Beneficial_Company_Name).FirstOrDefault()
                          })
                          .WhereIf(!input.CompanyName.IsNullOrEmpty(), p => p.CompanyName.ToLower().Contains(input.CompanyName.ToLower()))
                          .WhereIf(input.CalllCategoryId.HasValue, p => p.CalllCategoryId == input.CalllCategoryId)
                          //.WhereIf(checkUser == false, p => p.CreateUser == curId)
                          ;

                var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
                var ccCount = cc.Count();
                return new PagedResultDto<GetOpportunityDto>(ccCount, ccData.MapTo<List<GetOpportunityDto>>());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public async Task OpportunityCreate(createDto input)
        {
            Opportunity obj = new Opportunity();
            obj.CompanyName = input.CompanyName;
            obj.PersonName = input.PersonName;
            obj.MobileNumber = input.MobileNumber;
            obj.EmailId = input.EmailId;
            obj.CalllCategoryId = input.CalllCategoryId;
            obj.ProjectValue = input.ProjectValue;
            obj.BeneficiaryCompanyId = input.BeneficiaryCompanyId;
            //obj.AssignUserId = 0;

            int curId = (int)_abpSession.UserId;
            obj.AssignUserId = curId;

            obj.Comment = input.Comment;
            int id = 0;
            try
            {
                id = await _OpportunityRepository.InsertAndGetIdAsync(obj);
            }
            catch (Exception ex)
            {

                throw;
            }
            if (id > 0)
            {
                for (int i = 0; i < input.ProjectType.Length; i++)
                {
                    InterestedOpportunity pr = new InterestedOpportunity();
                    pr.Opportunityid = id;
                    pr.projectypeid = Convert.ToInt32(input.ProjectType[i]);
                    await _InterestedOpportunityRepository.InsertAsync(pr);
                }
                OpportunityFollowUp foll = new OpportunityFollowUp();
                foll.nextactiondate = input.nextactiondate.HasValue ? input.nextactiondate.Value : DateTime.Now;
                foll.opporutnityid = id;
                foll.Comment = input.Comment;
                foll.CalllCategoryId = input.CalllCategoryId;
                int ids = await _OpportunityFollowUpRepository.InsertAndGetIdAsync(foll);
                if (ids > 0)
                {
                    for (int i = 0; i < input.ProjectType.Length; i++)
                    {
                        FollowupIntrest ints = new FollowupIntrest();
                        ints.followupid = ids;
                        ints.intestedid = Convert.ToInt32(input.ProjectType[i]);
                        await _FollowupIntrestRepository.InsertAsync(ints);
                    }
                }
            }
        }

        public async Task<GetOpportunityDto> GetOpportunityEdit(EntityDto input)
        {
            var data = (await _OpportunityRepository.GetAsync(input.Id)).MapTo<GetOpportunityDto>();
            var datalast = _OpportunityFollowUpRepository.GetAll().Where(x => x.opporutnityid == input.Id).OrderByDescending(x => x.Id).FirstOrDefault();
            var opid = (await _OpportunityFollowUpRepository.GetAsync(datalast.Id));
            data.ProjectTypeName = _InterestedOpportunityRepository.GetAll().Where(x => x.Opportunityid == input.Id).Select(x => x.projectypeid).ToArray();
            //data.nextactiondate = Convert.ToDateTime(_OpportunityFollowUpRepository.GetAll().Where(x => x.opporutnityid == opid.Id).Select(x => x.nextactiondate.Value).FirstOrDefault());
            data.nextactiondate = opid.nextactiondate;
            return data;
        }

        public async Task UpdateOpportunity(OpportunityUpdateDto input)
        {
            int curId = (int)_abpSession.UserId;

            var obj = await _OpportunityRepository.GetAsync(input.Id);
            obj.CompanyName = input.CompanyName;
            obj.PersonName = input.PersonName;
            obj.MobileNumber = input.MobileNumber;
            obj.EmailId = input.EmailId;
            obj.CalllCategoryId = input.CalllCategoryId;
            obj.AssignUserId = curId;
            obj.Comment = input.Comment;
            obj.ProjectValue = input.ProjectValue;
            obj.BeneficiaryCompanyId = input.BeneficiaryCompanyId;

            await _OpportunityRepository.UpdateAsync(obj);
            await _InterestedOpportunityRepository.DeleteAsync(x => x.Opportunityid == input.Id);
            for (int i = 0; i < input.ProjectType.Length; i++)
            {
                InterestedOpportunity pr = new InterestedOpportunity();
                pr.Opportunityid = input.Id;
                pr.projectypeid = Convert.ToInt32(input.ProjectType[i]);
                await _InterestedOpportunityRepository.InsertAsync(pr);
            }
            OpportunityFollowUp foll = new OpportunityFollowUp();
            foll.nextactiondate = input.nextactiondate.HasValue ? input.nextactiondate.Value : DateTime.Now;
            foll.opporutnityid = input.Id;
            foll.Comment = input.Comment;
            foll.CalllCategoryId = input.CalllCategoryId;
            int ids = await _OpportunityFollowUpRepository.InsertAndGetIdAsync(foll);
            if (ids > 0)
            {
                for (int i = 0; i < input.ProjectType.Length; i++)
                {
                    FollowupIntrest ints = new FollowupIntrest();
                    ints.followupid = ids;
                    ints.intestedid = Convert.ToInt32(input.ProjectType[i]);
                    await _FollowupIntrestRepository.InsertAsync(ints);
                }
            }
        }

        public ListResultDto<GetFollowUpCountDto> GetfollowCountLists(GetFollowUpFilterDto inputs)
        {
            DateTime dtfrm = new DateTime();
            DateTime dt = new DateTime();


            if (inputs.FromDate != null)
            {
                var frmdate = inputs.FromDate.ToString("MM/dd/yyyy");

                var gt = Convert.ToString(frmdate + " 00:00:00");
                dtfrm = Convert.ToDateTime(gt);

            }
            if (inputs.ToDate != null)
            {
                var todate = inputs.ToDate.ToString("MM/dd/yyyy");
                var gt = Convert.ToString(todate + " 23:59:59");
                dt = Convert.ToDateTime(gt);
            }
            int curId = (int)_abpSession.UserId;
            //var entitesQuery = (from e in _OpportunityFollowUpRepository.GetAll()
            //                    join u in _userRepository.GetAll()
            //                   on e.CreatorUserId equals (int)u.Id
            //                    join r in _userRoleRepository.GetAll()
            //                    on u.Id equals r.UserId
            //                    join ur in _roleRepository.GetAll()
            //                    on r.RoleId equals ur.Id
            //                    where e.CreationTime >= dtfrm && e.CreationTime <= dt
            //                    group e by new
            //                    {
            //                        e.opporutnityid,
            //                        u.Name,
            //                        u.Surname

            //                    } into g
            //                    select new GetFollowUpCountDto
            //                    {
            //                        Name = g.Key.Name + " " + g.Key.Surname,

            //                        count = g.Count()
            //                    })
            //                    .OrderByDescending(x => x.Id)
            //                   .ToList(); ;
            var entitesQuery = (from e in _OpportunityFollowUpRepository.GetAll()
                                join o in _OpportunityRepository.GetAll()/*.Where(x=>x.AssignUserId==curId)*/
                                on e.opporutnityid equals o.Id
                                join u in _userRepository.GetAll()
                               on o.AssignUserId equals (int)u.Id
                                where (e.nextactiondate >= dtfrm && e.nextactiondate <= dt) && o.AssignUserId == (int)curId
                                select new
                                {
                                    Name = u.Name + " " + u.Surname,
                                    opId = e.opporutnityid
                                }).Distinct()
                                .GroupBy(info => info.Name)
                        .Select(group => new GetFollowUpCountDto
                        {
                            Name = group.Key,
                            count = group.Count()
                        }).ToList();
            return new ListResultDto<GetFollowUpCountDto>(entitesQuery.MapTo<List<GetFollowUpCountDto>>());
        }

        public List<GetOpportunityReportDto> OpportunityReportExport(GetOpportunityInputDto input)
        {
            var companydata = _companyRepository.GetAll();
            var followupdata = _OpportunityFollowUpRepository.GetAll();
            var frmdate = input.FromDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.FromDate.Value.ToString("MM/dd/yyyy");
            DateTime dtfrm = Convert.ToDateTime(frmdate + " 00:00:00");

            var todate = input.ToDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.ToDate.Value.ToString("MM/dd/yyyy");
            DateTime dt = Convert.ToDateTime(todate + " 23:59:59");
            int curId = (int)_abpSession.UserId;
            var cc = (from p in _OpportunityRepository.GetAll()
                      join u in _userRepository.GetAll()
                      on p.AssignUserId equals u.Id
                      join c in _CallCategoriesRepository.GetAll()
                      on p.CalllCategoryId equals c.Id
                      join io in _InterestedOpportunityRepository.GetAll() on p.Id equals io.Opportunityid
                      join pr in _projecttypeRepository.GetAll() on io.projectypeid equals pr.Id
                      group pr by new { p, c, u } into g
                      select new GetOpportunityReportDto
                      {
                          Id = g.Key.p.Id,
                          CompanyName = g.Key.p.CompanyName,
                          AssignUserId = g.Key.p.AssignUserId,
                          AssignUserName = g.Key.u.Name + " " + g.Key.u.Surname,
                          CalllCategoryId = g.Key.p.CalllCategoryId,
                          CallCategoryName = g.Key.c.Name,
                          Comment = g.Key.p.Comment,
                          PersonName = g.Key.p.PersonName,
                          ProjectType_Name = g.Select(y => y.ProjectTypeName).ToList(),
                          ProjectValue = g.Key.p.ProjectValue,
                          CreateUser = (int)g.Key.p.CreatorUserId,
                          CreateDate = g.Key.p.CreationTime,
                          EmailId = g.Key.p.EmailId,
                          MobileNumber = g.Key.p.MobileNumber,
                          ExpectedClosingDate = g.Key.p.CalllCategoryId == 6 ? followupdata.Where(x => x.opporutnityid == g.Key.p.Id).OrderByDescending(x => x.Id).Select(x => x.expectedclosingdate).FirstOrDefault() : null,
                          Reason = followupdata.Where(x => x.opporutnityid == g.Key.p.Id).OrderByDescending(x => x.Id).Select(x => x.Comment).FirstOrDefault(),
                          ClosedAmount = g.Key.p.ProjectValue,
                          BeneficiaryCompanyId = g.Key.p.BeneficiaryCompanyId,
                          BeneficiaryCompany = companydata.Where(x=>x.Id == g.Key.p.BeneficiaryCompanyId).Select(x=>x.Beneficial_Company_Name).FirstOrDefault(),
                      })
                      .WhereIf(!input.CompanyName.IsNullOrEmpty(), p => p.CompanyName.ToLower().Contains(input.CompanyName.ToLower()))
                      .WhereIf(input.CalllCategoryId.HasValue, p => p.CalllCategoryId == input.CalllCategoryId)
                      .WhereIf(input.AssignUserId.HasValue, p => p.AssignUserId == input.AssignUserId)
                      // .WhereIf(input.CurrentUser.HasValue, p => p.CreateUser == input.CurrentUser)
                      .WhereIf(input.FromDate.HasValue, p => p.CreateDate >= dtfrm)
                      .WhereIf(input.ToDate.HasValue, p => p.CreateDate <= dt)
                      .WhereIf(input.BeneficiaryCompanyId.HasValue, p => p.BeneficiaryCompanyId == input.BeneficiaryCompanyId).ToList();
            return new List<GetOpportunityReportDto>(cc);
        }

        public async Task<PagedResultDto<GetOpportunityReportDto>> GetOpportunityReport(GetOpportunityInputDto input)
        {
            var followupdata = _OpportunityFollowUpRepository.GetAll();
            var companydata = _companyRepository.GetAll();
            var frmdate = input.FromDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.FromDate.Value.ToString("MM/dd/yyyy");
            DateTime dtfrm = Convert.ToDateTime(frmdate + " 00:00:00");

            var todate = input.ToDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.ToDate.Value.ToString("MM/dd/yyyy");
            DateTime dt = Convert.ToDateTime(todate + " 23:59:59");

            int curId = (int)_abpSession.UserId;
            bool checkUser = await _userManager.IsInRoleAsync(curId, "DubaiManager");

            var cc = (from p in _OpportunityRepository.GetAll()
                      join u in _userRepository.GetAll()
                      on p.AssignUserId equals u.Id
                      join c in _CallCategoriesRepository.GetAll()
                      on p.CalllCategoryId equals c.Id
                      join io in _InterestedOpportunityRepository.GetAll() on p.Id equals io.Opportunityid
                      join pr in _projecttypeRepository.GetAll() on io.projectypeid equals pr.Id
                      group pr by new { p, c, u } into g
                      select new GetOpportunityReportDto
                      {
                          Id = g.Key.p.Id,
                          CompanyName = g.Key.p.CompanyName,
                          AssignUserId = g.Key.p.AssignUserId,
                          AssignUserName = g.Key.u.Name + " " + g.Key.u.Surname,
                          CalllCategoryId = g.Key.p.CalllCategoryId,
                          CallCategoryName = g.Key.c.Name,
                          Comment = g.Key.p.Comment,
                          PersonName = g.Key.p.PersonName,
                          ProjectValue = g.Key.p.ProjectValue,
                          ProjectType_Name = g.Select(y => y.ProjectTypeName).ToList(),
                          CreateUser = (int)g.Key.p.CreatorUserId,
                          CreateDate = g.Key.p.CreationTime,
                          EmailId = g.Key.p.EmailId,
                          MobileNumber = g.Key.p.MobileNumber,
                          ExpectedClosingDate = g.Key.p.CalllCategoryId == 6 ? followupdata.Where(x => x.opporutnityid == g.Key.p.Id).OrderByDescending(x => x.Id).Select(x => x.expectedclosingdate).FirstOrDefault() : null,
                          Reason = followupdata.Where(x => x.opporutnityid == g.Key.p.Id).OrderByDescending(x => x.Id).Select(x => x.Comment).FirstOrDefault(),
                          ClosedAmount = g.Key.p.ProjectValue,
                          BeneficiaryCompanyId = g.Key.p.BeneficiaryCompanyId,
                          BeneficiaryCompany = companydata.Where(x=>x.Id == g.Key.p.BeneficiaryCompanyId).Select(x=>x.Beneficial_Company_Name).FirstOrDefault(),
                      })
                      .WhereIf(!input.CompanyName.IsNullOrEmpty(), p => p.CompanyName.ToLower().Contains(input.CompanyName.ToLower()))
                      .WhereIf(input.CalllCategoryId.HasValue, p => p.CalllCategoryId == input.CalllCategoryId)
                      .WhereIf(input.AssignUserId.HasValue, p => p.AssignUserId == input.AssignUserId)
            // .WhereIf(input.CurrentUser.HasValue, p => p.CreateUser == input.CurrentUser)
                       .WhereIf(input.FromDate.HasValue, p => p.CreateDate >= dtfrm)
                       .WhereIf(input.ToDate.HasValue, p => p.CreateDate <= dt)
                       .WhereIf(input.BeneficiaryCompanyId.HasValue, p => p.BeneficiaryCompanyId == input.BeneficiaryCompanyId);

            if (checkUser)
            {
                int beneid = _companyRepository.GetAll().Where(x => x.Beneficial_Company_Name == "Megh Technologies LLC").Select(x => x.Id).FirstOrDefault();
                cc = cc.Where(x => x.BeneficiaryCompanyId == beneid);
            }

            var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
            var ccCount = cc.Count();
            return new PagedResultDto<GetOpportunityReportDto>(ccCount, ccData.MapTo<List<GetOpportunityReportDto>>());
        }

        public PagedResultDto<GetDailySalesActivityReportDto> DailySalesActivityReport(GetOpportunityInputDto input)
        {
            var frmdate = input.FromDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.FromDate.Value.ToString("MM/dd/yyyy");
            DateTime dtfrm = Convert.ToDateTime(frmdate + " 00:00:00");

            var todate = input.ToDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.ToDate.Value.ToString("MM/dd/yyyy");
            DateTime dt = Convert.ToDateTime(todate + " 23:59:59");
            int curId = (int)_abpSession.UserId;


            var of1 = _OpportunityFollowUpRepository.GetAll();


            var cc = (from p in _OpportunityRepository.GetAll()
                      join u in _userRepository.GetAll()
                      on p.AssignUserId equals u.Id
                      join c in _CallCategoriesRepository.GetAll()
                      on p.CalllCategoryId equals c.Id
                      join io in _InterestedOpportunityRepository.GetAll() on p.Id equals io.Opportunityid
                      join pr in _projecttypeRepository.GetAll() on io.projectypeid equals pr.Id
                      group pr by new { p, c, u } into g
                      select new GetDailySalesActivityReportDto
                      {
                          Id = g.Key.p.Id,
                          CompanyName = g.Key.p.CompanyName,
                          AssignUserId = g.Key.p.AssignUserId,
                          AssignUserName = g.Key.u.Name + " " + g.Key.u.Surname,
                          CalllCategoryId = g.Key.p.CalllCategoryId,
                          CallCategoryName = g.Key.c.Name,
                          Comment = g.Key.p.Comment,
                          PersonName = g.Key.p.PersonName,
                          ProjectType_Name = g.Select(y => y.ProjectTypeName).ToList(),
                          CreateUser = (int)g.Key.p.CreatorUserId,
                          CreateDate = g.Key.p.CreationTime,
                          EmailId = g.Key.p.EmailId,
                          MobileNumber = g.Key.p.MobileNumber,

                          FollowupCount = of1.Where(x=>x.opporutnityid == g.Key.p.Id).Select(x => x.Id).DefaultIfEmpty(0).Count(),
                          //FollowupCount = of.Where(x => x.opporutnityid == g.Key.p.Id).Count(),
                          //FollowupCount = 2,

                      })
                      .WhereIf(!input.CompanyName.IsNullOrEmpty(), p => p.CompanyName.ToLower().Contains(input.CompanyName.ToLower()))
                      .WhereIf(input.CalllCategoryId.HasValue, p => p.CalllCategoryId == input.CalllCategoryId)
                      .WhereIf(input.AssignUserId.HasValue, p => p.AssignUserId == input.AssignUserId)
                      // .WhereIf(input.CurrentUser.HasValue, p => p.CreateUser == input.CurrentUser)
                      .WhereIf(input.FromDate.HasValue, p => p.CreateDate >= dtfrm)
                      .WhereIf(input.ToDate.HasValue, p => p.CreateDate <= dt); ;

            var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
            var ccCount = cc.Count();
            //return new PagedResultDto<GetDailySalesActivityReportDto>(ccCount, ccData.MapTo<List<GetDailySalesActivityReportDto>>());
            return new PagedResultDto<GetDailySalesActivityReportDto>(ccCount, ccData);
        }


        public List<GetDailySalesActivityReportDto> DailySalesActivityReportExport(GetOpportunityExportInputDto input)
        {
            var frmdate = input.FromDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.FromDate.Value.ToString("MM/dd/yyyy");
            DateTime dtfrm = Convert.ToDateTime(frmdate + " 00:00:00");

            var todate = input.ToDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.ToDate.Value.ToString("MM/dd/yyyy");
            DateTime dt = Convert.ToDateTime(todate + " 23:59:59");
            int curId = (int)_abpSession.UserId;


            var of1 = _OpportunityFollowUpRepository.GetAll();


            var cc = (from p in _OpportunityRepository.GetAll()
                      join u in _userRepository.GetAll()
                      on p.AssignUserId equals u.Id
                      join c in _CallCategoriesRepository.GetAll()
                      on p.CalllCategoryId equals c.Id
                      join io in _InterestedOpportunityRepository.GetAll() on p.Id equals io.Opportunityid
                      join pr in _projecttypeRepository.GetAll() on io.projectypeid equals pr.Id
                      group pr by new { p, c, u } into g
                      select new GetDailySalesActivityReportDto
                      {
                          Id = g.Key.p.Id,
                          CompanyName = g.Key.p.CompanyName,
                          AssignUserId = g.Key.p.AssignUserId,
                          AssignUserName = g.Key.u.Name + " " + g.Key.u.Surname,
                          CalllCategoryId = g.Key.p.CalllCategoryId,
                          CallCategoryName = g.Key.c.Name,
                          Comment = g.Key.p.Comment,
                          PersonName = g.Key.p.PersonName,
                          ProjectType_Name = g.Select(y => y.ProjectTypeName).ToList(),
                          CreateUser = (int)g.Key.p.CreatorUserId,
                          CreateDate = g.Key.p.CreationTime,
                          EmailId = g.Key.p.EmailId,
                          MobileNumber = g.Key.p.MobileNumber,

                          FollowupCount = of1.Where(x => x.opporutnityid == g.Key.p.Id).Select(x => x.Id).DefaultIfEmpty(0).Count(),
                          //FollowupCount = of.Where(x => x.opporutnityid == g.Key.p.Id).Count(),
                          //FollowupCount = 2,

                      })
                      .WhereIf(!input.CompanyName.IsNullOrEmpty(), p => p.CompanyName.ToLower().Contains(input.CompanyName.ToLower()))
                      .WhereIf(input.CalllCategoryId.HasValue, p => p.CalllCategoryId == input.CalllCategoryId)
                      .WhereIf(input.AssignUserId.HasValue, p => p.AssignUserId == input.AssignUserId)
                      // .WhereIf(input.CurrentUser.HasValue, p => p.CreateUser == input.CurrentUser)
                      .WhereIf(input.FromDate.HasValue, p => p.CreateDate >= dtfrm)
                      .WhereIf(input.ToDate.HasValue, p => p.CreateDate <= dt); ;

            var ccData = cc.ToList();
            var ccCount = cc.Count();
            //return new PagedResultDto<GetDailySalesActivityReportDto>(ccCount, ccData.MapTo<List<GetDailySalesActivityReportDto>>());
            return new List<GetDailySalesActivityReportDto>(ccData);

        }


        public async Task<GetOpportunityDto> GetOpportunityDetails(EntityDto input)
        {
            var data = (await _OpportunityRepository.GetAsync(input.Id)).MapTo<GetOpportunityDto>();
            data.ProjectType_Name = (from op in _InterestedOpportunityRepository.GetAll()
                                     join pr in _projecttypeRepository.GetAll()
                                     on op.projectypeid equals pr.Id
                                     where op.Opportunityid == data.Id
                                     select new
                                     {
                                         name = pr.ProjectTypeName
                                     }).Select(x => x.name).ToList();
            data.CallCategoryName = _CallCategoriesRepository.GetAll().Where(x => x.Id == data.CalllCategoryId).Select(x => x.Name).FirstOrDefault().ToString();
            data.BeneficiaryCompany = _companyRepository.GetAll().Where(x => x.Id == data.BeneficiaryCompanyId).Select(x => x.Beneficial_Company_Name).FirstOrDefault();
            data.FollowUpTypeId = _OpportunityFollowUpRepository.GetAll().Where(x => x.opporutnityid == data.Id).OrderByDescending(x => x.Id).Select(x => x.Followuptypeid).FirstOrDefault();
            data.FollowUpType = _FollowuptypeRepository.GetAll().Where(x => x.Id == data.FollowUpTypeId).Select(x => x.FollowUpType).FirstOrDefault();
            //data.AssignUserName = _userRepository.GetAll().Where(x => x.Id == data.AssignUserId).Select(x => x.Name).FirstOrDefault().ToString() + " " + _userRepository.GetAll().Where(x => x.Id == data.AssignUserId).Select(x => x.Surname).FirstOrDefault().ToString();
            return data;
        }

        public async Task<GetFollowUpDetailDto> GetFollowUpDetail(EntityDto input)
        {
            GetFollowUpDetailDto obj = new GetFollowUpDetailDto();
            var oppdata = _OpportunityRepository.GetAll().Where(x => x.Id == input.Id);
            var datalast = _OpportunityFollowUpRepository.GetAll().Where(x => x.opporutnityid == input.Id).OrderByDescending(x => x.Id).FirstOrDefault();
            if (datalast != null)
            {
                var data = (await _OpportunityFollowUpRepository.GetAsync(datalast.Id));
                obj.opporutnityid = data.opporutnityid;
                obj.Id = data.Id;
                obj.CalllCategoryId = data.CalllCategoryId;
                //obj.nextactiondate = data.nextactiondate;
                obj.expectedclosingdate = data.expectedclosingdate;
                // obj.Comment = data.Comment;
                obj.ProjectTypeName = _FollowupIntrestRepository.GetAll().Where(x => x.followupid == data.Id).Select(x => x.intestedid).ToArray();
                obj.CompanyName = oppdata.Select(x => x.CompanyName).FirstOrDefault();
                obj.PersonName = oppdata.Select(x => x.PersonName).FirstOrDefault();
                obj.EmailId = oppdata.Select(x => x.EmailId).FirstOrDefault();
                obj.MobileNumber = oppdata.Select(x => x.MobileNumber).FirstOrDefault();
                obj.BeneficiaryCompanyId = oppdata.Select(x => x.BeneficiaryCompanyId).FirstOrDefault();
                obj.BeneficiaryCompany = obj.BeneficiaryCompanyId != 0 ?_companyRepository.GetAll().Where(x => x.Id == obj.BeneficiaryCompanyId).Select(x => x.Beneficial_Company_Name).FirstOrDefault() : null;
                return obj;
            }
            else
            {
                return null;
            }
        }

        public PagedResultDto<UserDto> GetUserMarketingLead()
        {
            var User_List = _userRepository.GetAll();
            var uid = (int)User_List.Where(u => u.Id == _abpSession.UserId).Select(t => t.Id).FirstOrDefault();
            List<User> userQuery = new List<User>();
            userQuery = (from u in _userRepository.GetAll()
                         join
                         r in _userRoleRepository.GetAll()
                         on u.Id equals r.UserId
                         join
                         role in _roleRepository.GetAll()
                         on r.RoleId equals role.Id
                         where (role.Name == "Marketing Leader" || role.Name == "TeleMarketing") && u.IsActive == true
                         select u).OrderBy(x => x.Name).Distinct().ToList();
            var userData = userQuery.ToList();
            var userCount = userQuery.Count();

            return new PagedResultDto<UserDto>(userCount, userData.MapTo<List<UserDto>>());
        }
        public async Task UpdateFollowUpClosed(EntityDto input)
        {
            var obj = await _OpportunityRepository.GetAsync(input.Id);
            obj.CalllCategoryId = 6;
            await _OpportunityRepository.UpdateAsync(obj);
        }
        public async Task UpdateFollowUp(UpdateFollowUpDto input)
        {
            var obj = await _OpportunityRepository.GetAsync(input.opporutnityid);

            obj.Comment = input.Comment;
            obj.ProjectValue = input.ProjectValue;

            if (input.CalllCategoryId == 1 || input.CalllCategoryId == 5)
            {
                obj.CalllCategoryId = input.CalllCategoryId;
                obj.Remarks = input.Comment;
                obj.ActionDate = DateTime.Now;
                await _OpportunityRepository.UpdateAsync(obj);
            }
            else if (input.CalllCategoryId == 8)
            {
                obj.CalllCategoryId = input.CalllCategoryId;
                obj.Remarks = input.Comment;
                obj.ActionDate = DateTime.Now;
                obj.Comment = input.Comment;
                await _OpportunityRepository.UpdateAsync(obj);
            }
            else if (input.CalllCategoryId == 7 || input.CalllCategoryId == 10)
            {
                obj.CalllCategoryId = input.CalllCategoryId;
                obj.Remarks = input.Comment;
                obj.ActionDate = DateTime.Now;
                obj.CalllCategoryId = input.CalllCategoryId;
                await _OpportunityRepository.UpdateAsync(obj);
                OpportunityFollowUp foll = new OpportunityFollowUp();
                foll.opporutnityid = input.opporutnityid;
                foll.Comment = input.Comment;
                foll.CalllCategoryId = input.CalllCategoryId;
                foll.nextactiondate = input.nextactiondate;
                foll.Followuptypeid = input.FollowuptypeId;
                await _OpportunityFollowUpRepository.InsertAndGetIdAsync(foll);
            }
            else if (input.CalllCategoryId == 6)
            {
                obj.CalllCategoryId = input.CalllCategoryId;
                obj.Remarks = input.Comment;
                //obj.ActionDate = DateTime.Now;
                obj.ProjectValue = input.ProjectValue;
                await _OpportunityRepository.UpdateAsync(obj);
                OpportunityFollowUp foll = new OpportunityFollowUp();
                foll.opporutnityid = input.opporutnityid;
                foll.Followuptypeid = input.FollowuptypeId;
                foll.Comment = input.Comment;
                foll.CalllCategoryId = input.CalllCategoryId;
                foll.nextactiondate = input.nextactiondate;
                foll.expectedclosingdate = input.expectedclosingdate != null ? input.expectedclosingdate : DateTime.Now;
                await _OpportunityFollowUpRepository.InsertAndGetIdAsync(foll);
            }
            else
            {
                if (input.AssignUserId == 0)
                {
                    obj.AssignUserId = obj.AssignUserId;
                }
                else
                {
                    obj.AssignUserId = input.AssignUserId;
                }

                obj.CalllCategoryId = input.CalllCategoryId;
                await _OpportunityRepository.UpdateAsync(obj);
                await _InterestedOpportunityRepository.DeleteAsync(x => x.Opportunityid == input.opporutnityid);
                for (int i = 0; i < input.ProjectType.Length; i++)
                {
                    InterestedOpportunity pr = new InterestedOpportunity();
                    pr.Opportunityid = input.opporutnityid;
                    pr.projectypeid = Convert.ToInt32(input.ProjectType[i]);
                    await _InterestedOpportunityRepository.InsertAsync(pr);
                }
                OpportunityFollowUp foll = new OpportunityFollowUp();
                foll.nextactiondate = input.nextactiondate.HasValue ? input.nextactiondate : null;
                foll.expectedclosingdate = input.expectedclosingdate.HasValue ? input.expectedclosingdate : null;
                foll.opporutnityid = input.opporutnityid;
                foll.Comment = input.Comment;
                foll.CalllCategoryId = input.CalllCategoryId;
                foll.Followuptypeid = input.FollowuptypeId;
                int ids = await _OpportunityFollowUpRepository.InsertAndGetIdAsync(foll);
                if (ids > 0)
                {
                    for (int i = 0; i < input.ProjectType.Length; i++)
                    {
                        FollowupIntrest ints = new FollowupIntrest();
                        ints.followupid = ids;
                        ints.intestedid = Convert.ToInt32(input.ProjectType[i]);
                        await _FollowupIntrestRepository.InsertAsync(ints);
                    }
                }
            }

        }
        //old query
        public ListResultDto<FollowupHistoryDto> FollowUpHistoryData(int Opporutnityid = 0)
        {
            var followuptypedata = _FollowuptypeRepository.GetAll();
            var k = _OpportunityFollowUpRepository.GetAll();
            var t = _FollowupIntrestRepository.GetAll();
            var p = _projecttypeRepository.GetAll();
            //var data = (from a in _OpportunityFollowUpRepository.GetAll()
            //            join b in _OpportunityRepository.GetAll() on a.opporutnityid equals b.Id
            //            into ab
            //            from b in ab.DefaultIfEmpty()
            //            join c in _CallCategoriesRepository.GetAll() on a.CalllCategoryId equals c.Id
            //            join d in _FollowupIntrestRepository.GetAll() on a.Id equals d.followupid
            //            join e in _projecttypeRepository.GetAll() on d.intestedid equals e.Id
            //            where a.opporutnityid == Opporutnityid
            //            group e by new { a, c } into g
            //            select new FollowupHistoryDto
            //            {
            //                Opporutnityid = g.Key.a.Id,
            //                CreationDate = g.Key.a.CreationTime,
            //                CalllCategoryId = g.Key.a.CalllCategoryId,
            //                CalllCategoryName = g.Key.c.Name,
            //                ProjectType = g.Select(x => x.ProjectTypeName).ToList(),
            //                NextActionDate = g.Key.a.nextactiondate.HasValue ? g.Key.a.nextactiondate : null,
            //                ClosingDate = g.Key.a.expectedclosingdate.HasValue ? g.Key.a.expectedclosingdate : null,
            //                Comment = g.Key.a.Comment,
            //            })
            //            .OrderByDescending(x => x.Opporutnityid)
            //            .ToList();

            var datacheck = (from a in _OpportunityFollowUpRepository.GetAll()
                             join b in _OpportunityRepository.GetAll() on a.opporutnityid equals b.Id
                             join c in _CallCategoriesRepository.GetAll() on a.CalllCategoryId equals c.Id
                             where a.opporutnityid == Opporutnityid
                             select new FollowupHistoryDto
                             {
                                 Opporutnityid = a.Id,
                                 CreationDate = a.CreationTime,
                                 CalllCategoryId = a.CalllCategoryId,
                                 CalllCategoryName = c.Name,
                                 projectNames = (from z in k
                                                 join
                                                 d in t.Where(x => x.followupid == a.Id)
                                                 on z.Id equals d.followupid
                                                 into zd
                                                 from d in zd.DefaultIfEmpty()
                                                 join e in p
                                                 on d.intestedid equals e.Id
                                                 where z.opporutnityid == Opporutnityid
                                                 select new ProjectName
                                                 {
                                                     Name = e.ProjectTypeName,
                                                 }).ToList(),
                                 //ProjectType = g.Select(x => x.ProjectTypeName).ToList(),
                                 NextActionDate = a.nextactiondate.HasValue ? a.nextactiondate : null,
                                 ClosingDate = a.expectedclosingdate.HasValue ? a.expectedclosingdate : null,
                                 Comment = a.Comment,
                                 FollowUpTypeId = a.Followuptypeid,
                                 FollowUpType = a.Followuptypeid != 0 ? followuptypedata.Where(x => x.Id == a.Followuptypeid).Select(x => x.FollowUpType).FirstOrDefault() : null
                             })
                             .OrderByDescending(x => x.Opporutnityid)
                        .ToList();
            return new ListResultDto<FollowupHistoryDto>(datacheck.MapTo<List<FollowupHistoryDto>>());
        }


        //public ListResultDto<FollowupHistoryDto> FollowUpHistoryDatad(int Opporutnityid = 0)
        //{
        //    var da = _projecttypeRepository.GetAll();
        //    var data = (from a in _OpportunityRepository.GetAll()
        //                join b in _OpportunityFollowUpRepository.GetAll() on a.Id equals b.opporutnityid
        //                join c in _CallCategoriesRepository.GetAll() on b.CalllCategoryId equals c.Id
        //                join d in _FollowupIntrestRepository.GetAll() on b.Id equals d.followupid
        //                into bd
        //                from d in bd.DefaultIfEmpty()
        //               // join e in _projecttypeRepository.GetAll() on d.intestedid equals e.Id
        //                where a.Id == Opporutnityid
        //                //group e by new { a, c ,b} into g
        //                select new FollowupHistoryDto
        //                {
        //                    Opporutnityid = b.Id,
        //                    CreationDate = b.CreationTime,
        //                    CalllCategoryId = b.CalllCategoryId,
        //                    CalllCategoryName = c.Name,
        //                    //ProjectType = g.Select(x => x.ProjectTypeName).ToList(),
        //                    ProjectType= da.Where(x=>x.Id==d.intestedid).Select(x=>x.ProjectTypeName).ToList(),
        //                    NextActionDate = b.nextactiondate.HasValue ? b.nextactiondate : null,
        //                    ClosingDate = b.expectedclosingdate.HasValue ? b.expectedclosingdate : null,
        //                    Comment = b.Comment
        //                })
        //                .OrderByDescending(x => x.Opporutnityid)
        //                .ToList();
        //    //var checknull = data.Select(x => x.ProjectType);
        //    //if (checknull!=null)
        //    //{
        //    //    var dataa=(from d in data.ToList())
        //    //}



        //    return new ListResultDto<FollowupHistoryDto>(data.MapTo<List<FollowupHistoryDto>>());
        //}

        public async Task<ProjectTypeDto> GetProjectTypesDetails(EntityDto input)
        {
            Opportunity kk = new Opportunity();
            var data = (await _OpportunityRepository.GetAsync(input.Id)).MapTo<ProjectTypeDto>();
            data.ProjectType_Name = (from op in _InterestedOpportunityRepository.GetAll()
                                     join pr in _projecttypeRepository.GetAll()
                                     on op.projectypeid equals pr.Id
                                     where op.Opportunityid == data.Id
                                     select new ProjectList
                                     {
                                         Name = pr.ProjectTypeName,
                                         Id = pr.Id,

                                     }).ToArray();
            return data;
        }

        public async Task SaveBulkDataInDB(InsertOpportunityBulkData input)
        {
            List<Opportunity> importBulk_Items = input.BulkOpportunityItemsData;
            List<Opportunity> dataExsist = new List<Opportunity>();
            dataExsist = _OpportunityRepository.GetAll().ToList();

            var duplicates = importBulk_Items.Join(dataExsist,  
                                            d => d.EmailId,   
                                            e => e.EmailId, 
                                            (d, e) => e)
                      
                                            .Where(contact => dataExsist.Any(existContact =>
                                                    existContact.EmailId == contact.EmailId ||                                        existContact.MobileNumber == contact.MobileNumber))
                                            .ToList();

            var Insertdatalist = importBulk_Items.Where(s => !duplicates.Any(p => p.EmailId == s.EmailId || p.MobileNumber == s.MobileNumber)).ToList();

            try
            {
                // int curId = (int)_abpSession.UserId;
                //foreach (var item in importBulk_Items)
                //{
                //    item.CalllCategoryId = 10;
                //    var importData = item.MapTo<Opportunity>();
                //    await _OpportunityRepository.InsertAsync(importData);
                //}
                foreach(var item in Insertdatalist)
                {
                    item.CalllCategoryId = 10;
                    var importdata = item.MapTo<Opportunity>();
                    await _OpportunityRepository.InsertAsync(importdata);
                }
            }
            catch (Exception ex)
            {
            };
        }

        public async Task ConvertToOpportunity(EntityDto input)
        {
            int curId = (int)_abpSession.UserId;
            var obj = await _OpportunityRepository.GetAsync(input.Id);
            obj.OpportunityOwner = curId;
            obj.AssignUserId = curId;
            await _OpportunityRepository.UpdateAsync(obj);

        }
        public async Task AssignUser(AssignUser input)
        {
            int curId = (int)_abpSession.UserId;
            var obj = await _OpportunityRepository.GetAsync(input.Id);
            obj.AssignUserId = input.AssignUserId;
            //obj.OpportunityOwner = curId;
            await _OpportunityRepository.UpdateAsync(obj);

        }
        public async Task<PagedResultDto<GetOpportunityReportDto>> GetOpportunityClosingReport(GetOpportunityInputDto input)
        {
            var followupdata = _OpportunityFollowUpRepository.GetAll();
            var companydata = _companyRepository.GetAll();
            var frmdate = input.FromDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.FromDate.Value.ToString("MM/dd/yyyy");
            DateTime dtfrm = Convert.ToDateTime(frmdate + " 00:00:00");
            var todate = input.ToDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.ToDate.Value.ToString("MM/dd/yyyy");
            DateTime dt = Convert.ToDateTime(todate + " 23:59:59");
            int curId = (int)_abpSession.UserId;
            bool checkUser = await _userManager.IsInRoleAsync(curId, "DubaiManager");
            var cc = (from p in _OpportunityRepository.GetAll()
                      join u in _userRepository.GetAll()
                      on p.AssignUserId equals u.Id
                      join c in _CallCategoriesRepository.GetAll()
                      on p.CalllCategoryId equals c.Id
                      join io in _InterestedOpportunityRepository.GetAll() on p.Id equals io.Opportunityid
                      join pr in _projecttypeRepository.GetAll() on io.projectypeid equals pr.Id
                      group pr by new { p, c, u } into g
                      select new GetOpportunityReportDto
                      {
                          Id = g.Key.p.Id,
                          CompanyName = g.Key.p.CompanyName,
                          AssignUserId = g.Key.p.AssignUserId,
                          AssignUserName = g.Key.u.Name + " " + g.Key.u.Surname,
                          CalllCategoryId = g.Key.p.CalllCategoryId,
                          CallCategoryName = g.Key.c.Name,
                          Comment = g.Key.p.Comment,
                          PersonName = g.Key.p.PersonName,
                          ProjectValue = g.Key.p.ProjectValue,
                          ProjectType_Name = g.Select(y => y.ProjectTypeName).ToList(),
                          CreateUser = (int)g.Key.p.CreatorUserId,
                          CreateDate = g.Key.p.CreationTime,
                          EmailId = g.Key.p.EmailId,
                          MobileNumber = g.Key.p.MobileNumber,
                          ExpectedClosingDate = g.Key.p.CalllCategoryId == 6 ? followupdata.Where(x => x.opporutnityid == g.Key.p.Id).OrderByDescending(x => x.Id).Select(x => x.expectedclosingdate).FirstOrDefault() : null,
                          Reason = followupdata.Where(x => x.opporutnityid == g.Key.p.Id).OrderByDescending(x => x.Id).Select(x => x.Comment).FirstOrDefault(),
                          ClosedAmount = g.Key.p.ProjectValue,
                          BeneficiaryCompanyId = g.Key.p.BeneficiaryCompanyId,
                          BeneficiaryCompany = companydata.Where(x => x.Id == g.Key.p.BeneficiaryCompanyId).Select(x => x.Beneficial_Company_Name).FirstOrDefault(),
                      })
                      .Where(x=>x.CalllCategoryId == 6)
                      .WhereIf(!input.CompanyName.IsNullOrEmpty(), p => p.CompanyName.ToLower().Contains(input.CompanyName.ToLower()))
                      //.WhereIf(input.CalllCategoryId.HasValue, p => p.CalllCategoryId == input.CalllCategoryId)
                      .WhereIf(input.AssignUserId.HasValue, p => p.AssignUserId == input.AssignUserId)
            // .WhereIf(input.CurrentUser.HasValue, p => p.CreateUser == input.CurrentUser)
                       .WhereIf(input.FromDate.HasValue, p => p.CreateDate >= dtfrm)
                       .WhereIf(input.ToDate.HasValue, p => p.CreateDate <= dt)
                       .WhereIf(input.BeneficiaryCompanyId.HasValue, p => p.BeneficiaryCompanyId == input.BeneficiaryCompanyId);

            if (checkUser)
            {
                int beneid = _companyRepository.GetAll().Where(x => x.Beneficial_Company_Name == "Megh Technologies LLC").Select(x => x.Id).FirstOrDefault();
                cc = cc.Where(x => x.BeneficiaryCompanyId == beneid);
            }

            var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
            var ccCount = cc.Count();
            return new PagedResultDto<GetOpportunityReportDto>(ccCount, ccData.MapTo<List<GetOpportunityReportDto>>());
        }
        public List<GetOpportunityReportDto> GetOpportunityClosingReportExport(GetOpportunityInputDto input)
        {
            var companydata = _companyRepository.GetAll();
            var followupdata = _OpportunityFollowUpRepository.GetAll();
            var frmdate = input.FromDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.FromDate.Value.ToString("MM/dd/yyyy");
            DateTime dtfrm = Convert.ToDateTime(frmdate + " 00:00:00");

            var todate = input.ToDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.ToDate.Value.ToString("MM/dd/yyyy");
            DateTime dt = Convert.ToDateTime(todate + " 23:59:59");
            int curId = (int)_abpSession.UserId;
            var cc = (from p in _OpportunityRepository.GetAll()
                      join u in _userRepository.GetAll()
                      on p.AssignUserId equals u.Id
                      join c in _CallCategoriesRepository.GetAll()
                      on p.CalllCategoryId equals c.Id
                      join io in _InterestedOpportunityRepository.GetAll() on p.Id equals io.Opportunityid
                      join pr in _projecttypeRepository.GetAll() on io.projectypeid equals pr.Id
                      group pr by new { p, c, u } into g
                      select new GetOpportunityReportDto
                      {
                          Id = g.Key.p.Id,
                          CompanyName = g.Key.p.CompanyName,
                          AssignUserId = g.Key.p.AssignUserId,
                          AssignUserName = g.Key.u.Name + " " + g.Key.u.Surname,
                          CalllCategoryId = g.Key.p.CalllCategoryId,
                          CallCategoryName = g.Key.c.Name,
                          Comment = g.Key.p.Comment,
                          PersonName = g.Key.p.PersonName,
                          ProjectType_Name = g.Select(y => y.ProjectTypeName).ToList(),
                          ProjectValue = g.Key.p.ProjectValue,
                          CreateUser = (int)g.Key.p.CreatorUserId,
                          CreateDate = g.Key.p.CreationTime,
                          EmailId = g.Key.p.EmailId,
                          MobileNumber = g.Key.p.MobileNumber,
                          ExpectedClosingDate = g.Key.p.CalllCategoryId == 6 ? followupdata.Where(x => x.opporutnityid == g.Key.p.Id).OrderByDescending(x => x.Id).Select(x => x.expectedclosingdate).FirstOrDefault() : null,
                          Reason = followupdata.Where(x => x.opporutnityid == g.Key.p.Id).OrderByDescending(x => x.Id).Select(x => x.Comment).FirstOrDefault(),
                          ClosedAmount = g.Key.p.ProjectValue,
                          BeneficiaryCompanyId = g.Key.p.BeneficiaryCompanyId,
                          BeneficiaryCompany = companydata.Where(x => x.Id == g.Key.p.BeneficiaryCompanyId).Select(x => x.Beneficial_Company_Name).FirstOrDefault(),
                      })
                      .WhereIf(!input.CompanyName.IsNullOrEmpty(), p => p.CompanyName.ToLower().Contains(input.CompanyName.ToLower()))
                      .WhereIf(input.CalllCategoryId.HasValue, p => p.CalllCategoryId == input.CalllCategoryId)
                      .WhereIf(input.AssignUserId.HasValue, p => p.AssignUserId == input.AssignUserId)
                      // .WhereIf(input.CurrentUser.HasValue, p => p.CreateUser == input.CurrentUser)
                      .WhereIf(input.FromDate.HasValue, p => p.CreateDate >= dtfrm)
                      .WhereIf(input.ToDate.HasValue, p => p.CreateDate <= dt)
                      .WhereIf(input.BeneficiaryCompanyId.HasValue, p => p.BeneficiaryCompanyId == input.BeneficiaryCompanyId).ToList();
            return new List<GetOpportunityReportDto>(cc);
        }
        public bool MobileOrEmailExsistence(GetOpportunityDto input)
        {
            return _OpportunityRepository.GetAll().Where(e => e.MobileNumber == input.MobileNumber || e.EmailId == input.EmailId).Any();
        }
        public bool MobileOrEmailExsistenceById(GetOpportunityDto input)
        {
            return _OpportunityRepository.GetAll().Where(e => (e.MobileNumber == input.MobileNumber || e.EmailId == input.EmailId) && e.Id != input.Id).Any();
        }
    }
}
