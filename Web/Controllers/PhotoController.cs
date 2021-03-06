﻿using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using PhotoFiler.Photo;
using PhotoFiler.Photo.Models;

namespace PhotoFiler.Web.Controllers
{
    public class PhotoController : Controller
    {
        private const int DEFAULT_PAGE = 1;
        private const int DEFAULT_COUNT = 12;

        private readonly IHashedAlbum album;

        public PhotoController(IHashedAlbum album)
        {
            this.album = album;
        }

        [Route("{hash}")]
        public ActionResult Index(Hash hash)
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
        public ActionResult Download(Hash hash)
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
        public ActionResult Preview(Hash hash)
        {
            var content = album.Preview(hash);
            if (content == null)
            {
                return new HttpNotFoundResult($"Cannot find preview for photo with \"{hash}\"");
            }

            var result =
                ImageFile(
                    hash,
                    true,
                    album.Photo(hash),
                    content
                );

            return result;
        }

        private ActionResult Retrieve(Hash hash, bool inline)
        {
            var content = album.View(hash);
            if (content == null)
                return new HttpNotFoundResult($"Cannot find view for \"{hash}\"");

            var result =
                ImageFile(
                    hash,
                    inline,
                    album.Photo(hash),
                    content
                );

            return result;
        }

        private ActionResult ImageFile(
            Hash hash,
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
            return PartialView(album.List(page, count));
        }

        [Route("")]
        public ActionResult Gallery(int page = DEFAULT_PAGE, int count = DEFAULT_COUNT)
        {
            return View(album.List(page, count));
        }
    }
}