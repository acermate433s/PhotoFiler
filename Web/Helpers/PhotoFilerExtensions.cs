using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using PhotoFiler.Photo;
using PhotoFiler.Photo.FileSystem;
using PhotoFiler.Photo.Logged;
using PhotoFiler.Photo.Models;

namespace PhotoFiler.Web.Helpers
{
    public static class PhotoFilerExtensions
    {
        public static IServiceCollection AddPhotoFiler(this IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            return services
                .AddTransient<IExifReaderService, ImageServices>()
                .AddTransient<IImageResizerService, ImageServices>()
                .AddSingleton<PhotoFilerConfiguration>(provider => configurationRoot.GetSection("PhotoFiler").Get<PhotoFilerConfiguration>())
                .AddSingleton<IFileSystemConfiguration, PhotoFilerConfiguration>(provider => provider.GetService<PhotoFilerConfiguration>())
                .AddSingleton<IRepository>(provider =>
                {
                    var configuration = provider.GetService<PhotoFilerConfiguration>();
                    var exifReader = provider.GetService<IExifReaderService>();
                    var imageResizer = provider.GetService<IImageResizerService>();
                    IRepository repository = new FileSystemRepository(configuration, exifReader, imageResizer);

                    if (configuration.EnableLogging)
                    {
                        repository =
                            new LoggedRepository(
                                provider.GetService<ILogger>(),
                                repository
                            );
                    }

                    return repository;
                })
                .AddSingleton<IPhotosRepository>(provider => provider.GetService<IRepository>().CreatePhotosRepository())
                .AddSingleton<IAlbumRepository>(provider => provider.GetService<IRepository>().CreateAlbumRepository())
                .AddSingleton<IPreviewablePhotos>(provider => provider.GetService<IPhotosRepository>().Create())
                .AddSingleton<List<IPreviewablePhoto>>(provider =>
                {
                    return provider.GetService<IPreviewablePhotos>()
                        .Retrieve(
                            (sender, args) =>
                            {
                                provider.GetService<ILogger>()?
                                    .LogError(args.Exception, "Error generating preview for photo \"{0}\"", args.Photo.Location);
                            });
                })
                .AddSingleton<IHashedAlbum>(provider =>
                {
                    var photos = provider.GetService<IPreviewablePhotos>().Retrieve();
                    return provider.GetService<IAlbumRepository>().Create(photos);
                });
        }

        public static IServiceCollection AddPhotoFiler(this IServiceCollection services, IConfigurationRoot configurationRoot, ILogger logger)
        {
            return services
                .AddSingleton<PhotoFilerConfiguration>(provider => configurationRoot.GetSection("PhotoFiler").Get<PhotoFilerConfiguration>())
                .AddSingleton<IFileSystemConfiguration, PhotoFilerConfiguration>(provider => provider.GetService<PhotoFilerConfiguration>())
                .AddSingleton<IRepository>(provider =>
                {
                    var configuration = provider.GetService<PhotoFilerConfiguration>();
                    var exifReader = provider.GetService<IExifReaderService>();
                    var imageResizer = provider.GetService<IImageResizerService>();
                    IRepository repository = new FileSystemRepository(configuration, exifReader, imageResizer);

                    if (configuration.EnableLogging)
                    {
                        repository = new LoggedRepository(logger, repository);
                    }

                    return repository;
                })
                .AddSingleton<IPhotosRepository>(provider => provider.GetService<IRepository>().CreatePhotosRepository())
                .AddSingleton<IAlbumRepository>(provider => provider.GetService<IRepository>().CreateAlbumRepository())
                .AddSingleton<IPreviewablePhotos>(provider => provider.GetService<IPhotosRepository>().Create())
                .AddSingleton<List<IPreviewablePhoto>>(provider =>
                {
                    return provider.GetService<IPreviewablePhotos>()
                        .Retrieve((sender, args) => logger?.LogError(args.Exception, "Error generating preview for photo \"{0}\"", args.Photo.Location));
                })
                .AddSingleton<IHashedAlbum>(provider =>
                {
                    var photos = provider.GetService<IPreviewablePhotos>().Retrieve();
                    return provider.GetService<IAlbumRepository>().Create(photos);
                });
        }
    }
}