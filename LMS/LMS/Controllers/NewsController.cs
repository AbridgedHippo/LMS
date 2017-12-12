using LMS.DataAccess;
using LMS.Models;
using LMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace LMS.Controllers
{
    public class NewsController : Controller
    {

        private NewsRepository nR = new NewsRepository();

        public ActionResult Feed()
        {
            var model = nR.ShowFeed();

            return View(model);
        }

        public ActionResult Create(string title, string breadtext)
        {
            nR.AddToFeed(title, breadtext);
            return View();
        }
    }
}
