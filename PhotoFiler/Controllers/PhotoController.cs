using PhotoFiler.Helper;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoFiler.Controllers
{
    public class PhotoController : Controller
    {
        const int DEFAULT_COUNT = 12;

        delegate byte[] FileBytes<T, U>(T input);

        PhotosPreviewer _Photos = (PhotosPreviewer) System.Web.HttpContext.Current.Application["Photos"];

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
            return ImageFile(hash, true, _Photos.Preview);
        }

        private ActionResult Retrieve(string hash, bool inline)
        {
            return ImageFile(hash, inline, _Photos.View);
        }

        private ActionResult ImageFile(string hash, bool inline, FileBytes<string, string> action)
        {
            var fileData = action(hash);
            if (fileData == null)
                return new HttpNotFoundResult("Hash not found");

            var name = _Photos[hash].Name;
            var cd = new System.Net.Mime.ContentDisposition()
            {
                FileName = hash + "." + Path.GetExtension(name),
                Inline = inline,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            var contentType = MimeMapping.GetMimeMapping(name);

            return File(fileData, contentType);
        }

        [Route("List")]
        public ActionResult List(int page = 1, int count = DEFAULT_COUNT)
        {
            int total = _Photos.Count();

            ViewBag.Total = total;
            ViewBag.Previous = page - 1;
            ViewBag.Current = page;
            ViewBag.Max = total / count + (total % count == 0 ? 0 : 1);
            ViewBag.Next = page + 1;
            ViewBag.Count = count;

            return View(_Photos.List(page, count));
        }

        [Route("")]
        public ActionResult Gallery(int page = 1, int count = DEFAULT_COUNT)
        {
            int total = _Photos.Count();

            ViewBag.Total = total;
            ViewBag.Previous = page - 1;
            ViewBag.Current = page;
            ViewBag.Max = total / count + (total % count == 0 ? 0 : 1);
            ViewBag.Next = page + 1;
            ViewBag.Count = count;

            return View(_Photos.List(page, count));
        }
    }
}