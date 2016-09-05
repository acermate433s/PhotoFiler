﻿using PhotoFiler.Helpers;
using PhotoFiler.Helpers.Hasher;
using PhotoFiler.Helpers.Photos.Hashed;
using PhotoFiler.Helpers.Photos.Logged;
using PhotoFiler.Helpers.Repositories;
using PhotoFiler.Helpers.Repositories.Logged;
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

            using (var logger = new ActivityTracerScope(new TraceSource("PhotoFiler")))
            {
                var configuration = new Configuration();
                var loggedRepository = 
                        new LoggedRepository(
                            logger, 
                            new Repository(configuration)
                        );

                IPreviewablePhotos retriever = null;
                IHashedAlbum album = null;

                var photosRepository = loggedRepository.CreatePhotosRepository();
                var albumRepository = loggedRepository.CreateAlbumRepository();

                try
                {
                    retriever = photosRepository.Create();
                    var photos = retriever.Retrieve();

                    if (retriever != null)
                        album = albumRepository.Create(photos);
                }
                catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException)
                {
                    logger.Error(ex);
                }

                if ((album != null ) && (configuration.CreatePreview))
                    album.GeneratePreviews();

                HttpContext.Current.Application["Album"] = album;
            }
        }
    }
}

