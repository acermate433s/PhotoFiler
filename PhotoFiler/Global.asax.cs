#pragma warning disable CA1707 // Identifiers should not contain underscores

using Microsoft.Extensions.Logging;
using Photo.FileSystem;
using Photo.Logged;
using Photo.Models;
using PhotoFiler.Helpers;
using Serilog;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PhotoFiler
{
    public class MvcApplication : System.Web.HttpApplication
    {
        Microsoft.Extensions.Logging.ILogger _Logger = new LoggerFactory()
            .AddSerilog()
            .CreateLogger("PhotoFiler");

        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelMetadataProviders.Current = new InterfaceMetadataProvider();

            using (var scope = _Logger.BeginScope("Application initialization"))
            {
                try
                {
                    var applicationConfiguration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                    var photoFilerConfiguration = (Configuration) applicationConfiguration.GetSection("photoFilerConfiguration");
                    _Logger.LogInformation(photoFilerConfiguration.ToString());

                    IRepository repository = new FileSystemRepository(photoFilerConfiguration);
                    if (photoFilerConfiguration.EnableLogging)
                    {
                        _Logger.LogInformation("Logging enabled.");
                        repository =
                            new LoggedRepository(
                                _Logger,
                                new FileSystemRepository(photoFilerConfiguration)
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
                                var logger = ((Microsoft.Extensions.Logging.ILogger) HttpContext.Current?.Application["Logger"]) ?? _Logger;
                                logger?.LogError(args.Exception, "Error generating preview for photo \"{0}\"", args.Photo.Location);
                            }
                        );

                        if (retriever != null)
                            album = albumRepository.Create(photos);
                    }
                    catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException)
                    {
                        _Logger.LogError(ex, "Cannot create photos from repository.");
                    }

                    if ((album != null) && (photoFilerConfiguration.CreatePreview))
                    {
                        _Logger.LogInformation("Deleting old previews");

                        photoFilerConfiguration
                            .PreviewLocationDirectory
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
                    _Logger.LogCritical(ex, "Application initialization error");
                    throw new HttpUnhandledException("Cannot continue.  Check trace file for explanation.", ex);
                }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (Server != null)
            {
                Exception ex = Server.GetLastError();

                var logger = (Microsoft.Extensions.Logging.ILogger) HttpContext.Current?.Application["Logger"];
                logger?.LogError(ex, "Application error");
            }
        }
    }
}

