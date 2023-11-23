using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using AbpProjects.NthLevelCategory.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using Abp.AutoMapper;
using Abp.Extensions;
using Abp.Collections.Extensions;
using Abp.Authorization;
using AbpProjects.Authorization;
using Abp.Configuration;

namespace AbpProjects.NthLevelCategory
{
    [AbpAuthorize(PermissionNames.Pages_ManageCategory)]
    public class NthCategoryAppService : AbpProjectsCoreModule, INthCategoryAppService
    {
        private readonly IRepository<CategoryMaster> _repositoryCategoryMaster;
        public NthCategoryAppService(IRepository<CategoryMaster> repositoryCategoryMaster)
        {
            _repositoryCategoryMaster = repositoryCategoryMaster;
        }

        public async Task<int> Create(CategoryCreateDto input, int cid = 0)
        {
            try
            {
                var isExsistName = _repositoryCategoryMaster.GetAll().Where(x => x.Name == input.Name).FirstOrDefault();
                if (isExsistName != null)
                {
                    return 0;
                }
                else
                {
                    var LeftExtent = 0;
                    if (cid > 0)
                    {
                        LeftExtent = _repositoryCategoryMaster.GetAll().Where(x => x.Id == cid).FirstOrDefault().LeftExtent;
                    }
                    var catListRight = _repositoryCategoryMaster.GetAll().Where(x => x.RightExtent > LeftExtent).ToList();
                    var catListLeft = _repositoryCategoryMaster.GetAll().Where(x => x.LeftExtent > LeftExtent).ToList();
                    foreach (var item in catListRight)
                    {
                        item.RightExtent = item.RightExtent + 2;
                        await _repositoryCategoryMaster.UpdateAsync(item);
                    }
                    foreach (var item in catListLeft)
                    {
                        item.LeftExtent = item.LeftExtent + 2;
                        await _repositoryCategoryMaster.UpdateAsync(item);
                    }
                    //var left
                    CategoryMaster obj = new CategoryMaster();
                    obj.Name = input.Name;
                    obj.LeftExtent = LeftExtent + 1;
                    obj.RightExtent = LeftExtent + 2;
                    obj.SortOrder = input.sortOrder;
                    await _repositoryCategoryMaster.InsertAsync(obj);
                    return 1;
                }

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async Task Delete(EntityDto inputs)
        {
            try
            {
                var maxLeftExtent = _repositoryCategoryMaster.GetAll().Min(x => x.LeftExtent);
                var maxRightExtent = _repositoryCategoryMaster.GetAll().Max(x => x.RightExtent);
                int rootNode = _repositoryCategoryMaster.GetAll().Where(x => x.LeftExtent == maxLeftExtent && x.RightExtent == maxRightExtent)
                    .Select(x => x.Id).FirstOrDefault();

                var catItem = _repositoryCategoryMaster.GetAll().Where(x => x.Id == inputs.Id).FirstOrDefault();
                int LeftExtent = catItem.LeftExtent;
                int RIghtExtent = catItem.RightExtent;
                int width = (RIghtExtent - LeftExtent) + 1;

                if (inputs.Id != rootNode)
                {
                    int catLevel = (from c in _repositoryCategoryMaster.GetAll().ToList()
                                    from p in _repositoryCategoryMaster.GetAll().ToList()
                                    where c.Id == inputs.Id && c.LeftExtent >= p.LeftExtent && c.LeftExtent <= p.RightExtent
                                    group p by c into g
                                    select new { cnt = g.Count() }).FirstOrDefault().cnt + 1;

                    var deleteData = (from c in _repositoryCategoryMaster.GetAll().ToList()
                                      from p in _repositoryCategoryMaster.GetAll().ToList()
                                      where c.LeftExtent >= p.LeftExtent && c.LeftExtent <= p.RightExtent && c.Id != inputs.Id && c.LeftExtent >= LeftExtent && c.RightExtent <= RIghtExtent
                                      group p by c into g
                                      where g.Count() == catLevel
                                      select new GetCategoryListDto
                                      {
                                          Id = g.Key.Id,
                                          Name = g.Key.Name,
                                          SortOrder = g.Key.SortOrder
                                      }).ToList();


                    if (deleteData.Count > 0)
                    {
                        foreach (var item in deleteData)
                        {
                            await _repositoryCategoryMaster.DeleteAsync(x => x.Id == item.Id);
                        }

                    }
                }
                else
                {
                    var deleteData = (from c in _repositoryCategoryMaster.GetAll().ToList()
                                      from p in _repositoryCategoryMaster.GetAll().ToList()
                                      where c.LeftExtent >= p.LeftExtent && c.LeftExtent <= p.RightExtent && c.Id != inputs.Id && c.LeftExtent >= LeftExtent && c.RightExtent <= RIghtExtent
                                      group p by c into g
                                      select new GetCategoryListDto
                                      {
                                          Id = g.Key.Id,
                                          Name = g.Key.Name,
                                          SortOrder = g.Key.SortOrder
                                      }).ToList();


                    if (deleteData.Count > 0)
                    {
                        foreach (var item in deleteData)
                        {
                            await _repositoryCategoryMaster.DeleteAsync(x => x.Id == item.Id);
                        }

                    }
                }

                await _repositoryCategoryMaster.DeleteAsync(inputs.Id);

                var catListRight = _repositoryCategoryMaster.GetAll().Where(x => x.RightExtent > RIghtExtent).ToList();
                var catListLeft = _repositoryCategoryMaster.GetAll().Where(x => x.LeftExtent > RIghtExtent).ToList();
                foreach (var item in catListRight)
                {
                    item.RightExtent = item.RightExtent - width;
                    await _repositoryCategoryMaster.UpdateAsync(item);
                }
                foreach (var item in catListLeft)
                {
                    item.LeftExtent = item.LeftExtent - width;
                    await _repositoryCategoryMaster.UpdateAsync(item);
                }

            }
            catch (Exception EX)
            {

                throw new UserFriendlyException();
            }
        }

        public PagedResultDto<GetCategoryListDto> GetCategoriesList(GetMasterInput inputs, int cid = 0)
        {

            //var entitesQuery = (from c in _repositoryCategoryMaster.GetAll()
            //                    select new GetCategoryListDto
            //                    {
            //                        Id = c.Id,
            //                        Name = c.Name,
            //                        SortOrder = c.SortOrder
            //                    })
            //                    .WhereIf(!inputs.Filter.IsNullOrEmpty(),
            //                               j => j.Name.ToLower().Contains(inputs.Filter.ToLower())
            //                               );


            //var entitiesData = entitesQuery.OrderBy(inputs.Sorting).PageBy(inputs).ToList();

            //var entitiesCount = entitesQuery.Count();
            //return new PagedResultDto<GetCategoryListDto>(entitiesCount, entitiesData.MapTo<List<GetCategoryListDto>>());

            var entitiesData = (from c in _repositoryCategoryMaster.GetAll().ToList()
                                from p in _repositoryCategoryMaster.GetAll().ToList()
                                where c.LeftExtent >= p.LeftExtent && c.LeftExtent <= p.RightExtent
                                group p by c into g
                                where g.Count() == 1
                                select new GetCategoryListDto
                                {
                                    Id = g.Key.Id,
                                    Name = g.Key.Name,
                                    SortOrder = g.Key.SortOrder
                                })
                                .WhereIf(!inputs.Filter.IsNullOrEmpty(),
                                           j => j.Name.ToLower().Contains(inputs.Filter.ToLower())
                                           ).AsQueryable();

            var entities = entitiesData.OrderBy(inputs.Sorting).PageBy(inputs).ToList();
            var entitiesCount = entitiesData.Count();
            return new PagedResultDto<GetCategoryListDto>(entitiesCount, entities.MapTo<List<GetCategoryListDto>>());
        }
        public ListResultDto<GetCategoryListDto> GetAllParentByid(int cid = 0)
        {
            var entitiesData = (from c in _repositoryCategoryMaster.GetAll().ToList()
                                from p in _repositoryCategoryMaster.GetAll().ToList()
                                where c.Id == cid && c.LeftExtent >= p.LeftExtent && c.LeftExtent <= p.RightExtent
                                group c by p into g
                                //group p by new
                                //{
                                //    p.Name,
                                //    p.id,
                                //    node.SortOrder
                                //} into g
                                select new GetCategoryListDto
                                {
                                    Id = g.Key.Id,
                                    Name = g.Key.Name
                                });
            return new ListResultDto<GetCategoryListDto>(entitiesData.MapTo<List<GetCategoryListDto>>());
        }
        public ListResultDto<GetCategoryListDto> GetCategoriesListByid(int cid = 0)
        {
            if (cid == 0)
            {
                var entitiesData = (from c in _repositoryCategoryMaster.GetAll().ToList()
                                    from p in _repositoryCategoryMaster.GetAll().ToList()
                                    where c.LeftExtent >= p.LeftExtent && c.LeftExtent <= p.RightExtent
                                    group p by c into g
                                    where g.Count() == 1
                                    select new GetCategoryListDto
                                    {
                                        Id = g.Key.Id,
                                        Name = g.Key.Name,
                                        SortOrder = g.Key.SortOrder
                                    }).OrderBy(x => x.SortOrder);
                return new ListResultDto<GetCategoryListDto>(entitiesData.MapTo<List<GetCategoryListDto>>());
            }
            else
            {
                int LeftExtent = _repositoryCategoryMaster.GetAll().Where(x => x.Id == cid).FirstOrDefault().LeftExtent;
                int RightExtent = _repositoryCategoryMaster.GetAll().Where(x => x.Id == cid).FirstOrDefault().RightExtent;
                int catLevel = (from c in _repositoryCategoryMaster.GetAll().ToList()
                                from p in _repositoryCategoryMaster.GetAll().ToList()
                                where c.Id == cid && c.LeftExtent >= p.LeftExtent && c.LeftExtent <= p.RightExtent
                                group p by c into g
                                select new { cnt = g.Count() }).FirstOrDefault().cnt + 1;

                var entitiesData = (from c in _repositoryCategoryMaster.GetAll().ToList()
                                    from p in _repositoryCategoryMaster.GetAll().ToList()
                                    where c.LeftExtent >= p.LeftExtent && c.LeftExtent <= p.RightExtent && c.Id != cid && c.LeftExtent >= LeftExtent && c.RightExtent <= RightExtent
                                    group p by c into g
                                    where g.Count() == catLevel
                                    select new GetCategoryListDto
                                    {
                                        Id = g.Key.Id,
                                        Name = g.Key.Name,
                                        SortOrder = g.Key.SortOrder
                                    }).OrderBy(x => x.SortOrder);
                return new ListResultDto<GetCategoryListDto>(entitiesData.MapTo<List<GetCategoryListDto>>());

            }
        }

        public async Task<GetCategoryByIdDto> GetCategoryById(EntityDto Id)
        {
            var data = (await _repositoryCategoryMaster.GetAsync(Id.Id)).MapTo<GetCategoryByIdDto>();
            return data;
        }

        public async Task<ListResultDto<GetCategoryDataddlDto>> GetCategoryList()
        {
            try
            {
                var entities = (from u in _repositoryCategoryMaster
                                  .GetAll()
                                select new GetCategoryDataddlDto { Id = u.Id, Name = u.Name }
                                );
                return new ListResultDto<GetCategoryDataddlDto>(entities.MapTo<List<GetCategoryDataddlDto>>());
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException();
            }
        }

        public async Task<int> Update(CategoryEditDto inputs)
        {
            var isExsistName = _repositoryCategoryMaster.GetAll().Where(x => x.Name == inputs.Name && x.Id != inputs.Id).FirstOrDefault();
            if (isExsistName != null)
            {
                return 0;
            }
            else
            {
                var data = _repositoryCategoryMaster.GetAll().Where(x => x.Id == inputs.Id).FirstOrDefault();
                data.Name = inputs.Name;
                data.SortOrder = inputs.SortOrder;
                await _repositoryCategoryMaster.UpdateAsync(data);
                return 1;
            }
        }

        public PagedResultDto<GetCategoryListDto> GetSubCategoriesListByParent(GetMasterInput inputs)
        {
            if (inputs.CategoryId == 0)
            {
                var entitiesData = (from c in _repositoryCategoryMaster.GetAll().ToList()
                                    from p in _repositoryCategoryMaster.GetAll().ToList()
                                    where c.LeftExtent >= p.LeftExtent && c.LeftExtent <= p.RightExtent
                                    group p by c into g
                                    where g.Count() == 1
                                    select new GetCategoryListDto
                                    {
                                        Id = g.Key.Id,
                                        Name = g.Key.Name,
                                        SortOrder = g.Key.SortOrder
                                    })
                                    .WhereIf(!inputs.Filter.IsNullOrEmpty(),
                                           j => j.Name.ToLower().Contains(inputs.Filter.ToLower())
                                           ).AsQueryable();

                var entities = entitiesData.OrderBy(inputs.Sorting).PageBy(inputs).ToList();
                var entitiesCount = entitiesData.Count();
                return new PagedResultDto<GetCategoryListDto>(entitiesCount, entities.MapTo<List<GetCategoryListDto>>());
            }
            else
            {
                int LeftExtent = _repositoryCategoryMaster.GetAll().Where(x => x.Id == inputs.CategoryId).FirstOrDefault().LeftExtent;
                int RightExtent = _repositoryCategoryMaster.GetAll().Where(x => x.Id == inputs.CategoryId).FirstOrDefault().RightExtent;
                int catLevel = (from c in _repositoryCategoryMaster.GetAll().ToList()
                                from p in _repositoryCategoryMaster.GetAll().ToList()
                                where c.Id == inputs.CategoryId && c.LeftExtent >= p.LeftExtent && c.LeftExtent <= p.RightExtent
                                group p by c into g
                                select new { cnt = g.Count() }).FirstOrDefault().cnt + 1;

                var entitiesData = (from c in _repositoryCategoryMaster.GetAll().ToList()
                                    from p in _repositoryCategoryMaster.GetAll().ToList()
                                    where c.LeftExtent >= p.LeftExtent && c.LeftExtent <= p.RightExtent && c.Id != inputs.CategoryId && c.LeftExtent >= LeftExtent && c.RightExtent <= RightExtent
                                    group p by c into g
                                    where g.Count() == catLevel
                                    select new GetCategoryListDto
                                    {
                                        Id = g.Key.Id,
                                        Name = g.Key.Name,
                                        SortOrder = g.Key.SortOrder
                                    })
                                    .WhereIf(!inputs.Filter.IsNullOrEmpty(),
                                           j => j.Name.ToLower().Contains(inputs.Filter.ToLower())
                                           ).AsQueryable();

                var entities = entitiesData.OrderBy(inputs.Sorting).PageBy(inputs).ToList();
                var entitiesCount = entitiesData.Count();
                return new PagedResultDto<GetCategoryListDto>(entitiesCount, entities.MapTo<List<GetCategoryListDto>>());

            }
        }

    }
}


