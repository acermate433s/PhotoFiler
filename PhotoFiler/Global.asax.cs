using PhotoFiler.Helper;
using PhotoFiler.Helpers;
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

            // hack to make model binding work for classes that
            // implements interfaces
            ModelMetadataProviders.Current = new InterfaceMetadataProvider();

            var configuration = new Configuration();
            var previewPath = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            var album =
                new Helpers.MD5HashedAlbum(
                    configuration.RootPath,
                    configuration.HashLength,
                    previewPath
                );
            if (configuration.CreatePreview)
            {
                album.GeneratePreviews();
            }

            HttpContext.Current.Application["Album"] = album;
        }
    }
}