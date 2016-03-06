using PhotoFiler.Helper;
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

        [Route("Photo/{hash}")]
        public ActionResult Index(string hash)
        {
            var hasher = new FileInfoHasher(ROOT_PATH, MAX_HASH_LENGTH);
            if (!hasher.ContainsHash(hash))
                return new HttpNotFoundResult("Hash not found");

            var fileInfo = hasher[hash];
            var cd = new System.Net.Mime.ContentDisposition()
            {
                FileName = fileInfo.FullName,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            var fileData = System.IO.File.ReadAllBytes(fileInfo.FullName);
            var contentType = MimeMapping.GetMimeMapping(fileInfo.FullName);

            return File(fileData, contentType);
        }
    }
}