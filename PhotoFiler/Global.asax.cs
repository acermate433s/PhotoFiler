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
                IHashedAlbum album =
                    new LoggedHashedAlbum(
                        logger,
                        new HashedAlbum(
                            previewPath,
                            GetPhotoFiles(new DirectoryInfo(configuration.RootPath))
                                .Select(photo =>

                                    (IPreviewableHashedPhoto)
                                        (new LoggedPreviewableHashedPhoto(
                                            logger,
                                            new PreviewableHashedPhoto(
                                                configuration.HashLength,
                                                photo.FullName,
                                                new SHA512(),
                                                new DirectoryInfo(previewPath)
                                            )
                                        ))

                                )
                                .ToList()
                        )
                    );
                if (configuration.CreatePreview)
                {
                    album.GeneratePreviews();
                }

                HttpContext.Current.Application["Album"] = album;
            }
        }

        private List<FileInfo> GetPhotoFiles(DirectoryInfo root)
        {
            var value = new List<FileInfo>();

            // add files in the current directory
            value
                .AddRange(
                    root
                        .EnumerateFiles()
                        .Where(file => (new[] { ".jpg", ".png" }).Contains(file.Extension.ToLower()))
                        .Cast<FileInfo>()
                );

            // iterate all directories and add files in that directory
            value
                .AddRange(
                    root
                        .EnumerateDirectories()
                        .SelectMany(directory => GetPhotoFiles(directory)
                    )
                );

            return value;
        }
    }
}

