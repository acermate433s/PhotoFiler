using PhotoFiler.Helpers;
using PhotoFiler.Helpers.Hashed;
using PhotoFiler.Helpers.MD5;
using PhotoFiler.Models;
using System;
using System.IO;
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

            // hack to make model binding work for models that implements interfaces
            ModelMetadataProviders.Current = new InterfaceMetadataProvider();

            var configuration = new Configuration();
            var previewPath = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            IHashedAlbum album =
                new HashedAlbum(
                    previewPath,
                    new HashedPhotos(
                        configuration.RootPath, 
                        configuration.HashLength, 
                        (filename) => {
                            return new HashedPhoto(
                                configuration.HashLength, 
                                filename, 
                                new MD5Hasher()
                            );
                        }
                    )
                );
            if (configuration.CreatePreview)
            {
                album.GeneratePreviews();
            }

            HttpContext.Current.Application["Album"] = album;
        }
    }
}

