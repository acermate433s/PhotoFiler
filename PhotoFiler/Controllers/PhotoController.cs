using PhotoFiler.Helper;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoFiler.Controllers
{
    public class PhotoController : Controller
    {
        const int DEFAULT_COUNT = 12;

        delegate byte[] FileBytes<T, U>(T input, out T name);

        FileInfoHasher _FileInfoHasher = (FileInfoHasher) System.Web.HttpContext.Current.Application["FileHashes"];

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
            return ImageFile(hash, true, _FileInfoHasher.Preview);
        }

        private ActionResult Retrieve(string hash, bool inline)
        {
            return ImageFile(hash, inline, _FileInfoHasher.View);
        }

        private ActionResult ImageFile(string hash, bool inline, FileBytes<string, string> action)
        {
            string name = "";
            var fileData = action(hash, out name);
            if (fileData == null)
                return new HttpNotFoundResult("Hash not found");

            var cd = new System.Net.Mime.ContentDisposition()
            {
                FileName = name,
                Inline = inline,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            var contentType = MimeMapping.GetMimeMapping(name);

            return File(fileData, contentType);
        }

        [Route("List")]
        public ActionResult List(int page = 1, int count = DEFAULT_COUNT)
        {
            int total = _FileInfoHasher.Items.Count();

            ViewBag.Total = total;
            ViewBag.Previous = page - 1;
            ViewBag.Current = page;
            ViewBag.Max = total / count + (total % count == 0 ? 0 : 1);
            ViewBag.Next = page + 1;
            ViewBag.Count = count;

            return View(_FileInfoHasher.List(page, count));
        }

        [Route("")]
        public ActionResult Gallery(int page = 1, int count = DEFAULT_COUNT)
        {
            int total = _FileInfoHasher.Items.Count();

            ViewBag.Total = total;
            ViewBag.Previous = page - 1;
            ViewBag.Current = page;
            ViewBag.Max = total / count + (total % count == 0 ? 0 : 1);
            ViewBag.Next = page + 1;
            ViewBag.Count = count;

            return View(_FileInfoHasher.List(page, count));
        }
    }
}