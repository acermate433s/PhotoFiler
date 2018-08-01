using Photo.Models;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace PhotoFiler.Controllers
{
    public class PhotoController : Controller
    {
        private const int DEFAULT_PAGE = 1;
        private const int DEFAULT_COUNT = 12;

        private IHashedAlbum Album = (IHashedAlbum) System.Web.HttpContext.Current.Application["Album"];

        public PhotoController()
        {
        }

        [Route("{hash}")]
        public ActionResult Index(string hash)
        {
            if (hash != null)
            {
                return Retrieve(hash, true);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("Download/{hash}")]
        public ActionResult Download(string hash)
        {
            if (hash != null)
            {
                return Retrieve(hash, false);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("Preview/{hash}")]
        public ActionResult Preview(string hash)
        {
            var content = Album.Preview(hash);
            if (content == null)
            {
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

        [Route("Photos")]
        public ActionResult Photos(int page = DEFAULT_PAGE, int count = DEFAULT_COUNT)
        {
            return PartialView(Album.List(page, count));
        }

        [Route("")]
        public ActionResult Gallery(int page = DEFAULT_PAGE, int count = DEFAULT_COUNT)
        {
            return View(Album.List(page, count));
        }
    }
}