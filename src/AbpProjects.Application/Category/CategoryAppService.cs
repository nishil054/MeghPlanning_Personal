using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using Abp.Authorization;
using AbpProjects.Category.Dto;
using Microsoft.Build.Framework.XamlTypes;
using AbpProjects.Authorization;
using AbpProjects.Team;

namespace AbpProjects.Category
{
    [AbpAuthorize(PermissionNames.Pages_DataVault, PermissionNames.Pages_DataVault_Category)]
    public class CategoryAppService : ICategoryAppService
    {
        private readonly IRepository<category> _categoryRepository;
        private readonly IRepository<tbl_category_team> _categoryteamRepository;
        private readonly IRepository<team> _teamRepository;
        public CategoryAppService(IRepository<category> categoryRepository, IRepository<tbl_category_team> categoryteamRepository, IRepository<team> teamRepository)
        {
            _categoryRepository = categoryRepository;
            _categoryteamRepository = categoryteamRepository;
            _teamRepository = teamRepository;
        }

        public bool CategoryExsistence(CreateCategoryDto input)
        {

            var items = _categoryRepository.GetAll().Where(e => e.Category == input.Category).Any();
            return items;
        }

        public async Task CreateCategory(CreateCategoryDto input)
        {
            var reasons = input.MapTo<category>();
            await _categoryRepository.InsertAndGetIdAsync((category)reasons);

            //Insert into child table 

            if (input.TeamId != null)
            {
                foreach (var item in input.TeamId)
                {
                    tbl_category_team obj = new tbl_category_team();
                    obj.CategoryId = reasons.Id;
                    int ctId = 0;
                    int.TryParse(item, out ctId);
                    obj.TeamId = ctId;
                    obj.MapTo<tbl_category_team>();
                    _categoryteamRepository.InsertAsync(obj);
                }
            }
        }

        //public async Task<PagedResultDto<CategoryDto>> GetCategory(GetCategoryInput Input)
        //{
        //    var reasons = _categoryRepository.GetAll().WhereIf(
        //       !Input.CategoryFilter.IsNullOrEmpty(),
        //       p => p.Category.ToLower().Contains(Input.CategoryFilter.ToLower()));

        //    var result = reasons
        //        .OrderByDescending(p => p.Id)
        //    .PageBy(Input).ToList();
        //    //.ToList();
        //    var Count = reasons.Count();
        //    return new PagedResultDto<CategoryDto>(Count, result.MapTo<List<CategoryDto>>());

        //}

        public PagedResultDto<CategoryDto> GetCategory(GetCategoryInput Input)
        {
            //var teamlist = _categoryteamRepository.GetAll()
            //    .WhereIf(Input.TeamFilter.HasValue, p => p.TeamId == Input.TeamFilter);

               var Query = (from a in _categoryRepository.GetAll()

                         join b in _categoryteamRepository.GetAll().
                         WhereIf(Input.TeamFilter.HasValue, p => p.TeamId == Input.TeamFilter)
                         on a.Id equals b.CategoryId into t
                         from b in t.DefaultIfEmpty()

                         join c in _teamRepository.GetAll()
                         on b.TeamId equals c.Id into gp
                         from c in gp.DefaultIfEmpty()

                         group new {a.Category,a.Id,c.TeamName}
                         by new { a.Id,a.Category}
                         into g
                         
                         select new CategoryDto
                         {
                             Category = g.Key.Category,
                             Teams = g.Where(x=>x.TeamName!=null).Select(y => y.TeamName).ToList(),
                             Id = g.Key.Id
                         }).ToList();

            var querylist = Query
                //.Where(x => x.Teams.Count() > 0)
                .ToList()
                .WhereIf(!Input.CategoryFilter.IsNullOrEmpty(),p => p.Category.ToLower()
                            .Contains(Input.CategoryFilter.ToLower()));

            var userData = querylist.AsQueryable().OrderBy(Input.Sorting).PageBy(Input).ToList();
            var userCount = querylist.Count();
            return new PagedResultDto<CategoryDto>(userCount, userData.MapTo<List<CategoryDto>>());

        }

        public async Task DeleteCategory(EntityDto input)
        {
            await _categoryRepository.DeleteAsync(input.Id);
        }

        public async Task<CategoryDto> GetCategoryForEdit(EntityDto input)
        {
            var items = (await _categoryRepository.GetAsync(input.Id)).MapTo<CategoryDto>();
            items.TeamId = _categoryteamRepository.GetAll().Where(e => e.CategoryId == items.Id).Select(x => x.TeamId.ToString()).ToArray();
            return items;

        }

        public bool CategoryExsistenceById(EditCategoryDto input)
        {
            var items = _categoryRepository.GetAll().Where(e => e.Category == input.Category && e.Id != input.Id).Any();
            return items;
        }

        public async Task UpdateCategory(EditCategoryDto input)
        {
            var items = await _categoryRepository.GetAsync(input.Id);
            items.Category = input.Category;
            if (input.TeamId != null)
            {
                var deleteitem = _categoryteamRepository.GetAll().Where(x => x.CategoryId == input.Id).ToList();
                foreach (var item in deleteitem)
                {
                    var id = item.Id;
                    await _categoryteamRepository.DeleteAsync(id);
                }
                foreach (var i in input.TeamId)
                {
                    tbl_category_team teams = new tbl_category_team();
                    teams.CategoryId = items.Id;
                    int SkId = 0;
                    int.TryParse(i, out SkId);
                    teams.TeamId = SkId;
                    teams.MapTo<tbl_category_team>();
                    _categoryteamRepository.InsertAsync(teams);
                }

            }
            await _categoryRepository.UpdateAsync(items);
        }

        public async Task<CategoryDto> GetCategoryForDetail(EntityDto input)
        {
            //var items = (await _categoryRepository.GetAsync(input.Id)).MapTo<CategoryDto>();
            //return items;

           var Query = (from a in _categoryRepository.GetAll()

                         join b in _categoryteamRepository.GetAll()
                         on a.Id equals b.CategoryId into t
                         from b in t.DefaultIfEmpty()

                         join c in _teamRepository.GetAll()
                         on b.TeamId equals c.Id into gp
                         from c in gp.DefaultIfEmpty()

                         where a.Id == input.Id
                         group new { a.Category, a.Id, c.TeamName }
                         by new { a.Id, a.Category }
                         into g

                         select new CategoryDto
                         {
                             Category = g.Key.Category,
                             Teams = g.Where(x => x.TeamName != null).Select(y => y.TeamName).ToList(),
                             Id = g.Key.Id
                         }).FirstOrDefault();

           
            return Query;
        }

        public List<CategoryDto> GetCategorys()
        {
            var projectnolists = (from a in _categoryRepository.GetAll()
                                  select new CategoryDto
                                  {
                                      Category = a.Category,
                                      CategoryId = a.Id
                                  }).ToList();

            return projectnolists;
        }

    }
}
