using PhotoFiler.Helper;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PhotoFiler
{
    public class MvcApplication : System.Web.HttpApplication
    {
        const string ROOT_PATH = @"\\ARCHIVE\Volume_1\Media\Videos";
        const int MAX_HASH_LENGTH = 5;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            HttpContext.Current.Application["FileHashes"] = new FileInfoHasher(ROOT_PATH, MAX_HASH_LENGTH);
        }
    }
}
