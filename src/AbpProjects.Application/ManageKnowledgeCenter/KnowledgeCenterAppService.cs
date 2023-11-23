using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using AbpProjects.ManageKnowledgeCenter.Dto;
using AbpProjects.Team;
using System;
using System.Collections.Generic;
using Abp.Linq.Extensions;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Abp.Authorization;
using AbpProjects.Authorization;
using System.Web;
using AbpProjects.Category.Dto;
using AbpProjects.Category;
using System.Linq;
using Abp.Runtime.Session;
using AbpProjects.Authorization.Users;
using Abp.Authorization.Users;
using AbpProjects.Authorization.Roles;

namespace AbpProjects.ManageKnowledgeCenter
{
    [AbpAuthorize(PermissionNames.Pages_KnowledgeCenter, PermissionNames.Pages_KnowledgeCenterList, PermissionNames.Pages_KnowledgeCenterCrud)]
    public class KnowledgeCenterAppService : IKnowledgeCenterAppService
    {
        private readonly IRepository<team> _teamRepository;
        private readonly IRepository<manageKnowledgeCenter> _knowledgecenterRepository;
        private readonly IRepository<KnowledgeDocuments> _knowledgedocumentRepository;
        private readonly IRepository<category> _categoryRepository;
        private readonly IAbpSession _abpSession;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;

        public KnowledgeCenterAppService(IRepository<team> teamRepository,
            IRepository<manageKnowledgeCenter> knowledgecenterRepository,
            IRepository<KnowledgeDocuments> knowledgedocumentRepository,
            IRepository<category> categoryRepository, IAbpSession abpSession,
            IRepository<User, long> userRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository)
        {
            _teamRepository = teamRepository;
            _knowledgecenterRepository = knowledgecenterRepository;
            _knowledgedocumentRepository = knowledgedocumentRepository;
            _categoryRepository = categoryRepository;
            _abpSession = abpSession;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
        }
        #region Manage Knowledge Center

        public List<GetTeamListDto> GetTeams()
        {
            int UserId = (int)_abpSession.UserId;
            var TeamId = _userRepository.GetAll().Where(x => x.Id == UserId).Select(x => x.TeamId).FirstOrDefault();
            var RoleList = _userRoleRepository.GetAll().Where(x => x.UserId == UserId).Select(x => x.RoleId).ToList();
            var Role = _roleRepository.GetAll().Where(x => RoleList.Contains(x.Id)).Select(x => x.Name).ToList();

            List<GetTeamListDto> Datalist = new List<GetTeamListDto>();

            if (Role.Contains("Admin"))
            {
                Datalist = (from a in _teamRepository.GetAll()
                                orderby a.TeamName ascending
                                select new GetTeamListDto
                                {
                                    Id = a.Id,
                                    TeamName = a.TeamName,
                                }).OrderBy(x=>x.TeamName).ToList();
            }
            else
            {
                Datalist = (from a in _teamRepository.GetAll()
                            where a.Id == TeamId
                            orderby a.TeamName ascending
                            select new GetTeamListDto
                            {
                                Id = a.Id,
                                TeamName = a.TeamName,
                            }).OrderBy(x => x.TeamName).ToList();
            }

            return Datalist;

        }
        public async Task<int> CreateKnowledgeCenter(CreateKnowledgeCenterDto input)
        {
            var ongoings = input.MapTo<manageKnowledgeCenter>();
            var KnowledgeCenterId = await _knowledgecenterRepository.InsertAndGetIdAsync(ongoings);

            return KnowledgeCenterId;

            // Old Code
            //if (input.Attachments != null && input.Attachmentpath != null && input.NewAttachments != null
            //    && input.Attachments.Count() != 0 && input.Attachmentpath.Count() != 0 && input.NewAttachments.Count() != 0)
            //{
            //    for (int i = 0; i < input.Attachments.Count(); i++)
            //    {
            //        for (int j = 0; j < input.Attachmentpath.Count(); j++)
            //        {
            //            for (int k = 0; k < input.NewAttachments.Count(); k++)
            //            {
            //                if (i == j && i == k && j == k)
            //                {
            //                    KnowledgeDocuments doc = new KnowledgeDocuments();
            //                    doc.FileName = input.Attachments[i];
            //                    doc.FilePath = input.Attachmentpath[j];
            //                    doc.DocumentName = input.NewAttachments[k];
            //                    doc.KnowledgeCenterId = KnowledgeCenterId;

            //                    doc.MapTo<KnowledgeDocuments>();
            //                    await _knowledgedocumentRepository.InsertAsync(doc);
            //                }
            //            }
            //        }
            //    }
            //}
        }

        public bool KnowledgeCenterExsistence(CreateKnowledgeCenterDto input)
        {
            var items = _knowledgecenterRepository.GetAll().Where(e => e.Title == input.Title).Any();
            return items;
        }

        //public async Task<PagedResultDto<GetKnowledgeCenterDto>> getKnowledgeCenter1(GetKnowledgeCenterInput Input)
        //{
        //    var knowledgecenterlist = _knowledgecenterRepository.GetAll().WhereIf(
        //       !Input.Filter.IsNullOrEmpty(),
        //       p => p.Title.ToLower().Contains(Input.Filter.ToLower()))
        //        .WhereIf((Input.TeamId != 0 && Input.TeamId != null),
        //                          p => p.TeamId == Input.TeamId);

        //    var knowledgedocs = _knowledgedocumentRepository.GetAll().AsEnumerable();
        //    var result = (from og in knowledgecenterlist
        //                  join puser in _teamRepository.GetAll() on og.TeamId equals puser.Id into puserJoined
        //                  from puser in puserJoined
        //                  join category in _categoryRepository.GetAll() on og.CategoryId equals category.Id into categoryJoined
        //                  from category in categoryJoined
        //                      //join clt in _knowledgedocumentRepository.GetAll() on og.Id equals clt.KnowledgeCenterId into cltjoined
        //                      //from clt in cltjoined
        //                  let KnowledgeCenterDocuments = (from a in knowledgedocs
        //                                                  where a.KnowledgeCenterId == og.Id
        //                                                  select new KnowledgeDocumentsView
        //                                                  {
        //                                                      Id = a.Id,
        //                                                      KnowledgeCenterId = a.KnowledgeCenterId,
        //                                                      FileName = a.FileName,
        //                                                      FilePath = a.FilePath,
        //                                                      DocumentName = a.DocumentName,
        //                                                  }).ToList()
        //                  select new GetKnowledgeCenterDto
        //                  {
        //                      Id = og.Id,
        //                      Title = og.Title,
        //                      IsDocument = og.IsDocument,
        //                      Url = og.Url,
        //                      Comment = og.Comment,
        //                      TeamId = og.TeamId,
        //                      Team = puser.TeamName,
        //                      CategoryId = og.CategoryId,
        //                      Category = category.Category,
        //                      KnowledgeDocuments = KnowledgeCenterDocuments
        //                  }).OrderByDescending(p => p.Id)
        //    .PageBy(Input).ToList();

        //    //var result = knowledgecenterlist
        //    //    .OrderByDescending(p => p.Id)
        //    //.PageBy(Input).ToList();

        //    var Count = knowledgecenterlist.Count();
        //    return new PagedResultDto<GetKnowledgeCenterDto>(Count, result.MapTo<List<GetKnowledgeCenterDto>>());

        //}

        public PagedResultDto<GetKnowledgeCenterDto> getKnowledgeCenter(GetKnowledgeCenterInput Input)
       {
            var knowledgecenterlist = _knowledgecenterRepository.GetAll().WhereIf(!Input.Filter.IsNullOrEmpty(), p => p.Title.ToLower().Contains(Input.Filter.ToLower()))
                .WhereIf((Input.TeamId != 0 && Input.TeamId != null),
                                  p => p.TeamId == Input.TeamId)
                .WhereIf((Input.CategoryId != 0 && Input.CategoryId != null),
                                  p => p.CategoryId == Input.CategoryId);
            List<GetKnowledgeCenterDto> userData = new List<GetKnowledgeCenterDto>();

            var knowledgedocs = _knowledgedocumentRepository.GetAll().AsEnumerable();
            var result = (from og in knowledgecenterlist
                          join puser in _teamRepository.GetAll() on og.TeamId equals puser.Id into puserJoined
                          from puser in puserJoined
                          join category in _categoryRepository.GetAll() on og.CategoryId equals category.Id into categoryJoined
                          from category in categoryJoined
                              ////join clt in _knowledgedocumentRepository.GetAll() on og.Id equals clt.KnowledgeCenterId into cltjoined
                              ////from clt in cltjoined
                          let KnowledgeCenterDocuments = (from a in knowledgedocs
                                                          where a.KnowledgeCenterId == og.Id
                                                          select new KnowledgeDocumentsView
                                                          {
                                                              Id = a.Id,
                                                              KnowledgeCenterId = a.KnowledgeCenterId,
                                                              FileName = a.FileName,
                                                              FilePath = a.FilePath,
                                                              DocumentName = a.DocumentName,
                                                          }).ToList()
                          select new GetKnowledgeCenterDto
                          {
                              Id = og.Id,
                              Title = og.Title,
                              IsDocument = og.IsDocument,
                              Url = og.Url,
                              Comment = og.Comment,
                              TeamId = og.TeamId,
                              Team = puser.TeamName,
                              CategoryId = og.CategoryId,
                              Category = category.Category,
                              KnowledgeDocuments = KnowledgeCenterDocuments
                          });
            if (Input.Sorting == "Team asc")
            {
                userData = result.OrderBy(x => x.Team).PageBy(Input).ToList();
            }
            if (Input.Sorting == "Team desc")
            {
                userData = result.OrderByDescending(x => x.Team).PageBy(Input).ToList();
            }
            if (Input.Sorting == "Title asc")
            {
                userData = result.OrderBy(x => x.Title).PageBy(Input).ToList();
            }
            if (Input.Sorting == "Title desc")
            {
                userData = result.OrderByDescending(x => x.Title).PageBy(Input).ToList();
            }
            if (Input.Sorting == "Category asc")
            {
                userData = result.OrderBy(x => x.Category).PageBy(Input).ToList();
            }
            if (Input.Sorting == "Category desc")
            {
                userData = result.OrderByDescending(x => x.Category).PageBy(Input).ToList();
            }
            if (Input.Sorting == "Id desc")
            {
                userData = result.OrderByDescending(x => x.Id).PageBy(Input).ToList();
            }

            var userCount = result.Count();
            return new PagedResultDto<GetKnowledgeCenterDto>(userCount, userData.MapTo<List<GetKnowledgeCenterDto>>());

        }

        public async Task<GetKnowledgeCenterDto> GetknowledgeCenterForEdit(EntityDto input)
        {
            var items = (await _knowledgecenterRepository.GetAsync(input.Id)).MapTo<GetKnowledgeCenterDto>();
            return items;

        }

        public ListResultDto<GetKnowledgeDocumentsDto> getknowledgeCenterDocuments(EntityDto input)
        {
            try
            {
                //var Result = new List<ProjectDocumentsDto>();
                var ActivityLog = _knowledgedocumentRepository.GetAll()
                .WhereIf(input.Id != 0, p => p.KnowledgeCenterId == input.Id);
                //Result = ActivityLog.MapTo<List<ProjectDocumentsDto>>();

                var Result = (from result in ActivityLog

                                  //join ur in _userRepository.GetAll() on result.CreatorUserId equals ur.Id into urJoined
                                  //from ur in urJoined.DefaultIfEmpty()

                              select new GetKnowledgeDocumentsDto
                              {
                                  Id = result.Id,
                                  FileName = result.FileName,
                                  FilePath = result.FilePath,
                                  KnowledgeCenterId = result.KnowledgeCenterId,
                                  DocumentName = result.DocumentName

                              }).ToList();

                return new ListResultDto<GetKnowledgeDocumentsDto>(Result.MapTo<List<GetKnowledgeDocumentsDto>>());
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async Task DeleteknowledgeCenterFiles(EntityDto input)
        {
            var FilePath = _knowledgedocumentRepository.GetAll().Where(x => x.Id == input.Id).Select(x => x.FilePath).FirstOrDefault();
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
            await _knowledgedocumentRepository.DeleteAsync(input.Id);
        }

        public bool KnowledgeCenterExsistenceById(EditKnowledgeCenterDto input)
        {
            var items = _knowledgecenterRepository.GetAll().Where(e => e.Title == input.Title && e.Id != input.Id).Any();
            return items;
        }

        public async Task UpdateKnowledgeCenter(EditKnowledgeCenterDto input)
        {
            var items = await _knowledgecenterRepository.GetAsync(input.Id);
            items.TeamId = input.TeamId;
            items.CategoryId = input.CategoryId;
            items.Title = input.Title;
            items.IsDocument = input.IsDocument;
            items.Url = input.Url;
            if (input.IsDocument == 0)
            {
                var deleteitem = _knowledgedocumentRepository.GetAll().Where(x => x.KnowledgeCenterId == input.Id).ToList();
                foreach (var item in deleteitem)
                {
                    var id = item.Id;
                    await _knowledgedocumentRepository.DeleteAsync(id);

                    var filePath = _knowledgedocumentRepository.GetAll().Where(x => x.Id == id).Select(x => x.FilePath).FirstOrDefault();
                    //string filePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "userfiles\\KnowledgeCenter\\" + FilePath;
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);

                    }
                    await _knowledgedocumentRepository.DeleteAsync(input.Id);
                }


            }
            items.Comment = input.Comment;
            await _knowledgecenterRepository.UpdateAsync(items);

            //Old Code
            //if (input.Attachments != null && input.Attachmentpath != null && input.NewAttachments != null
            //&& input.Attachments.Count() != 0 && input.Attachmentpath.Count() != 0 && input.NewAttachments.Count() != 0)
            //{
            //    var deleteitem = _knowledgedocumentRepository.GetAll().Where(x => x.KnowledgeCenterId == input.Id).ToList();
            //    foreach (var item in deleteitem)
            //    {
            //        var id = item.Id;
            //        await _knowledgedocumentRepository.DeleteAsync(id);

            //        var filePath = _knowledgedocumentRepository.GetAll().Where(x => x.Id == id).Select(x => x.FilePath).FirstOrDefault();
            //        //string filePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "userfiles\\KnowledgeCenter\\" + FilePath;
            //        if (File.Exists(filePath))
            //        {
            //            File.Delete(filePath);
            //        }
            //        await _knowledgedocumentRepository.DeleteAsync(input.Id);
            //    }

            //    for (int i = 0; i < input.Attachments.Count(); i++)
            //    {
            //        for (int j = 0; j < input.Attachmentpath.Count(); j++)
            //        {
            //            for (int k = 0; k < input.NewAttachments.Count(); k++)
            //            {
            //                if (i == j && i == k && j == k)
            //                {
            //                    KnowledgeDocuments doc = new KnowledgeDocuments();
            //                    doc.FileName = input.Attachments[i];
            //                    doc.FilePath = input.Attachmentpath[j];
            //                    doc.DocumentName = input.NewAttachments[k];
            //                    doc.KnowledgeCenterId = input.Id;

            //                    doc.MapTo<KnowledgeDocuments>();
            //                    await _knowledgedocumentRepository.InsertAsync(doc);
            //                }
            //            }
            //        }
            //    }
            //}
        }

        public async Task<GetKnowledgeCenterDto> GetknowledgeCenterForDetail(EntityDto input)
        {
            var items = (await _knowledgecenterRepository.GetAsync(input.Id)).MapTo<GetKnowledgeCenterDto>();
            items.Team = _teamRepository.GetAll().Where(x => x.Id == items.TeamId).Select(x => x.TeamName).FirstOrDefault();
            return items;

        }
        public async Task DeleteKnowledgeCenter(EntityDto input)
        {
            await _knowledgecenterRepository.DeleteAsync(input.Id);
        }

        public List<CategoryDto> GetCategories()
        {
            var Datalist = (from a in _categoryRepository.GetAll()
                            orderby a.Category ascending
                            select new CategoryDto
                            {
                                Id = a.Id,
                                Category = a.Category,
                            }).ToList();

            return Datalist;

        }

        public async Task<GetKnowledgeDocumentsDto> GetknowledgeDocuments(EntityDto input)
        {
            var items = (await _knowledgedocumentRepository.GetAsync(input.Id)).MapTo<GetKnowledgeDocumentsDto>();
            
            return items;
        }

        #endregion
    }
}
