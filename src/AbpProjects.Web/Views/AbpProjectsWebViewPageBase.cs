using Abp.Web.Mvc.Views;

namespace AbpProjects.Web.Views
{
    public abstract class AbpProjectsWebViewPageBase : AbpProjectsWebViewPageBase<dynamic>
    {

    }

    public abstract class AbpProjectsWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected AbpProjectsWebViewPageBase()
        {
            LocalizationSourceName = AbpProjectsConsts.LocalizationSourceName;
        }
    }
}