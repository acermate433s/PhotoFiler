#pragma warning disable CA1707 // Identifiers should not contain underscores

using Photo.FileSystem;
using Photo.Logged;
using Photo.Models;
using PhotoFiler.Helpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Telemetry;
using Telemetry.TraceSource;

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

                    IRepository repository = new FileSystemRepository(configuration);
                    if (configuration.EnableLogging)
                    {
                        scope.Information("Logging enabled.");
                        repository =
                            new LoggedRepository(
                                new ActivityTracerScope(new TraceSource("PhotoFiler"), "Logger"),
                                new FileSystemRepository(configuration)
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
                            (sender, args) =>
                            {
                                var logger = ((ILogger) HttpContext.Current?.Application["Logger"]) ?? scope;
                                logger?.Error(args.Exception, "Error generating preview for photo \"{0}\"", args.Photo.Location);
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

        public sealed override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_Logger != null)
                {
                    _Logger.Dispose();
                    _Logger = null;
                }
            }

            base.Dispose();
        }
    }
}

