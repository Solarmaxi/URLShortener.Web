using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using URLShortener.Web.Models;
using System.Configuration;

namespace URLShortener.Web.Controllers
{
    public class HomeController : Controller
    {
        private URLShortener.Client.UrlShortenerHttpClient client = new URLShortener.Client.UrlShortenerHttpClient(ConfigurationManager.AppSettings["URLShortenerAPI"]);

        [HttpGet]
        public ActionResult Index(string shortUrl)
        {
            return View(new IndexViewModel());
        }

        [HttpPost]
        public ActionResult Index(IndexViewModel model)
        {
            try
            {
                model.shortUrl = String.Format("{0}{1}", Request.Url.AbsoluteUri, client.GetTinyUrl(model.longUrl));

                return View(model);
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Relocate(string shortUrl)
        {
            try
            {
                var url = client.GetLongUrl(shortUrl);
                if (string.IsNullOrEmpty(url) is false)
                    return new RedirectResult(url);
                else
                {
                    TempData["error"] = "Url not found!";
                    return RedirectToAction("Index", "Home");
                }

            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
                return RedirectToAction("Index", "Home");
            }
        }
    }
}