#pragma warning disable CA1707 // Identifiers should not contain underscores

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PhotoFiler.Web.Helpers;
using Serilog;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using PhotoFiler.Photo.Models;
using PhotoFiler.Photo.Logged;
using PhotoFiler.Photo.FileSystem;

namespace PhotoFiler.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private void Registration()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private IConfigurationRoot Configuration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(HttpRuntime.AppDomainAppPath)
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }

        private IServiceCollection RegisterServices(IConfigurationRoot configurationRoot, IServiceCollection services)
        {
            services = (services ?? new ServiceCollection())
                .AddLogging(configure =>
                {
                    configure.AddSerilog();
                })
                .AddSingleton<ILoggerFactory, LoggerFactory>()
                .AddSingleton<Microsoft.Extensions.Logging.ILogger>(new LoggerFactory().CreateLogger("PhotoFiler"))
                .AddSingleton<IRepository>(factory => {

                    var configuration = configurationRoot.GetSection("PhotoFiler").Get<PhotoFilerConfiguration>();
                    IRepository repository = new FileSystemRepository(configuration);

                    if (configuration.EnableLogging)
                    {
                        repository =
                            new LoggedRepository(
                                factory.GetService<Microsoft.Extensions.Logging.ILogger>(),
                                repository
                            );
                    }

                    return repository;
                })
                .AddSingleton<IHashedAlbum>(factory =>
                {
                    var repository = factory.GetService<IRepository>();
                    var photosRepository = repository.CreatePhotosRepository();
                    var albumRepository = repository.CreateAlbumRepository();
                    var retriever = photosRepository.Create();

                    var photos = retriever.Retrieve(
                            (sender, args) =>
                            {
                                factory.GetService<Microsoft.Extensions.Logging.ILogger>()?
                                    .LogError(args.Exception, "Error generating preview for photo \"{0}\"", args.Photo.Location);
                            }
                        );

                    return albumRepository.Create(photos);
                });

            return services;
        }

        protected void Application_Start()
        {
            this.Registration();

            var configuration = this.Configuration();
            Bootstrapper.ServiceProvider = RegisterServices(configuration, new ServiceCollection()).BuildServiceProvider();
            var logger = Bootstrapper.ServiceProvider.GetService<Microsoft.Extensions.Logging.ILogger>();
            ModelMetadataProviders.Current = new InterfaceMetadataProvider();

            using (var scope = logger.BeginScope("Application initialization"))
            {
                try
                {
                    var photoFilerConfiguration = configuration.GetSection("PhotoFiler").Get<PhotoFilerConfiguration>();
                    logger?.LogInformation(configuration.ToString());

                    IHashedAlbum album = Bootstrapper.ServiceProvider.GetService<IHashedAlbum>();

                   try
                    {
                        if ((album != null) && (album.Count() == 0) && (photoFilerConfiguration.CreatePreview))
                        {
                            logger.LogInformation("Deleting old previews");

                            photoFilerConfiguration
                                .PreviewLocationDirectory
                                .GetFiles()
                                .ToList()
                                .ForEach(item => item.Delete());

                            album.GeneratePreviews();
                        }
                    }
                    catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException)
                    {
                        logger.LogError(ex, "Cannot create photos from repository.");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogCritical(ex, "Application initialization error");
                    throw new HttpUnhandledException("Cannot continue.  Check trace file for explanation.", ex);
                }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (Server != null)
            {
                Exception ex = Server.GetLastError();
                Bootstrapper.ServiceProvider.GetService<Microsoft.Extensions.Logging.ILogger>()?.LogError(ex, "Application error");
            }
        }
    }
}

