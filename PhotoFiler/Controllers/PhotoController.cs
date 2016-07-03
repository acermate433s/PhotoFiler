using PhotoFiler.Helpers;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace PhotoFiler.Controllers
{
    public class PhotoController : Controller
    {
        private const int DEFAULT_COUNT = 12;

        private delegate byte[] FileBytes<T, U>(T input);

        private MD5HashedAlbum Album = (MD5HashedAlbum) System.Web.HttpContext.Current.Application["Album"];

        public PhotoController()
        {
        }

        [Route("{hash}")]
        public ActionResult Index(string hash)
        {
            if (hash != null)
                return Retrieve(hash, true);
            else
                return RedirectToAction("Index", "Home");
        }

        [Route("Download/{hash}")]
        public ActionResult Download(string hash)
        {
            if (hash != null)
                return Retrieve(hash, false);
            else
                return RedirectToAction("Index", "Home");
        }

        [Route("Preview/{hash}")]
        public ActionResult Preview(string hash)
        {
            return ImageFile(hash, true, Album.Photo(hash), Album.Preview(hash));
        }

        private ActionResult Retrieve(string hash, bool inline)
        {
            return ImageFile(hash, inline, Album.Photo(hash), Album.View(hash));
        }

        private ActionResult ImageFile(string hash, bool inline, IPhoto photo, byte[] content)
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
        public ActionResult List(int page = 1, int count = DEFAULT_COUNT)
        {
            int total = Album.Count();

            ViewBag.Total = total;
            ViewBag.Previous = page - 1;
            ViewBag.Current = page;
            ViewBag.Max = total / count + (total % count == 0 ? 0 : 1);
            ViewBag.Next = page + 1;
            ViewBag.Count = count;

            return View(Album.List(page, count));
        }

        [Route("")]
        public ActionResult Gallery(int page = 1, int count = DEFAULT_COUNT)
        {
            int total = Album.Count();

            ViewBag.Total = total;
            ViewBag.Previous = page - 1;
            ViewBag.Current = page;
            ViewBag.Max = total / count + (total % count == 0 ? 0 : 1);
            ViewBag.Next = page + 1;
            ViewBag.Count = count;

            return View(Album.List(page, count));
        }
    }
}