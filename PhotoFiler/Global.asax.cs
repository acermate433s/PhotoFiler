using PhotoFiler.Helpers;
using PhotoFiler.Helpers.Hashed;
using PhotoFiler.Helpers.MD5;
using PhotoFiler.Helpers.Photos.Logged;
using PhotoFiler.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Telemetry;

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

            using (var logger = new ActivityTracerScope(new TraceSource("PhotoFiler")))
            {
                IHashedAlbum album =
                    new LoggedHashedAlbum(
                        logger,
                        new HashedAlbum(
                            previewPath,
                            new LoggedHashedPhotos(
                                logger,
                                new HashedPhotos(
                                    configuration.RootPath,
                                    configuration.HashLength,
                                    (filename) =>
                                    {
                                        return
                                            new LoggedHashedPhoto(
                                                logger,
                                                new HashedPhoto(
                                                    configuration.HashLength,
                                                    filename,
                                                    new MD5Hasher()
                                                )
                                            );
                                    }
                                )
                            ),
                            (photo, directory) =>
                            {
                                return 
                                    new LoggedHashedPhotoPreviewer(
                                        logger, 
                                        new HashedPhotoPreviewer(
                                            photo, 
                                            directory
                                        )
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
}

