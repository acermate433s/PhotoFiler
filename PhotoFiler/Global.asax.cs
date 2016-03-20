using PhotoFiler.Helper;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PhotoFiler
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var configuration = new Configuration();
            var previewPath = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            var hasher =
                new PhotoHasher(
                    configuration.RootPath,
                    configuration.HashLength,
                    previewPath
                );
            hasher.CreatePreviews();

            HttpContext.Current.Application["FileHashes"] = hasher;
        }
    }
}
