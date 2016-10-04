﻿using PhotoFiler.Helpers;
using PhotoFiler.Helpers.Repositories;
using PhotoFiler.Helpers.Repositories.Logged;
using PhotoFiler.Models;
using System;
using System.Diagnostics;
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
        ILogger _Logger = new ActivityTracerScope("PhotoFiler", "Global Logger");

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelMetadataProviders.Current = new InterfaceMetadataProvider();

            using (var scope = _Logger.CreateScope("Application initialization"))
            {
                try
                {
                    var configuration = new Configuration();
                    scope.Verbose(configuration.ToString());

                    IRepository repository = new Repository(configuration);
                    if (configuration.EnableLogging)
                    {
                        scope.Information("Logging enabled.");
                        repository =
                            new LoggedRepository(
                                new ActivityTracerScope(new TraceSource("PhotoFiler"), "Logger"),
                                new Repository(configuration)
                            );
                    }

                    IPreviewablePhotos retriever = null;
                    IHashedAlbum album = null;

                    var photosRepository = repository.CreatePhotosRepository();
                    var albumRepository = repository.CreateAlbumRepository();

                    try
                    {
                        retriever = photosRepository.Create();
                        var photos = retriever.Retrieve(
                            (p, e) =>
                            {
                                var logger = ((ILogger) HttpContext.Current?.Application["Logger"]) ?? scope;
                                logger?.Error(e, "Error generating preview for photo \"{0}\"", p.FileInfo.FullName);
                            }
                        );

                        if (retriever != null)
                            album = albumRepository.Create(photos);
                    }
                    catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException)
                    {
                        scope.Error(ex);
                    }

                    if ((album != null) && (configuration.CreatePreview))
                    {
                        scope.Information("Deleting old previews");
                        configuration
                            .PreviewLocation
                            .GetFiles()
                            .ToList()
                            .ForEach(item => item.Delete());

                        album.GeneratePreviews();
                    }

                    var applicationState = HttpContext.Current.Application;
                    applicationState.Lock();
                    applicationState["Album"] = album;
                    applicationState["Logger"] = _Logger;
                    applicationState.UnLock();
                }
                catch (Exception ex)
                {
                    _Logger.Critical(ex);
                    throw new HttpUnhandledException("Cannot continue.  Check trace file for explanation.", ex);
                }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (Server != null)
            {
                Exception ex = Server.GetLastError();

                var logger = (ILogger) HttpContext.Current?.Application["Logger"];
                logger?.Error(ex);
            }
        }

        protected void Dispose(bool disposing)
        {
            if(disposing)
                _Logger.Dispose();

            base.Dispose();
        }
    }
}

