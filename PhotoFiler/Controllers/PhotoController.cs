using PhotoFiler.Helper;
using PhotoFiler.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoFiler.Controllers
{
    public class PhotoController : Controller
    {
        const string ROOT_PATH = @"\\ARCHIVE\Volume_1\Public\trash\575 Wallpapers (All 1080p, No watermarks) - Imgur";
        const int MAX_HASH_LENGTH = 4;

        delegate byte[] FileBytes<T, U>(T input, out T name);

        FileInfoHasher _FileInfoHasher = new FileInfoHasher(ROOT_PATH, MAX_HASH_LENGTH);

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

        public ActionResult Download(string hash)
        {
            if (hash != null)
                return Retrieve(hash, false);
            else
                return RedirectToAction("Index", "Home");            
        }

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

        public ActionResult List(int page = 1, int count = 10)
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

        public ActionResult Gallery(int page = 1, int count = 10)
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