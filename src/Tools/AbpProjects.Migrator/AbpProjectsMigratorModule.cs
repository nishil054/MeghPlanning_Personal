using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using AbpProjects.EntityFramework;

namespace AbpProjects.Migrator
{
    [DependsOn(typeof(AbpProjectsDataModule))]
    public class AbpProjectsMigratorModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer<AbpProjectsDbContext>(null);

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}