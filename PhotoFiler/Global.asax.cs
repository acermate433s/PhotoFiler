using PhotoFiler.Helpers;
using PhotoFiler.Helpers.Hasher;
using PhotoFiler.Helpers.Photos.Hashed;
using PhotoFiler.Helpers.Photos.Logged;
using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

            if (!Directory.Exists(configuration.RootPath))
                return;

            using (var logger = new ActivityTracerScope(new TraceSource("PhotoFiler")))
            {
                var photos =
                    new LoggedPreviewableHashedPhotos(
                        logger,
                        new PreviewableHashedPhotos(
                            new DirectoryInfo(configuration.RootPath),
                            (file) =>
                            {
                                var photo =
                                    new LoggedPreviewableHashedPhoto(
                                        logger,
                                        new PreviewableHashedPhoto(
                                            configuration.HashLength,
                                            file.FullName,
                                            new SHA512(),
                                            new DirectoryInfo(previewPath)
                                        )
                                    );

                                var result =
                                    new LoggedPreviewableHashedPhoto(
                                        logger,
                                        photo
                                    );

                                return result;
                            }
                        )
                    );

                IHashedAlbum album =
                    new LoggedHashedAlbum(
                        logger,
                        new HashedAlbum(
                            previewPath,
                            photos.Retrieve()
                        )
                    );

                if (configuration.CreatePreview)
                    album.GeneratePreviews();

                HttpContext.Current.Application["Album"] = album;
            }
        }
    }
}

