using PhotoFiler.Helper;
using PhotoFiler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoFiler.Controllers
{
    public class PhotoController : Controller
    {
        const string ROOT_PATH = @"\\ARCHIVE\Volume_1\Public\trash\575 Wallpapers (All 1080p, No watermarks) - Imgur";
        const int MAX_HASH_LENGTH = 4;

        FileInfoHasher _FileInfoHasher = new FileInfoHasher(ROOT_PATH, MAX_HASH_LENGTH);

        public PhotoController()
        {

        }

        public ActionResult Index(string hash)
        {
            return Retrieve(hash, true);
        }

        public ActionResult Download(string hash)
        {
            return Retrieve(hash, false);
        }

        private ActionResult Retrieve(string hash, bool inline)
        {
            if (!_FileInfoHasher.ContainsHash(hash))
                return new HttpNotFoundResult("Hash not found");

            var fileInfo = _FileInfoHasher[hash];
            var cd = new System.Net.Mime.ContentDisposition()
            {
                FileName = fileInfo.FullName,
                Inline = inline,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            var fileData = System.IO.File.ReadAllBytes(fileInfo.FullName);
            var contentType = MimeMapping.GetMimeMapping(fileInfo.FullName);

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

            var value =
                _FileInfoHasher
                    .Items
                    .Select(item =>
                        new FileHash()
                        {
                            Hash = item.Key,
                            Name = item.Value.Name,
                            Size =
                                (new Func<long, string>(
                                    (length) =>
                                        {
                                            var suffixes = new[] { "bytes", "KB", "MB", "GB" };
                                            int index = 0;

                                            while(length > 1024)
                                            {
                                                length /= 1024;
                                                index++;
                                            }

                                            return String.Format("{0}{1}", length, suffixes[index]);
                                        }
                                    )
                                )
                                .Invoke(item.Value.Length),
                        }
                    )
                    .OrderBy(item => item.Name)
                    .Skip((page - 1) * count)
                    .Take(count);

            return View(value);
        }
    }
}