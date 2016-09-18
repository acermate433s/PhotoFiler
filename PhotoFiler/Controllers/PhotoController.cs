using PhotoFiler.Models;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Telemetry;

namespace PhotoFiler.Controllers
{
    public class PhotoController : Controller
    {
        private const int DEFAULT_PAGE = 1;
        private const int DEFAULT_COUNT = 12;

        private IHashedAlbum Album = (IHashedAlbum) System.Web.HttpContext.Current.Application["Album"];
        private ILogger Logger = (ILogger) System.Web.HttpContext.Current.Application["Logger"];

        public PhotoController()
        {
        }

        [Route("{hash}")]
        public ActionResult Index(string hash)
        {
            using (var logger = Logger.Create($"PhotoController.Index(hash = \"{hash}\")"))
            {
                logger.Information(Request.Url.AbsoluteUri);

                if (hash != null)
                {
                    logger.Information("Retrieve photo with hash \"{0}\".", hash);
                    return Retrieve(hash, true);
                }
                else
                {
                    logger.Warning("Hash cannot be null.");
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        [Route("Download/{hash}")]
        public ActionResult Download(string hash)
        {
            using (var logger = Logger.Create($"PhotoController.Download(hash = \"{hash}\")"))
            {
                logger.Information(Request.Url.AbsoluteUri);

                if (hash != null)
                {
                    logger.Information("Retrieve photo with hash \"{0}\".", hash);
                    return Retrieve(hash, false);
                }
                else
                {
                    logger.Warning("Hash cannot be null.");
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        [Route("Preview/{hash}")]
        public ActionResult Preview(string hash)
        {
            using (var logger = Logger.Create($"PhotoController.Preview(hash = \"{hash}\")"))
            {
                logger.Information(Request.Url.AbsoluteUri);

                var content = Album.Preview(hash);
                if (content == null)
                {
                    logger.Warning("Cannot find preview for photo with hash {0}", hash);
                    return new HttpNotFoundResult($"Cannot find preview for photo with \"{hash}\"");
                }

                var result =
                    ImageFile(
                        hash,
                        true,
                        Album.Photo(hash),
                        content
                    );

                return result;
            }
        }

        private ActionResult Retrieve(string hash, bool inline)
        {
            var content = Album.View(hash);
            if (content == null)
                return new HttpNotFoundResult($"Cannot find view for \"{hash}\"");

            var result = 
                ImageFile(
                    hash, 
                    inline, 
                    Album.Photo(hash), 
                    content
                );

            return result;
        }

        private ActionResult ImageFile(
            string hash, 
            bool inline, 
            IPhoto photo, 
            byte[] content
        )
        {
            if ((photo == null) || (content == null))
                return new HttpNotFoundResult("Photo not found");

            var name = photo.Name;
            var cd = new System.Net.Mime.ContentDisposition()
            {
                FileName = hash + "." + Path.GetExtension(name),
                Inline = inline,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            var contentType = MimeMapping.GetMimeMapping(name);

            return File(content, contentType);
        }

        [Route("List")]
        public ActionResult List(int page = DEFAULT_PAGE, int count = DEFAULT_COUNT)
        {
            using (var logger = Logger.Create($"PhotoController.List(page = {page}, count = {count})"))
            {
                int total = Album.Count();

                ViewBag.Total = total;
                ViewBag.Previous = page - 1;
                ViewBag.Current = page;
                ViewBag.Max = total / count + (total % count == 0 ? 0 : 1);
                ViewBag.Next = page + 1;
                ViewBag.Count = count;
            
                logger.Information(Request.Url.AbsoluteUri);

                return View(Album.List(page, count));
            }
        }

        [Route("")]
        public ActionResult Gallery(int page = DEFAULT_PAGE, int count = DEFAULT_COUNT)
        {
            using (var logger = Logger.Create($"PhotoController.Gallery(page = {page}, count = {count}"))
            {
                int total = Album.Count();
                    
                ViewBag.Total = total;
                ViewBag.Previous = page - 1;
                ViewBag.Current = page;
                ViewBag.Max = total / count + (total % count == 0 ? 0 : 1);
                ViewBag.Next = page + 1;
                ViewBag.Count = count;
            
                logger.Information(Request.Url.AbsoluteUri);
                    
                return View(Album.List(page, count));
            }
        }
    }
}