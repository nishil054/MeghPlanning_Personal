using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace AbpProjects.EntityFramework.Repositories
{
    public abstract class AbpProjectsRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<AbpProjectsDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected AbpProjectsRepositoryBase(IDbContextProvider<AbpProjectsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class AbpProjectsRepositoryBase<TEntity> : AbpProjectsRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected AbpProjectsRepositoryBase(IDbContextProvider<AbpProjectsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
