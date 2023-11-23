using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.Category.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Category
{
    public interface ICategoryAppService : IApplicationService
    {
        bool CategoryExsistence(CreateCategoryDto input);
        Task CreateCategory(CreateCategoryDto input);
        //Task<PagedResultDto<CategoryDto>> GetCategory(GetCategoryInput Input);
        PagedResultDto<CategoryDto> GetCategory(GetCategoryInput Input);
        //Task<PagedResultDto<CategoryDto>> GetCategory(GetCategoryInput Input);
        Task DeleteCategory(EntityDto input);
        Task<CategoryDto> GetCategoryForEdit(EntityDto input);
        bool CategoryExsistenceById(EditCategoryDto input);
        Task UpdateCategory(EditCategoryDto input);
        Task<CategoryDto> GetCategoryForDetail(EntityDto input);
        List<CategoryDto> GetCategorys();
    }
}
