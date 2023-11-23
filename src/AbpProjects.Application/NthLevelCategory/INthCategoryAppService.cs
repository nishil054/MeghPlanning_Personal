using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.NthLevelCategory.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.NthLevelCategory
{
    public interface INthCategoryAppService : IApplicationService
    {
        PagedResultDto<GetCategoryListDto> GetCategoriesList(GetMasterInput inputs, int cid = 0);
        Task<int> Create(CategoryCreateDto input, int cid = 0);
        Task<GetCategoryByIdDto> GetCategoryById(EntityDto Id);
        Task<int> Update(CategoryEditDto inputs);
        Task Delete(EntityDto inputs);
        Task<ListResultDto<GetCategoryDataddlDto>> GetCategoryList();
        ListResultDto<GetCategoryListDto> GetCategoriesListByid(int cid = 0);
        ListResultDto<GetCategoryListDto> GetAllParentByid(int cid = 0);
        PagedResultDto<GetCategoryListDto> GetSubCategoriesListByParent(GetMasterInput inputs);

    }
}
