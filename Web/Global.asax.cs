#pragma warning disable CA1707 // Identifiers should not contain underscores

using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Serilog;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using PhotoFiler.Web.Helpers;
using PhotoFiler.Photo.Models;

namespace PhotoFiler.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static void Registration()
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

        private IServiceCollection RegisterServices(IConfigurationRoot configurationRoot, IServiceCollection services = null)
        {
            services = (services ?? new ServiceCollection())
                .AddLogging(configure =>
                {
                    configure.AddSerilog();
                })
                .AddSingleton<ILoggerFactory, LoggerFactory>()
                .AddSingleton(provider => provider.GetService<ILoggerFactory>().CreateLogger("PhotoFiler"))
                .AddPhotoFiler(configurationRoot)                
                .AddControllersAsServices();

            return services;
        }

        protected void Application_Start()
        {
            Registration();

            var configuration = this.Configuration();
            var serviceProvider = RegisterServices(configuration).BuildServiceProvider();
            var resolver = new DefaultDependencyResolver(serviceProvider);
            DependencyResolver.SetResolver(resolver);

            var logger = DependencyResolver.Current.GetService<Microsoft.Extensions.Logging.ILogger>();
            ModelMetadataProviders.Current = new InterfaceMetadataProvider();            

            using (var scope = logger?.BeginScope("Application initialization"))
            {
                try
                {
                    var photoFilerConfiguration = configuration.GetSection("PhotoFiler").Get<PhotoFilerConfiguration>();
                    logger?.LogInformation(configuration.ToString());

                    IHashedAlbum album = DependencyResolver.Current.GetService<IHashedAlbum>();

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
                DependencyResolver.Current.GetService<Microsoft.Extensions.Logging.ILogger>()?.LogError(ex, "Application error");
            }
        }
    }
}

