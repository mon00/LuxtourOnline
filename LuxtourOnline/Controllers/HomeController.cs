﻿using LuxtourOnline.Models;
using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Facebook;
using System.Dynamic;

namespace LuxtourOnline.Controllers
{
    [OutputCache(Location = System.Web.UI.OutputCacheLocation.Server, VaryByParam = "language", Duration = 60)]
    public class HomeController : BaseAppController, IDisposable
    {
        protected SiteDbContext _currentContext = null;

        protected string _fbToken = "access_token=1592303977741978|idYp70kTD_eA9FeofzGN6LrLkmo";
        protected string _fbHttps = "https://graph.facebook.com/v2.8";
        protected string _fbGroup = "luxtour.online/feed?fields=message,story,picture,link,likes,full_picture,actions";

        public HomeController(SiteDbContext newContext)
        {
            if (_currentContext == null)
                _currentContext = newContext;
        }

        public HomeController()
        {
            if (_currentContext == null)
                _currentContext = new SiteDbContext();
        }

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Tours()
        {

            return View();
        }

        protected int _newsCount = 10;

        [HttpGet]
        public ActionResult News(int? page, int? count)
        {
            if (page == null || page < 1)
                page = 1;

            if (count == null || count < 1)
                count = _newsCount;


            ViewBag.Page = (int)page;
            ViewBag.Count = (int)count;

            return View();
        }

        [OutputCache(NoStore = true, Duration = 0, Location = System.Web.UI.OutputCacheLocation.None)]
        public async Task<ActionResult> GetNews(int page, int count)
        {
            if (page < 1)
                page = 1;

            if (count < 1)
                count = _newsCount;

            string st;
            string url = $"{_fbHttps}/{_fbGroup}&offset={(page - 1) * count}&limit={count}&{_fbToken}";

            using (WebClient client = new WebClient())
            {

                st = await client.DownloadStringTaskAsync(url);
            }

            dynamic result = new JavaScriptSerializer().DeserializeObject(st);

            return Json(result["data"], JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (_currentContext != null)
                _currentContext.Dispose();

            base.Dispose(disposing);

        }
    }
}