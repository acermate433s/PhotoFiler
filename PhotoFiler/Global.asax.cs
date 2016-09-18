using PhotoFiler.Helpers;
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
        ILogger _Logger = new ActivityTracerScope(new TraceSource("PhotoFiler"));

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelMetadataProviders.Current = new InterfaceMetadataProvider();

            using (var logger = _Logger.Create("Application_Start"))
            {
                try
                {
                    var configuration = new Configuration();
                    logger.Verbose(configuration.ToString());

                    IRepository repository = new Repository(configuration);
                    if (configuration.EnableLogging)
                    {
                        logger.Information("Logging enabled.");
                        repository =
                            new LoggedRepository(
                                _Logger,
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
                        var photos = retriever.Retrieve();

                        if (retriever != null)
                            album = albumRepository.Create(photos);
                    }
                    catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException)
                    {
                        logger.Error(ex);
                    }

                    if ((album != null) && (configuration.CreatePreview))
                    {
                        logger.Information("Deleting old previews");
                        configuration
                            .PreviewLocation
                            .GetFiles()
                            .ToList()
                            .ForEach(item => item.Delete());

                        album.GeneratePreviews();
                    }

                    HttpContext.Current.Application["Album"] = album;
                    HttpContext.Current.Application["Logger"] = _Logger;
                }
                catch (Exception ex)
                {
                    logger.Critical(ex);
                    throw new HttpUnhandledException("Cannot continue.  Check trace file for explanation.", ex);
                }
            }          
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (Server != null)
            {
                Exception ex = Server.GetLastError();
                _Logger.Error(ex);
            }
        }
    }
}

