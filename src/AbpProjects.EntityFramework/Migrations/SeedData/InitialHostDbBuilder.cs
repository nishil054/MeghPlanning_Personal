using Abp.Runtime.Session;
using AbpProjects.EntityFramework;
using EntityFramework.DynamicFilters;

namespace AbpProjects.Migrations.SeedData
{
    public class InitialHostDbBuilder
    {
        private readonly AbpProjectsDbContext _context;
        public InitialHostDbBuilder(AbpProjectsDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            _context.DisableAllFilters();

            new DefaultEditionsCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
        }
    }
}
